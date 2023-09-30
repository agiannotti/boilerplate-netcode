using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World_Manager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        // WorldSaveGameManager Singleton
        public static WorldSaveGameManager Instance;

        [SerializeField] private int worldSceneIndex = 1;

        public void Awake()
        {
            // THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPT, IF ANOTHER EXISTS, DESTROY IT
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            // Persists save game manager instance through different scenes
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame()
        {
            var loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}