using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 1;
        [SerializeField] private float leftAndRightRotationSpeed = 75;
        [SerializeField] private float upAndDownRotationSpeed = 1;
        [SerializeField] private float minimumPivot = -30; // lowest point we can look down
        [SerializeField] private float maximumPivot = 60; // highest point we can look up
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask collideWithLayers;

        [Header("Camera Values")]
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        private Vector3
            _cameraObjectPosition; // used for camera collisions ( moves cam object to this position upon colliding )
        private Vector3 _cameraVelocity;
        private float _cameraZPosition; // values used for camera collisions
        private bool _isplayerNotNull;
        private float _targetCameraZPosition; // values used for camera collisions

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
            _cameraZPosition = cameraObject.transform.localPosition.z;
        }

        private void HandleCollisions()
        {
            _targetCameraZPosition = _cameraZPosition;
            RaycastHit hit;
            // direction for collision check
            var direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // we check if there is an object in front of our desired direction ^
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit,
                    Mathf.Abs(_targetCameraZPosition), collideWithLayers)) ;
            {
                // if there is, we get our distance from it
                var distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // we then equate our target z position to the following
                _targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // if our target position is less than our collision radius, we subtract our collision radius ( making it snap back! )
            if (Mathf.Abs(_targetCameraZPosition) < cameraCollisionRadius)
                _targetCameraZPosition = -cameraCollisionRadius;

            // we then apply our final position using a lerp over a time of 0.2f
            _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = _cameraObjectPosition;
        }

        public void HandleAllCameraActions()
        {
            // FOLLOW PLAYER
            // ROTATE AROUND PLAYER
            // COLLIDE WITH OBJECTS
            if (!_isplayerNotNull) return;
            HandleFollowTarget();
            HandleRotation();

            // FIXME: Collisions not working :(
            // HandleCollisions();
        }

        private void HandleFollowTarget()
        {
            var targetCameraPosition = Vector3.SmoothDamp(
                transform.position,
                player.transform.position,
                ref _cameraVelocity,
                cameraSmoothSpeed * Time.deltaTime);
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