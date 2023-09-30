using Unity.Netcode;
using UnityEngine;
using World_Manager;

namespace Menu_Screen
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            // this coroutine runs the async function LoadNewGame in the WSGM script
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }
}