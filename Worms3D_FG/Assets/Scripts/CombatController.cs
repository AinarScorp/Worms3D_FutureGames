using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;

namespace WormsGame.Combat
{
    [RequireComponent(typeof(InputHandler))]
    public class CombatController : MonoBehaviour
    {
        [SerializeField] Weapon _currentWeapon;
        [SerializeField] Transform _projectileSpawnPoint; // use frist person camera
        [SerializeField] Transform _handTransform;

        InputHandler _inputHandler;
        bool _hasShot;
        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();

            _inputHandler.SubscribeToActivation(()=>this.enabled = true, true);
            _inputHandler.SubscribeToActivation(()=>this.enabled = false, false);
        }
        
        void OnEnable()
        {
            _hasShot = false;
        }

        void Start()
        {
            _currentWeapon.SpawnWeapon(_handTransform);

        }

        void Update()
        {

            if (!_hasShot && _inputHandler.ShootInput)
            {
                _hasShot = true;
                Vector3 direction = _projectileSpawnPoint.rotation * Vector3.forward;
            
                _currentWeapon.Fire(_projectileSpawnPoint.position, direction);
            }
            else if (!_inputHandler.ShootInput)
            {
                _hasShot = false;
            }
        }


    }
    
    
    
}
