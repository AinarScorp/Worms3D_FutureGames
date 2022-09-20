using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Combat;

namespace WormsGame.Combat
{
    [CreateAssetMenu(fileName = "Launchable Weapon", menuName = "Weapons/Make New Launchable", order = 0)]
    public class LaunchableWeapon : Weapon
    {
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] int _maxDamage = 10;
        [SerializeField] int _minDamage = 1;

        [SerializeField] int _obstuctionPenalty = 3;

        public int MaxDamage => _maxDamage;
        public int MinDamage => _minDamage;

        public int ObstuctionPenalty => _obstuctionPenalty;


        public override void Fire(Vector3 spawnPos, float launchForce, Vector3 direction)
        {
            base.Fire(spawnPos, launchForce,direction);
            if (projectilePrefab == null) return;
            
            Projectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectile.SetupProjectile(direction, this, launchForce);
        }
    }
    
}
