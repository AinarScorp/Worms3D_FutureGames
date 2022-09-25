using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Core;

public class StartGameBtn : MonoBehaviour
{
    [SerializeField] SceneHandler _sceneHandler;
    [SerializeField] UnitOptions[] _unitOptions;
    
    [Header("Shake options")]
    [SerializeField] float _shakeTime = 2.0f;
    [SerializeField] float _shakeSpeed = 5.0f;
    [SerializeField] float _shakeAmount = 2.0f;
    [SerializeField] float _returnSpeed = 2.0f;
    
    [Header("Don't touch during this project")]
    [SerializeField] int _maxPlayerCount = 4;
    [SerializeField] int _minPlayerCount = 2;

    Coroutine _shakingCoroutine;
    List<UnitOptions> _activeUnitOptions = new List<UnitOptions>();
    int _activePlayers;
    void Start()
    {
        _activePlayers = _minPlayerCount;
        foreach (var unitOption in _unitOptions)
        {
            unitOption.gameObject.SetActive(false);
        }
        for (int i = 0; i < _minPlayerCount; i++)
        {
            _unitOptions[i].gameObject.SetActive(true);
            _activeUnitOptions.Add(_unitOptions[i]);
        }
    }

    public void AddPlayer()
    {
        
        _activePlayers++;
        if (_activePlayers>_maxPlayerCount)
            _activePlayers = _maxPlayerCount;

        _activeUnitOptions.Add(_unitOptions[_activePlayers-1]);
        _unitOptions[_activePlayers-1].gameObject.SetActive(true);
    }

    public void RemovePlayer()
    {

        _activePlayers--;
        if (_activePlayers<_minPlayerCount)
            _activePlayers = _minPlayerCount;
        _activeUnitOptions.Remove(_unitOptions[_activePlayers]);
        _unitOptions[_activePlayers].gameObject.SetActive(false);
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
    public void StartGame()
    {
        bool canStart = true;
        List<TeamAlliance> alliances = new List<TeamAlliance>();
        foreach (var _activeUnitOption in _activeUnitOptions)
        {
            if (alliances.Contains(_activeUnitOption.SelectedAlliance))
            {
                canStart = false;
                if (_shakingCoroutine == null)
                    _shakingCoroutine = StartCoroutine(ShakeText(_activeUnitOption.gameObject));
                
                break;
            }
            alliances.Add(_activeUnitOption.SelectedAlliance);
        }

        if (!canStart) return;


        foreach (var _activeUnitOption in _activeUnitOptions)
        {
            switch (_activeUnitOption.SelectedAlliance)
            {
                case TeamAlliance.Bats:
                    TeamsStartingHealth.BatsStartHealth = _activeUnitOption.SelectedHealth;
                    UnitSpawner.BatUnitCount = _activeUnitOption.SelectedUnitCount;
                    break;
                case TeamAlliance.Slimes:
                    TeamsStartingHealth.SlimesStartHealth = _activeUnitOption.SelectedHealth;
                    UnitSpawner.SlimeUnitCount = _activeUnitOption.SelectedUnitCount;
                    break;                
                case TeamAlliance.Rabbits:
                    TeamsStartingHealth.RabbitsStartHealth = _activeUnitOption.SelectedHealth;
                    UnitSpawner.RabbitUnitCount = _activeUnitOption.SelectedUnitCount;
                    break;                
                case TeamAlliance.Ghosts:
                    TeamsStartingHealth.GhostsStartHealth = _activeUnitOption.SelectedHealth;
                    UnitSpawner.GhostUnitCount = _activeUnitOption.SelectedUnitCount;

                    break;
            }
            
        }
        _sceneHandler.LoadNextScene();
    }
}
