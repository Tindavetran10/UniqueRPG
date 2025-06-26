using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;


    [HideInInspector] public PlayerUIHudManager playerUIHudManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            // Must shutdown, because the game started as a host during the title screen
            NetworkManager.Singleton.Shutdown();
            // Then restart, as a Client
            NetworkManager.Singleton.StartClient();
        }
    }
}
