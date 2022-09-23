using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;

namespace WormsGame.Core
{
    public class Unit : MonoBehaviour
    {
        public static event Action<Unit> UnitSpawned;
        public static event Action<Unit> UnitRemoved;

        [SerializeField] TeamAlliance _alliance;
        [SerializeField] Transform _handTransform;
        [SerializeField] float _deathDelay = 2.0f;

        [SerializeField] int _startingHealth = 10;
        bool _unitIsAcive;
        public event Action<int, int> HealthModifed; //pass start health and modifiedHealth
        
        public event Action<Unit> Dying;
        InputHandler _inputHandler;
        int _currentHealth = 0;

        public TeamAlliance Alliance => _alliance;

        public Transform HandTransform => _handTransform;
        public InputHandler InputHandler => _inputHandler;

        public bool UnitIsAcive => _unitIsAcive;

        public int CurrentHealth => _currentHealth;

        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _inputHandler.SubscribeToActivation(() => _unitIsAcive = true,true);
            _inputHandler.SubscribeToActivation(() => _unitIsAcive = false,false);
            

        }

        void OnEnable()
        {
            UnitSpawned?.Invoke(this);
        }
        

        void Start()
        {
            ModifyHealth(_startingHealth);
        }

        public void ModifyHealth(int amount)
        {
            int startingHealth = _currentHealth;
            _currentHealth += amount;
            if (_currentHealth<=0)
            {
                _currentHealth = 0;
                Dying?.Invoke(this);
                StartCoroutine(DestroyMe());
            }
            HealthModifed?.Invoke(startingHealth, _currentHealth);
        }
        
        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(_deathDelay);
            this.gameObject.SetActive(false);
        }



        public void ToggleUnit(bool activate)
        {
            _inputHandler.enabled = activate;
            
        }
        
    }
    
}
