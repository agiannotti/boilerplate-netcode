using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public Camera cameraObject;
        public PlayerManager player;

        private readonly float cameraSmoothSpeed = 1;
        private bool _isplayerNotNull;
        private Vector3 cameraVelocity;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _isplayerNotNull = player != null;
            DontDestroyOnLoad(gameObject);
        }

        public void HandleAllCameraActions()
        {
            // FOLLOW PLAYER
            // ROTATE AROUND PLAYER
            // COLLIDE WITH OBJECTS
            if (_isplayerNotNull) HandleFollowTarget();
        }

        private void HandleFollowTarget()
        {
            var targetCameraPosition = Vector3.SmoothDamp(
                transform.position,
                player.transform.position,
                ref cameraVelocity,
                cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }
    }
}