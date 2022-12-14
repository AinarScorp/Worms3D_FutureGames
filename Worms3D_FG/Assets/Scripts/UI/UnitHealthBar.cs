using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WormsGame.Core;
using WormsGame.Units;

namespace WormsGame.UI
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] float positionOffset = 2.0f;
        [SerializeField] float _displayUpdateSpeed =2.0f;
        
        [SerializeField] TextMeshProUGUI _lifeLeftDisplay;
        
        int _currentDisplayedHealth = -999;
        
        //cached
        Camera _mainCamera;
        Unit _unit;
        void Awake()
        {
            _mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (_unit == null) return;

            FollowUnitWithOffset();
            LookAtCamera();
        }

        void LookAtCamera()
        {
            transform.LookAt(_mainCamera.transform, Vector3.up);
        }

        void FollowUnitWithOffset()
        {
            transform.position = _unit.transform.position + Vector3.up * positionOffset;
        }

        public void SetupHealthBar(Unit unit)
        {
            this._unit = unit;
            unit.HealthModifed += UpdateHealth;
        }

        void UpdateHealth(int startHealth,int modifiedHealth)
        {
            StartCoroutine(UpdatingHealth(startHealth,modifiedHealth));
        }

        IEnumerator UpdatingHealth(int startHealth, int modifiedHealth)
        {
            int startHealthValue = startHealth;
            int newHealthValue = modifiedHealth;
            float percent = 0;
            while (percent < 1)
            {
                _currentDisplayedHealth = (int)Mathf.Lerp(startHealthValue, newHealthValue, percent);
                DisplayHealth();
                percent += Time.deltaTime * _displayUpdateSpeed;
                yield return null;
            }

            _currentDisplayedHealth = newHealthValue;
            DisplayHealth();
            DestoyMe();
        }
        
        void DisplayHealth()
        {
            _lifeLeftDisplay.text = _currentDisplayedHealth.ToString();
            _lifeLeftDisplay.color = _unit.TeamColor;
        }

        void DestoyMe()
        {
            if (_currentDisplayedHealth <= 0)
                Destroy(this.gameObject);
        }
        
    }
}
