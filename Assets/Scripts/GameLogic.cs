using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    /*
      We want to make sure there can be only ONE! instance of our GameLogic. 
      We are creating a private static instance of our GameLogic and a public static Property to control the instance


     */




     private static GameLogic _gameLogicInstance;
    public static GameLogic GameLogicInstance //This static reference, determines the properties of the NetworkManager
    {
        //public get means you can read and get from anywhere
        //private means this can only set from here, no where else
        //TLDR; This script is READONLY when other scripts try to utilize it.

        //Property Read is public by default and reads the instance
        get => _gameLogicInstance;

        private set
        {
            //Property private write sets instance to the value if the instance is null
            if (_gameLogicInstance == null)
            {
                //if there is no other instance, then set this as the instance we wil use
                _gameLogicInstance = value;
            }
            else if (_gameLogicInstance != value)
            {
                //the $ in the string is for formatting
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroy duplicate! THERE CAN ONLY BE ONE!");
                //Destroys the script if this is not the initial instance
                Destroy(value);
            }
        }

    }

   [SerializeField] private GameObject _playerPrefab;
    public GameObject PlayerPrefab => _playerPrefab;

    private void Awake()
    {
        //sets the singleton to this
        GameLogicInstance = this;
    }

}
