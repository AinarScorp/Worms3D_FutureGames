using System;
using UnityEngine;
using WormsGame.Inputs;

//To Do: 
//•Think about windMechanic, you don't want to be able to move in the air the same you do in the ground
namespace WormsGame.Movement
{
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(CustomGravity))]
    public class PlayerController : MonoBehaviour
    {
        #region Exposed variables

        [Header("PlayerSettings")] //
        [SerializeField] float _moveSpeed = 2f;

        
        [Header("Rotation")] //
        [SerializeField] float _rotationSmoothTime = 0.1f;

        
        [Header("Camera Settings")] //
        [SerializeField] Transform _thirdPersonCamTarget;
        [SerializeField] Transform _firstPersonCamTarget;


        [SerializeField] float cameraTopClamp = 50f, cameraBottomClamp = -50f;
        [SerializeField] float _cameraRotationSpeed = 0.5f;

        
        [Header("Jumping")] //
        [SerializeField] float _heightToReach = 5f;
        [SerializeField] float _windMultilpier = 0.5f;

        [SerializeField] float _jumpCooldown = 0.5f;
        

        [Header("Animations")] 
        [SerializeField] Animator _animator;
        #endregion

        #region variables

        //Rotations
        float _targetAngle;
        bool rotateOnMove = false;

        //Jumping
        float _jumpCooldownTimer;
        
        // Camera Cinemachine
        Transform _cameraMain;
        float _cameraYaw, _cameraPitch;
        float _turnSmoothVelocity;

        //cached
        CustomGravity _customGravity;
        InputHandler _inputHandler;

        #endregion

        #region Properties
        public Transform ThirdPersonCamTarget => _thirdPersonCamTarget;
        public Transform FirstPersonCamTarget => _firstPersonCamTarget;

        #endregion


        #region Unity Logic

        void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            
            _inputHandler = GetComponent<InputHandler>();
            _customGravity = GetComponent<CustomGravity>();
            _cameraMain = Camera.main.transform;
            
            _inputHandler.SubscribeToActivation(() => this.enabled = true, true);
            _inputHandler.SubscribeToActivation(() => this.enabled = false, false);
            
        }
        

        void Start()
        {
            _cameraYaw = _thirdPersonCamTarget.rotation.eulerAngles.y;
        }


        void Update()
        {
            HandleRotation();
            Jump();
        }

        void LateUpdate()
        {            
            HandleCameraRotation();
        }

        #endregion

        #region Handlers

        public void GetDirectionAndSpeed(out Vector3 moveDirection,out float horizontalSpeed)
        {
            if (_inputHandler.ShootInput)
            {
                horizontalSpeed = 0;
                moveDirection = Vector3.zero;
                return;
            }
            horizontalSpeed = _inputHandler.MovementInputs == Vector2.zero || _inputHandler.IsAiming ? 0.0f : _moveSpeed;
            if (!_customGravity.IsGrounded())
            {
                horizontalSpeed *= _windMultilpier;
            }

            moveDirection = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;
            moveDirection.Normalize();

            if (_animator != null )
                _animator.SetBool("isMoving", horizontalSpeed > 0.1f);


        }

        void HandleRotation()
        {
            if (_inputHandler.MovementInputs != Vector2.zero || rotateOnMove)
            {
                float moveInputX = rotateOnMove ? 0.0f : _inputHandler.MovementInputs.x;
                float moveInputY = rotateOnMove ? 0.0f : _inputHandler.MovementInputs.y;
                Vector3 moveInputs = new Vector3(moveInputX, 0.0f, moveInputY).normalized;

                _targetAngle = Mathf.Atan2(moveInputs.x, moveInputs.z) * Mathf.Rad2Deg + _cameraMain.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
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

            if (_inputHandler.IsAiming)
                _firstPersonCamTarget.rotation = Quaternion.Euler(-_cameraPitch, _cameraYaw, 0.0f);
            else
                _thirdPersonCamTarget.rotation = Quaternion.Euler(-_cameraPitch, _cameraYaw, 0.0f);
        }


        void Jump()
        {
            if (_jumpCooldownTimer >= 0.0f) _jumpCooldownTimer -= Time.deltaTime;
            if (_inputHandler.ShootInput) return;
            if (!_inputHandler.JumpInput) return;
            if (_inputHandler.IsAiming)return;
            if (!_customGravity.IsGrounded() || _jumpCooldownTimer >= 0.0f) return;
            _customGravity.ApplyJumpVelocity(_heightToReach);
            _jumpCooldownTimer = _jumpCooldown;
        }

        

        #endregion


        public void MatchCameras()
        {
            _firstPersonCamTarget.rotation = _thirdPersonCamTarget.rotation;
        }


        public void SetRotateOnMove(bool value) => rotateOnMove = value;
        
        float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}

#region Commented out

//[Header("Physics")] //
//[SerializeField] float _gravity = -15;
//[SerializeField] float _heavierGravity = -30;

//[SerializeField] float _gravityMulitplier = 2.0f;
// [SerializeField] float groundedRadius = 0.5f;
// [SerializeField] LayerMask _groundLayers;
//••Physics
//float horizontalSpeed;
//Vector3 moveDirection;
//float _verticalVelocity;


//••••was inside GetDirectionAndSpped
//_characterController.Move(moveDirection.normalized * (horizontalSpeed * Time.deltaTime) +verticalVelocityVector * Time.deltaTime);
//_characterController.Move(moveDirection.normalized * (horizontalSpeed * Time.deltaTime));


// public Transform GetCurrentCameraTransform()
// {
//     return _inputHandler.IsAiming ? _firstPersonCamTarget : _thirdPersonCamTarget;
// }


// bool IsGrounded()
// {
//     return Physics.CheckSphere(transform.position, groundedRadius, _groundLayers);
// }

#endregion