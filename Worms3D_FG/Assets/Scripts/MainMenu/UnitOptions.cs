using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WormsGame.Core;

public class UnitOptions : MonoBehaviour
{
    
    [Header("Alliance Setup")] 
    [SerializeField] Transform _modelPlacement;
    [SerializeField] PlayerOptionModel[] _models;
    [SerializeField] TextMeshProUGUI _displayAllianceText;

    [Header("Unit Setup")] 
    [SerializeField] int _maxUnitCount;
    [SerializeField] TextMeshProUGUI _displayUnitCountText;

    [Header("Health Setup")]
    [SerializeField] int _healthIncerements = 50;
    [SerializeField] int _maxHealth = 500;
    [SerializeField] TextMeshProUGUI _displayHealthText;
    //Alliance Setup
    int _possibleTeamsAmount;
    int _selectedAllianceIndex;
    PlayerOptionModel _currentModel;
    TeamAlliance _selectedAlliance;
    //Unit Setup
    int _selectedHealth;
    //Health Setup
    int _selectedUnitCount;

    public TeamAlliance SelectedAlliance => _selectedAlliance;

    public int SelectedHealth => _selectedHealth;

    public int SelectedUnitCount => _selectedUnitCount;

    void Start()
    {
        _possibleTeamsAmount = Enum.GetNames(typeof(TeamAlliance)).Length;
        ModifyUnitCount(0);
        IncreaseHealth();
        ScrollAlliances(true);
        
    }

    public void ScrollAlliances(bool scrollRight)
    {
        _selectedAllianceIndex = scrollRight ? _selectedAllianceIndex+1 : _selectedAllianceIndex-1;
        
        if (_selectedAllianceIndex > _possibleTeamsAmount)
            _selectedAllianceIndex = 1;
        else if (_selectedAllianceIndex<1)
            _selectedAllianceIndex = _possibleTeamsAmount;

        _selectedAlliance = (TeamAlliance)_selectedAllianceIndex;

        if(_currentModel!=null) Destroy(_currentModel.gameObject);
        
        _currentModel = Instantiate(_models[_selectedAllianceIndex - 1], _modelPlacement);
        UpdateDisplay(_displayAllianceText, _selectedAlliance.ToString());
    }

    public void ModifyUnitCount(int amount)
    {
        _selectedUnitCount += amount;
        
        if (_selectedUnitCount < 1)
            _selectedUnitCount = 1;
        else if (_selectedUnitCount>_maxUnitCount)
            _selectedUnitCount = _maxUnitCount;
        UpdateDisplay(_displayUnitCountText,_selectedUnitCount.ToString());
    }
    public void IncreaseHealth()
    {
        _selectedHealth += _healthIncerements;
        if (_selectedHealth >_maxHealth)
            _selectedHealth = _maxHealth;
        UpdateDisplay(_displayHealthText,_selectedHealth.ToString());
    }
    public void DecreaseHealth()
    {
        _selectedHealth -= _healthIncerements;
        if (_selectedHealth<_healthIncerements)
            _selectedHealth = _healthIncerements;
        UpdateDisplay(_displayHealthText,_selectedHealth.ToString());
    }

    void UpdateDisplay(TextMeshProUGUI textDisplay,string text)
    {
        textDisplay.text = text;
    }

    
}
