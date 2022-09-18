using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Teams;

public class Unit : MonoBehaviour
{
    TeamAlliance _alliance;

    [SerializeField] int _startingHealth = 10;
    int _currentHealth;

    

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth<=0)
        {
            Defeat();
        }
    }

    void Defeat()
    {
        Destroy(this.gameObject);
    }
}
