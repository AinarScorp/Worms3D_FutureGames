using System;
using UnityEngine;
using UnityEngine.UI;
using WormsGame.UI;
using WormsGame.Units;

namespace WormsGame.Core
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] HealthBar _healthBar;
        [SerializeField] Image _teamHealthPrefab;
        void Awake()
        {
            Unit.UnitSpawned += AddHealthBar;
        }

        void OnDestroy()
        {
            Unit.UnitSpawned -= AddHealthBar;
        }


        void AddHealthBar(Unit unit)
        {
            HealthBar newHealthBar = Instantiate(_healthBar, transform);
            newHealthBar.SetupHealthBar(unit);
        }

    }
    
}
