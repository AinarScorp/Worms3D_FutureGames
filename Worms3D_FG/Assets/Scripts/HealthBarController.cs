using System;
using UnityEngine;
using WormsGame.Core;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] HealthBar _healthBar;
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
