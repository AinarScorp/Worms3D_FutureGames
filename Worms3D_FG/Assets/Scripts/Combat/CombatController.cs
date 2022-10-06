using UnityEngine;
using UnityEngine.UI;
using WormsGame.Core;
using WormsGame.Inputs;

namespace WormsGame.Combat
{
    [RequireComponent(typeof(InputHandler))]
    public class CombatController : MonoBehaviour
    {
        [SerializeField] float chargeSpeed = 5;
        [SerializeField] float _maxLaunchForce = 20.0f;
        [SerializeField] float _minLaunchForce = 5.0f;
        
        [SerializeField] Transform _projectileSpawnPoint; // use frist person camera
        [SerializeField] Transform _handTransform;
        
        [SerializeField] Weapon _currentWeapon;

        float _launchForce;
        bool _hasShot;
        bool _weaponIsChargable;
        
        Image _chargeBar;
        
        InputHandler _inputHandler;
        TurnHandler _turnHandler;
        public Weapon CurrentWeapon => _currentWeapon;

        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _turnHandler = FindObjectOfType<TurnHandler>();

            _inputHandler.SubscribeToActivation(()=>this.enabled = true, true);
            _inputHandler.SubscribeToActivation(()=>
            {
                _currentWeapon?.DestroyOldWeapon();
                this.enabled = false;
            }, false);
            _chargeBar = GameObject.FindWithTag("ChargeBar").GetComponent<Image>();
        }
        
        void OnEnable()
        {
            _hasShot = false;
            _chargeBar.fillAmount = 0;
        }
        //clean here, make named functions, then make it contact turnhandler and finish its turn instead of turnhandler contacting
        //weapon doing it
        void Update()
        {
            if (_currentWeapon ==null) return;

            if (!_hasShot && _inputHandler.ShootInput)
            {
                _hasShot = true;
                _weaponIsChargable = _currentWeapon is LaunchableWeapon;
                _launchForce = _minLaunchForce;

            }
            if (!_hasShot) return;

            if (!_weaponIsChargable)
            {
                FireAndEndTurn(_weaponIsChargable);
                return;
            }
            if (_inputHandler.ShootInput && _launchForce < _maxLaunchForce)
            {
                FillChargeBar();
            }
            else
            {
                FireAndEndTurn(_weaponIsChargable);
            }

        }

        void FireAndEndTurn(bool weaponIsChargable)
        {
            if (!weaponIsChargable)
            { 
                _currentWeapon.Fire(_projectileSpawnPoint.position, _projectileSpawnPoint.forward);
            }
            else
            {
                _currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,_projectileSpawnPoint.forward);
            }
            DestroyCurrentWeapon();
            _turnHandler.FinishTurn();
            this.enabled = false;
        }
        
        void FillChargeBar()
        {
            _launchForce +=  chargeSpeed *Time.deltaTime;
            if (_launchForce > _maxLaunchForce)
                _launchForce = _maxLaunchForce;
            
            _chargeBar.fillAmount = _launchForce / _maxLaunchForce;
        }

        public void AssignNewWeapon(Weapon newWeapon)
        {
            this.enabled = true;
            newWeapon.SpawnWeapon(_handTransform, this.gameObject);
            _currentWeapon = newWeapon;
        }

        public void DestroyCurrentWeapon()
        {
            this.enabled = false;
            _currentWeapon.DestroyOldWeapon();
            _currentWeapon = null;

        }
        
        

    }
    
    
    
}
