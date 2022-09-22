using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Core;
using WormsGame.Inputs;

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
        
        [SerializeField] float _minLaunchForce = 5.0f;

        float _launchForce;
        InputHandler _inputHandler;
        bool _hasShot;
        bool _weaponIsChargable;
        Vector3 direction;

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

        void Update()
        {
            if (_currentWeapon ==null) return;

            if (!_hasShot && _inputHandler.ShootInput)
            {
                _hasShot = true;
                _weaponIsChargable = _currentWeapon is LaunchableWeapon;
                
                _launchForce = _minLaunchForce;
                direction = _projectileSpawnPoint.rotation * Vector3.forward;
                //GetComponent<PlayerController>().enabled = false;
                //_currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,direction);
            }
            if (!_hasShot) return;

            if (!_weaponIsChargable)
            {
                StartCoroutine(FindObjectOfType<TurnHandler>().FinishTurn());

                _currentWeapon.Fire(_projectileSpawnPoint.position,direction);
                _currentWeapon = null;
                this.enabled = false;
                return;
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
                StartCoroutine(FindObjectOfType<TurnHandler>().FinishTurn());
                _currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,direction);
                _currentWeapon = null;

                this.enabled = false;

            }

        }
        public void AssignNewWeapon(Weapon newWeapon)
        {
            this.enabled = true;
            _currentWeapon = newWeapon;
        }

    }
    
    
    
}
