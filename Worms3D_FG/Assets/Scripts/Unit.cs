using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;

namespace WormsGame.Core
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] TeamAlliance _alliance;
        [SerializeField] Transform _handTransform;

        [SerializeField] int _startingHealth = 10;
        bool _unitIsAcive;
        public event Action<Unit> Dying;
        InputHandler _inputHandler;
        int _currentHealth;

        public TeamAlliance Alliance => _alliance;

        public Transform HandTransform => _handTransform;
        public InputHandler InputHandler => _inputHandler;

        public bool UnitIsAcive => _unitIsAcive;

        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _inputHandler.SubscribeToActivation(() => _unitIsAcive = true,true);
            _inputHandler.SubscribeToActivation(() => _unitIsAcive = false,false);

        }

        void Start()
        {
            _currentHealth = _startingHealth;
        }

        public void ModifyHealth(int amount)
        {
            _currentHealth += amount;
            if (_currentHealth<=0)
            {
                if (Dying != null)
                {
                    Dying(this);
                }
                
                DestroyMe();
            }
        }

        void DestroyMe()
        {
            this.gameObject.SetActive(false);
        }



        public void ToggleUnit(bool activate)
        {
            _inputHandler.enabled = activate;
            
        }
        
    }
    
}
