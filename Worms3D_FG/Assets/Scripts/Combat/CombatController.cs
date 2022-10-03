using UnityEngine;
using UnityEngine.UI;
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

        public Weapon CurrentWeapon => _currentWeapon;

        void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();

            _inputHandler.SubscribeToActivation(()=>this.enabled = true, true);
            _inputHandler.SubscribeToActivation(()=>this.enabled = false, false);
            _chargeBar = GameObject.FindWithTag("ChargeBar").GetComponent<Image>();
        }
        
        void OnEnable()
        {
            _hasShot = false;
            _chargeBar.fillAmount = 0;
        }
        
        void Update()
        {
            if (_currentWeapon ==null) return;

            if (!_hasShot && _inputHandler.ShootInput)
            {
                _hasShot = true;
                _weaponIsChargable = _currentWeapon is LaunchableWeapon;
                
                _launchForce = _minLaunchForce;
                //GetComponent<PlayerController>().enabled = false;
                //_currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,direction);
            }
            if (!_hasShot) return;

            if (!_weaponIsChargable)
            {
                Vector3 direction = _projectileSpawnPoint.rotation * Vector3.forward;

                _currentWeapon.Fire(_projectileSpawnPoint.position,_projectileSpawnPoint.forward);
                DestroyCurrentWeapon();
                this.enabled = false;
                return;
            }

 
            if (_inputHandler.ShootInput && _launchForce < _maxLaunchForce)
            {
                _launchForce +=  chargeSpeed *Time.deltaTime;
                if (_launchForce > _maxLaunchForce)
                    _launchForce = _maxLaunchForce;
                FillChargeBar();
            }
            else
            {                

                Vector3 direction = _projectileSpawnPoint.rotation * Vector3.forward;
                _currentWeapon.Fire(_projectileSpawnPoint.position,  _launchForce,_projectileSpawnPoint.forward);
                DestroyCurrentWeapon();
                this.enabled = false;

            }

        }

        void FillChargeBar()
        {
            _chargeBar.fillAmount = _launchForce / _maxLaunchForce;
        }

        public void AssignNewWeapon(Weapon newWeapon)
        {
            this.enabled = true;
            newWeapon.SpawnWeapon(_handTransform);
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
