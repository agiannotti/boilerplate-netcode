using UnityEngine;

namespace Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        // THESE VALUES ARE TAKEN FROM PLAYERINPUTMANAGER
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;


        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;
        [SerializeField] private float rotationSpeed = 15;

        private Vector3 _moveDirection;
        private PlayerManager _player;
        private Vector3 _targetRotationDirection;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<PlayerManager>();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;

            // CLAMP MOVEMENTS
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            // AERIAL MOVEMENT
        }

        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();

            var cameraTransform = PlayerCamera.Instance.transform;
            _moveDirection = cameraTransform.forward * verticalMovement;
            _moveDirection += cameraTransform.right * horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            if (PlayerInputManager.Instance.moveAmount > 0.5f)
                _player.characterController.Move(_moveDirection * (runningSpeed * Time.deltaTime));
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
                _player.characterController.Move(_moveDirection * (walkingSpeed * Time.deltaTime));
        }

        private void HandleRotation()
        {
            var cameraObject = PlayerCamera.Instance.cameraObject.transform;

            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = cameraObject.forward * verticalMovement;
            _targetRotationDirection += cameraObject.right * horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0;

            if (_targetRotationDirection == Vector3.zero) _targetRotationDirection = transform.forward;

            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation =
                Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}