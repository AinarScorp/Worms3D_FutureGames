using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WormsGame.Combat
{
    [CreateAssetMenu(fileName = "Launchable Weapon", menuName = "Weapons/Make New Launchable", order = 0)]
    public class LaunchableWeapon : Weapon
    {
        [SerializeField] int _maxDamage = 10;
        [SerializeField] int _minDamage = 1;

        [SerializeField] Projectile projectilePrefab;

        public int MaxDamage => _maxDamage;
        public int MinDamage => _minDamage;



        public override void Fire(Vector3 spawnPos, float launchForce, Vector3 direction)
        {
            base.Fire(spawnPos, launchForce,direction);
            if (projectilePrefab == null) return;
            
            Projectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectile.SetupProjectile(direction, this, launchForce);
        }
    }
    
}
