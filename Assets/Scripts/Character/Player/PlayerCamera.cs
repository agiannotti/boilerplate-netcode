using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        [SerializeField] private float leftAndRightRotationSpeed = 75;
        [SerializeField] private float upAndDownRotationSpeed = 1;
        [SerializeField] private float minimumPivot = -30; // lowest point we can look down
        [SerializeField] private float maximumPivot = 60; // highest point we can look up

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


            // normal rotations
            // rotate left and right based on horizontal movement
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed *
                                     Time.deltaTime;

            // rotate up and down based on vertical movement
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed *
                                  Time.deltaTime;
            // clamp up and down angle between min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            var cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // rotate THIS gameobject left and right ( camera )
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // rotate pivot gameobject up and down ( camera pivot )
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    }
}