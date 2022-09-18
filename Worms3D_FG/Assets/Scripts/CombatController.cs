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
        [SerializeField] Transform _handTransform;
        InputHandler _inputHandler;
        bool _hasShot;
        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
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
            if (!_hasShot &&_inputHandler.ShootInput )
            {
                _hasShot = true;
                _currentWeapon.Fire(transform.position,transform.forward);
            }
        }
    }
    
    
}
