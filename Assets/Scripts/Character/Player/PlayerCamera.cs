using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public PlayerManager player;
        public Camera cameraObject;

        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        [SerializeField] private float leftAndRightRotationSpeed = 220;
        [SerializeField] private float upAndDownRotationSpeed = 220;

        [Header("Camera Settings")]
        private readonly float _cameraSmoothSpeed = 1;


        [Header("Camera Values")]
        private Vector3 _cameraVelocity;

        private bool _isplayerNotNull;

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
            if (!_isplayerNotNull) return;
            HandleFollowTarget();
            HandleRotation();
        }

        private void HandleFollowTarget()
        {
            var targetCameraPosition = Vector3.SmoothDamp(
                transform.position,
                player.transform.position,
                ref _cameraVelocity,
                _cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            // IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
            // ELSE ROTATE REGULARLY
        }
    }
}