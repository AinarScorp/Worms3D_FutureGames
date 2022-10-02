using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;
using WormsGame.Core;
using WormsGame.Movement;

namespace WormsGame.Units
{
    public class Unit : MonoBehaviour
    {
        public static event Action<Unit> UnitSpawned;
        public static event Action<Unit> UnitRemoved;
        
        [Header("Debuggin")]
        [SerializeField] bool setStartingHealth;

        [SerializeField] float _deathDelay = 2.0f;
        [SerializeField] Transform _handTransform;
        [SerializeField] TeamAlliance _alliance;
        [SerializeField] Animator _animator;
        [SerializeField] Color _teamColor = Color.black;
        bool _unitIsAcive;
        int _currentHealth = 0;
        
        InputHandler _inputHandler;
        ImpactKnockback _impactKnockback;
        
        public event Action<Unit> Dying;
        public event Action<int, int> HealthModifed; //pass start health and modifiedHealth

        #region Properties
        public TeamAlliance Alliance => _alliance;
        public Transform HandTransform => _handTransform;
        public InputHandler InputHandler => _inputHandler;
        public bool UnitIsAcive => _unitIsAcive;

        public int CurrentHealth => _currentHealth;

        public Color TeamColor => _teamColor;

        #endregion

        void Awake()
        {
            _impactKnockback = GetComponent<ImpactKnockback>();
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
            ModifyHealth(GetStartingHealth());
        }

        int GetStartingHealth()
        {
            int startingHealth = 100;
            if (setStartingHealth) return startingHealth;
            
            switch (_alliance)
            {
                case TeamAlliance.Bats:
                    startingHealth = TeamsStartingHealth.BatsStartHealth;
                    break;
                case TeamAlliance.Slimes:
                    startingHealth = TeamsStartingHealth.SlimesStartHealth;
                    break;                
                case TeamAlliance.Rabbits:
                    startingHealth = TeamsStartingHealth.RabbitsStartHealth;
                    break;                
                case TeamAlliance.Ghosts:
                    startingHealth = TeamsStartingHealth.GhostsStartHealth;
                    break;
            }
            return startingHealth;
        }
        public void ModifyHealth(int amount)
        {
            int startingHealth = _currentHealth;
            _currentHealth += amount;
            if (_currentHealth<=0)
            {
                _currentHealth = 0;
                Dying?.Invoke(this);
                _animator.SetTrigger("HasDied");
                StartCoroutine(DestroyMe());

            }
            else if (_currentHealth<startingHealth)
            {
                _animator.SetTrigger("IsDamaged");
            }
            HealthModifed?.Invoke(startingHealth, _currentHealth);
        }
        
        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(_deathDelay);
            this.gameObject.SetActive(false);
        }

        public void Push(Vector3 direction, float force)
        {
            _impactKnockback.AddImpact(direction,force);
        }


        public void ToggleUnit(bool activate) => _inputHandler.enabled = activate;
    }

    public class TeamsStartingHealth
    {
        public static int SlimesStartHealth = -1;
        public static int BatsStartHealth = -1;
        public static int RabbitsStartHealth =-1;
        public static int GhostsStartHealth = -1;
    }
    
}
