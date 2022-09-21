using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Core;
using WormsGame.Inputs;

public class Unit : MonoBehaviour
{
    [SerializeField] TeamAlliance _alliance;
    [SerializeField] Transform _handTransform;

    [SerializeField] int _startingHealth = 10;

    public event Action<Unit> Dying;
    InputHandler _inputHandler;
    int _currentHealth;

    public TeamAlliance Alliance => _alliance;

    public Transform HandTransform => _handTransform;
    public InputHandler InputHandler => _inputHandler;

    void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    void Start()
    {
        _currentHealth = _startingHealth;
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth<=0)
        {
            if (Dying != null)
            {
                Dying(this);
            }
            
            DestroyMe();
        }
    }

    void DestroyMe()
    {
        this.gameObject.SetActive(false);
    }



    public void ToggleUnit(bool activate)
    {
        _inputHandler.enabled = activate;
    }
    
}
