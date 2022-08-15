using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;


public enum ClientToServerId //Puts the enum outside of the class, 
{
    //sets the value to 1, the default is 0
    name = 1
}

public class NetworkManager : MonoBehaviour
{
    /* 
    We want to make sure there can be only ONE! instance of our network manager. 
    We are creating a private static instance of our NetworkManager and a public static Property to control the instance

    */

    private static NetworkManager _networkManagerInstance;
    public static NetworkManager NetworkManagerInstance //This static reference, determines the properties of the NetworkManager
    {
        //public get means you can read and get from anywhere
        //private means this can only set from here, no where else
        //TLDR; This script is READONLY when other scripts try to utilize it.

        //Property Read is public by default and reads the instance
        get => _networkManagerInstance;

        private set
        {
            //Property private write sets instance to the value if the instance is null
            if (_networkManagerInstance == null)
            {
                //if there is no other instance, then set this as the instance we wil use
                _networkManagerInstance = value;
            }
            else if (_networkManagerInstance != value)
            {
                //the $ in the string is for formatting
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroy duplicate! THERE CAN ONLY BE ONE!");
                //Destroys the script if this is not the initial instance
                Destroy(value);
            }
        }

    }


    public Server GameServer { get; private set; }
    //ushort is an unsigned short int (16bit int)
    //for servers we ensure we can not go out of range of the protocolls
    [SerializeField] private ushort s_port; //s_port references the server port
    [SerializeField] private ushort s_maxClientCount;

    //We use the awake function so it is able to connect to other scripts before running, this is used to avoid errors such as null and reference errors
    //Awake runs the first time the object is active in the scene, Awake will run even if the script is inactive and only the object is active, We can use the awake function to run turn the script on
    private void Awake() 
    {
        //When the object that this script is attachd to is active in the game, set the instance to this... an check to see if instance is already set
        NetworkManagerInstance = this;
    }

    private void Start()
    {
        //Log what the network is doing
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false); //This is inbuilt and will automatically check things, the false is for timestamps but we will need to format the time in order to use it.

        //Create new server 
        GameServer = new Server();
        // starts the server at port XXXX with X ammount of clients (X is used as we are unsure of the actual amount just yet)
        GameServer.Start(s_port, s_maxClientCount);
        //when a client leaves the server run the PlayerLeft function
        GameServer.ClientDisconnected += PlayerLeft;
        //We are adding an event to a function, not running it which is why we do use brackets for PlayerLeft, (No PlayerLeft();)
        //When a client is disconnected, run the PlayerLeft function.
    }

    //Checking Server activity at set intervals
    private void FixedUpdate()
    {
        GameServer.Tick();
    }
    //When the game closes, It kills the connection to the servers.
    private void OnApplicationQuit()
    {
        GameServer.Stop();
    }
    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        //When a player leaves the server, Destroy the player object and remove from list

        //Destroy(Player.list[e.Id].gameObject); //We will use this once the Player script is made.
    }

}
