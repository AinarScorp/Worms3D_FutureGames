using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormsGame.Movement
{
    public class PhysicsController : MonoBehaviour
    {
        CharacterController _characterController;
        PlayerController _playerController;
        CustomGravity _customGravity;
        ImpactKnockback _impactKnockback;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerController = GetComponent<PlayerController>();
            _customGravity = GetComponent<CustomGravity>();
            _impactKnockback = GetComponent<ImpactKnockback>();

        }

        void Update()
        {
            SupplyCharacterController();
        }

        void SupplyCharacterController()
        {
            _characterController.Move(GetInputMotion() + GetGravityMotion() + GetImpactMotion());
        }

        Vector3 GetImpactMotion()
        {
            return _impactKnockback.Impact * Time.deltaTime;
        }
        Vector3 GetGravityMotion()
        {
            return _customGravity.GetVerticalVelocity() * Time.deltaTime;
        }
        Vector3 GetInputMotion()
        {
            _playerController.GetDirectionAndSpeed(out Vector3 moveDirection, out float horizontalSpeed);
            if (!_playerController.enabled) return Vector3.zero;
  
            return moveDirection * (horizontalSpeed * Time.deltaTime);
        }
    }
    
}
