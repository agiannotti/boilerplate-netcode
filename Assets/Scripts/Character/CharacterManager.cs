using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        private CharacterNetworkManager _characterNetworkManager;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            // if this char is being controlled on our side ( owner ), then assign network position to position of transform
            if (IsOwner)
            {
                var transform1 = transform;
                _characterNetworkManager.networkPosition.Value = transform1.position;
                _characterNetworkManager.networkRotation.Value = transform1.rotation;
            }
            // if this char is being controlled elsewhere, assign position locally by position of network transform
            else
            {
                // position
                Transform transform1;
                (transform1 = transform).position = Vector3.SmoothDamp
                (transform.position,
                    _characterNetworkManager.networkPosition.Value,
                    ref _characterNetworkManager.networkPositionVelocity,
                    _characterNetworkManager.networkPositionSmoothTime);

                // rotation
                transform.rotation = Quaternion.Slerp(
                    transform1.rotation,
                    _characterNetworkManager.networkRotation.Value,
                    _characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
        }
    }
}