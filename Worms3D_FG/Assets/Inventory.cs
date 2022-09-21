using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Combat;
using WormsGame.Core;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Weapon> _availableWeapons = new List<Weapon>();
    TurnHandler _turnHandler;
    private void Awake()
    {
        _turnHandler = FindObjectOfType<TurnHandler>(_turnHandler);
    }
    public void ChooseNewWeapon(Weapon newWeapon)
    {
        newWeapon.SpawnWeapon(_turnHandler.CurrentUnit.HandTransform);
        _turnHandler.CurrentUnit.GetComponent<CombatController>().AssignNewWeapon(newWeapon);
    }
}
