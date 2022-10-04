using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;

namespace WormsGame.Movement
{
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] float _gravity = -15.0f;
        [SerializeField] float _heavierGravity = -30.0f;
        [SerializeField] float _gravityMultiplier = 2;
        [SerializeField] float groundedRadius = 0.5f;
        [SerializeField] LayerMask _groundLayers;
        
        float _verticalVelocity;
        
        //cached
        InputHandler _inputHandler;

        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
        }

        void Update()
        {
            ChangeVelocityByGravity();
        }

        public Vector3 GetVerticalVelocity()
        {
            return new Vector3(0.0f, _verticalVelocity, 0.0f);
        }

        void ChangeVelocityByGravity()
        {
            float pushDownMultiplier = _verticalVelocity < 0.0f ? _gravityMultiplier : 1;
            float gravityToUse = _gravity;
            if (_verticalVelocity < 0.0f)
            {
                if (IsGrounded())
                {
                    _verticalVelocity = -2.0f;
                    pushDownMultiplier = 1f;
                }

                gravityToUse *= pushDownMultiplier;
            }
            else if (!IsGrounded() && _inputHandler.enabled && !_inputHandler.JumpInput)
            {
                gravityToUse = _heavierGravity;
            }


            _verticalVelocity += gravityToUse * Time.deltaTime;
        }

        public void ApplyJumpVelocity(float heightToReach)
        {
            _verticalVelocity = Mathf.Sqrt(_gravity * heightToReach * -2f);

        }
        public bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position, groundedRadius, _groundLayers);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, groundedRadius);
            
        }
    }
    
}
