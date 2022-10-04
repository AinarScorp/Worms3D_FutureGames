using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Combat;
using WormsGame.Inputs;
using WormsGame.Movement;

namespace WormsGame.Units
{
    public class Unit : MonoBehaviour
    {
        public static event Action<Unit> UnitSpawned;
        public static event Action<Unit> UnitRemoved;
        
        [Header("Debuggin")]
        [SerializeField] bool setStartingHealth;
        
        [Header("Wiring ")]
        [SerializeField] Animator _animator;
        
        [Header("Unit setup")]
        [SerializeField] Color _teamColor = Color.black;
        [SerializeField] TeamAlliance _alliance;
        [SerializeField] float _deathDelay = 2.0f;
        
        bool _unitIsAcive;
        int _currentHealth = 0;
        
        //cached
        InputHandler _inputHandler;
        ImpactKnockback _impactKnockback;
        CombatController _combatController;
        
        public event Action<Unit> Dying;
        public event Action<int, int> HealthModifed; //pass start health and modifiedHealth

        #region Properties
        public bool UnitIsAcive => _unitIsAcive;
        public int CurrentHealth => _currentHealth;
        public TeamAlliance Alliance => _alliance;
        public Color TeamColor => _teamColor;
        public InputHandler InputHandler => _inputHandler;
        public CombatController CombatController => _combatController;

        #endregion

        void Awake()
        {
            _combatController = GetComponent<CombatController>();
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

        public int GetStartingHealth()
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


        public void ToggleActivation(bool turnOn)
        {
            _inputHandler.enabled = turnOn;
            if (!turnOn)
                _combatController.CurrentWeapon?.DestroyOldWeapon();
            
        }
    }

    public class TeamsStartingHealth
    {
        public static int SlimesStartHealth = -1;
        public static int BatsStartHealth = -1;
        public static int RabbitsStartHealth =-1;
        public static int GhostsStartHealth = -1;
    }
    
}
