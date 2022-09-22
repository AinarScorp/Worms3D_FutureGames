using UnityEngine;
using WormsGame.Core;

namespace WormsGame.Combat
{
    [CreateAssetMenu(fileName = "Direct Weapon", menuName = "Weapons/Make New Direct", order = 0)]

    public class DirectWeapon : Weapon
    {
        [SerializeField] int _damage = 10;
        [SerializeField] LayerMask _targetLayerMask;

        public override void Fire(Vector3 shootStartPos,Vector3 direction)
        {
            base.Fire(shootStartPos, direction);
            RaycastHit hit;
            if (Physics.Raycast(shootStartPos, direction, out hit, _targetLayerMask))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit)
                {
                    Debug.Log("hit unit");
                    unit.ModifyHealth(-_damage);
                }

            }
        }
    }

}
