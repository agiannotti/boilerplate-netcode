using UnityEngine;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        [SerializeField] public Vector2 movementInput;
        [SerializeField] public Vector2 cameraInput;

        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        private PlayerControls _playerControls;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;
        }

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                _playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            _playerControls.Enable();
        }

        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled)
            {
                if (hasFocus)
                    _playerControls.Enable();
                else
                    _playerControls.Disable();
            }
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // RETURNS THE ABSOLUTE VALUE between 0 and 1 && x and y w/o negative sign
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // CLAMP MOVEMENT FOR BETTER FEELIES, clamped run and walk values ( 0, 0.5, or 1 )
            if (moveAmount <= 0.5 && moveAmount > 0)
                moveAmount = 0.5f;
            else if (moveAmount > 0.5 && moveAmount <= 1) moveAmount = 1;
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE PLAYER CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
                Instance.enabled = true;

            // OTHERWISE, WE MUST BE AT THE MAIN MENU, DISABLE CONTROLS
            // THIS IS SO OUR PLAYER CAN'T MOVE AROUND DURING CHARACTER CREATION ETC
            else
                Instance.enabled = false;
        }
    }
}