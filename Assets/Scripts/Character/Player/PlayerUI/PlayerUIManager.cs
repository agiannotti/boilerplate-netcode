using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    private static PlayerUIManager _instance;

    [Header("NETWORK JOIN")] [SerializeField]
    private bool startGameAsClient;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;

            // WE MUST FIRST SHUT DOWN, BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
            NetworkManager.Singleton.Shutdown();

            // WE THEN RESTART, AS A CLIENT

            NetworkManager.Singleton.StartClient();
        }
    }
}