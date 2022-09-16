using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        PlayerInputs _playerInputs;
        Vector2 _movementInputs;
        Vector2 _lookInput;
        bool _jumpInput = false;
        bool _isAiming;

        public Vector2 MovementInputs => _movementInputs;

        public Vector2 LookInput => _lookInput;

        public bool JumpInput => _jumpInput;

        public bool IsAiming => _isAiming;


        #region Setup

        void Awake()
        {
            _playerInputs = new PlayerInputs();
        }

        void OnEnable()
        {
            _playerInputs.Enable();
        }

        void OnDisable()
        {
            _playerInputs.Disable();
        }

        #endregion

        void Start()
        {
            _playerInputs.Main.Jump.performed += ctx => _jumpInput = ctx.ReadValue<float>() > 0.1f ? true : false;
            _playerInputs.Main.Move.performed += ctx => _movementInputs = ctx.ReadValue<Vector2>();
            _playerInputs.Main.Move.canceled += ctx => _movementInputs *= 0;
            _playerInputs.Main.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();

            _playerInputs.Main.FirstPersonCamera.performed += ctx => _isAiming = ctx.ReadValue<float>() > 0.1f ? true : false;
        }
    }
}