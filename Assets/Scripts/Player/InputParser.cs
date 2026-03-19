using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class InputParser : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        
        private PlayerInput _playerInput;
        private InputActionAsset _inputActionAsset;
        
        private void Awake()
        {
            GetReferences();
            Init();
        }

        private void Update()
        {
            Vector2 moveInput = _inputActionAsset["Move"].ReadValue<Vector2>();
            movement.SetMoveDirection(moveInput);
        }
        
        private void GetReferences()
        {
            _playerInput = GetComponent<PlayerInput>();
        }
        
        private void Init() => _inputActionAsset = _playerInput.actions;
    }
}