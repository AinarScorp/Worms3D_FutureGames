using System;
using UnityEngine;
using Main.Inputs;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace Main.Movement
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerController : MonoBehaviour
    {
        #region variables

        [Header("PlayerSettings")] 
        [SerializeField] float _moveSpeed = 2f;

        [SerializeField] float _jumpForce = 5f;

        [Header("Rotation")] 
        [SerializeField] float _rotationSmoothTime = 0.1f;

        [Header("Camera Settings")] 
        [SerializeField] Transform _cameraTarget;

        [SerializeField] float cameraTopClamp = 50f, cameraBottomClamp = -50f;
        [SerializeField] float _cameraRotationSpeed = 0.5f;

        [Header("Physics")] 
        [SerializeField] float _gravity = -15;

        
        //Physics
        float _verticalVelocity;
        
        //cached
        InputHandler _inputHandler;
        CharacterController _characterController;

        // Camera Cinemachine
        Transform _cameraMain;
        float _cameraYaw, _cameraPitch;
        float _turnSmoothVelocity;

        #endregion

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputHandler = GetComponent<InputHandler>();
            _cameraMain = Camera.main.transform;
        }

        void Start()
        {
            _cameraYaw = _cameraTarget.rotation.eulerAngles.y;
        }

        void Update()
        {
            HandleMovement();
            Jump();
        }


        void LateUpdate()
        {
            HandleCameraRotation();
        }

        void HandleMovement()
        {
            if (_inputHandler.MovementInputs == Vector2.zero) return;

            Vector3 moveInputs = new Vector3(_inputHandler.MovementInputs.x, 0.0f, _inputHandler.MovementInputs.y).normalized;
            float targetAngle = 0;

            targetAngle = CalculateTargetAngle(moveInputs);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDirection.normalized * (_moveSpeed * Time.fixedDeltaTime));
        }

        void Jump()
        {
            if (!_inputHandler.JumpInput) return;
            
            
        }

        void HandleCameraRotation()
        {
            if (_inputHandler.LookInput.sqrMagnitude >= 0.01f)
            {
                _cameraYaw += _inputHandler.LookInput.x * _cameraRotationSpeed;
                _cameraPitch += _inputHandler.LookInput.y * _cameraRotationSpeed;
            }

            _cameraYaw = ClampAngle(_cameraYaw, float.MinValue, float.MaxValue);
            _cameraPitch = ClampAngle(_cameraPitch, cameraBottomClamp, cameraTopClamp);
            _cameraTarget.rotation = Quaternion.Euler(-_cameraPitch, _cameraYaw, 0.0f);
        }

        float CalculateTargetAngle(Vector3 moveInputs)
        {
            float targetAngle;
            targetAngle = Mathf.Atan2(moveInputs.x, moveInputs.z) * Mathf.Rad2Deg + _cameraMain.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            return targetAngle;
        }

        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}