using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;
using WormsGame.Movement;

namespace WormsGame.Combat
{
    [RequireComponent(typeof(InputHandler))]
    public class CombatController : MonoBehaviour
    {
        [SerializeField] Weapon _currentWeapon;
        [SerializeField] Transform _projectileSpawnPoint; // use frist person camera
        //[SerializeField] Transform _handTransform;
        [SerializeField] float chargeSpeed = 5;
        [SerializeField] float _maxLaunchForce = 20.0f;
        
        [SerializeField] float _launchForce = 20.0f;
        InputHandler _inputHandler;
        bool _hasShot;
        bool _weaponIsChargable;
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

        //void Start()
        //{
        //    _currentWeapon.SpawnWeapon(_handTransform);
            
        //}

        Vector3 direction;
        void Update()
        {

            if (!_hasShot && _inputHandler.ShootInput)
            {
                _hasShot = true;
                _weaponIsChargable = _currentWeapon is LaunchableWeapon;
                _launchForce = 0;
                direction = _projectileSpawnPoint.rotation * Vector3.forward;
                //GetComponent<PlayerController>().enabled = false;
                //_currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,direction);
            }
            if (!_hasShot) return;

            if (!_weaponIsChargable)
            {
                _currentWeapon.Fire(_projectileSpawnPoint.position,direction);
                this.enabled = false;
            }

 
            if (_inputHandler.ShootInput)
            {
                _launchForce +=  chargeSpeed *Time.deltaTime;
                if (_launchForce > _maxLaunchForce)
                    _launchForce = _maxLaunchForce;
                
                print(_launchForce);
            }
            else
            {
                _currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,direction);
                this.enabled = false;

            }

        }
        public void AssignNewWeapon(Weapon newWeapon)
        {
            _currentWeapon = newWeapon;
        }

    }
    
    
    
}
