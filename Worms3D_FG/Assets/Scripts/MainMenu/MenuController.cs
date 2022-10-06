using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WormsGame.SceneManagement;
using WormsGame.UI;



namespace WormsGame.MainMenu
{
    public class MenuController : MonoBehaviour
    {
        //taking from startGamebtn.cs
        [Header("Hooking up")]
        [SerializeField] UnitOptions[] _unitOptions;
        [SerializeField] SceneHandler _sceneHandler;
        
        [Header("Don't touch during this project")]
        [SerializeField] int _maxPlayerCount = 4;
        [SerializeField] int _minPlayerCount = 2;
        
        int _activePlayers;
        
        List<UnitOptions> _activeUnitOptions = new List<UnitOptions>();
        TransitionController _transitionController;

        #region Properties

        public List<UnitOptions> ActiveUnitOptions => _activeUnitOptions;

        #endregion

        void Awake()
        {
            _transitionController = FindObjectOfType<TransitionController>();
        }

        void Start()
        {
            OpenTheGame();
            SetupUnitOptions();
        }

        void OpenTheGame()
        {
            _transitionController.TriggerSceneOpening();
        }
        void SetupUnitOptions()
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
            
            if (!_activeUnitOptions.Contains(_unitOptions[_activePlayers-1]))
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


        public void StartTheGame()
        {
            _transitionController.TriggerSceneCloser();
            _transitionController.SceneHasClosed += _sceneHandler.LoadNextScene;
        }
        public void SwitchSections(SwitchSectionBtn switchBtn)
        {
            switchBtn.SwitchSections();
        }
    }
    
}
