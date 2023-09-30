namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        private PlayerLocomotionManager _playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();
            // DO MORE STUFF ONLY FOR THE PLAYER !
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();


            if (!IsOwner)
                return;

            _playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) return;

            base.LateUpdate();
            PlayerCamera.Instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner) PlayerCamera.Instance.player = this;
        }
    }
}