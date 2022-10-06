using System;
using System.Collections;
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
        
        int _currentHealth = 0;
        
        //cached
        InputHandler _inputHandler;
        ImpactKnockback _impactKnockback;
        CombatController _combatController;
        PlayerController _playerController;
        
        public event Action<Unit> Dying;
        public event Action<int, int> HealthModifed; //pass start health and modifiedHealth

        #region Properties
        public int CurrentHealth => _currentHealth;
        public TeamAlliance Alliance => _alliance;
        public Color TeamColor => _teamColor;
        public InputHandler InputHandler => _inputHandler;

        public PlayerController PlayerController => _playerController;

        #endregion

        void Awake()
        {
            _combatController = GetComponent<CombatController>();
            _impactKnockback = GetComponent<ImpactKnockback>();
            _inputHandler = GetComponent<InputHandler>();
            _playerController = GetComponent<PlayerController>();

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
                Die();
            }
            else if (_currentHealth<startingHealth)
            {
                _animator.SetTrigger("IsDamaged");
            }
            HealthModifed?.Invoke(startingHealth, _currentHealth);
        }

        void Die()
        {
            ToggleActivation(false);
            Dying?.Invoke(this);
            _animator.SetTrigger("HasDied");
            StartCoroutine(DestroyMe());
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
