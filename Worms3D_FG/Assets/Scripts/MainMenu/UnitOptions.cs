using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WormsGame.Units;

namespace WormsGame.MainMenu
{
    public class UnitOptions : MonoBehaviour
    {
        [Header("Alliance Setup")] 
        [SerializeField] Transform _modelPlacement;
        [SerializeField] UnitOptionModel[] _models;
        [SerializeField] TextMeshProUGUI _displayAllianceText;

        [Header("Unit Setup")] 
        [SerializeField] int _maxUnitCount;
        [SerializeField] TextMeshProUGUI _displayUnitCountText;

        [Header("Health Setup")]
        [SerializeField] int _healthIncerements = 50;
        [SerializeField] int _maxHealth = 500;
        [SerializeField] TextMeshProUGUI _displayHealthText;
        
        [Header("Shake options")]
        [SerializeField] float _shakeTime = 0.5f;
        [SerializeField] float _shakeSpeed = 25.0f;
        [SerializeField] float _shakeAmount = 4.0f;
        [SerializeField] float _returnSpeed = 0.5f;

        //Alliance Setup
        int _possibleTeamsAmount;
        int _selectedAllianceIndex;
        UnitOptionModel _currentModel;
        TeamAlliance _selectedAlliance;
        //Unit Setup
        int _selectedHealth;
        //Health Setup
        int _selectedUnitCount;
        
        //Shaking Setup
        Coroutine _shakingCoroutine;


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

        public void TriggerShaking()
        {
            if (_shakingCoroutine != null) return;
            _shakingCoroutine = StartCoroutine(ShakeText(this.gameObject));
        }
        IEnumerator ShakeText(GameObject gameObjectToShake)
        {
            float timePassed = 0.0f;
            Vector3 originalPos = gameObjectToShake.transform.position;
            while (timePassed<_shakeTime)
            {
                timePassed += Time.deltaTime;
                Vector3 newPos =originalPos + new Vector3(Mathf.Sin(Time.time * _shakeSpeed) * _shakeAmount, 0, 0);
                gameObjectToShake.transform.position = newPos;
                yield return new WaitForEndOfFrame();
                
            }

            Vector3 startPos = gameObjectToShake.transform.position;
            Vector3 endPos = originalPos;
            float percent = 0.0f;
            while (percent<1.0f)
            {
                percent += Time.deltaTime * _returnSpeed;
                gameObjectToShake.transform.position = Vector3.Lerp(startPos, endPos, percent);
            }
            gameObjectToShake.transform.position = endPos;
            _shakingCoroutine = null;
        }
        
    }
}
