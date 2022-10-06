using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.SceneManagement;
using WormsGame.UI;
using WormsGame.Units;

//this is more UI manager
namespace WormsGame.UI
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] float _gameOverWaitTime = 5.0f;
        TeamsHandler _teamsHandler;
        TransitionController _transitionController;
        
        void Awake()
        {
            _transitionController = FindObjectOfType<TransitionController>();
            _teamsHandler = FindObjectOfType<TeamsHandler>();
            GameObject.FindWithTag("SceneManager").TryGetComponent(out SceneHandler sceneHandler); // I use this just a try it out
            
            SubscribeToEvents(sceneHandler);
        }

        void SubscribeToEvents(SceneHandler sceneHandler) //this is weird
        {
            if (_teamsHandler != null)
                _teamsHandler.TeamRemoved += GameOver;
            if (_transitionController != null)
                _teamsHandler.AllTeamsCreated += _transitionController.TriggerSceneOpening;
            if (sceneHandler != null)
                _transitionController.SceneHasClosed += sceneHandler.LoadNextScene;
        }

        void OnDestroy()
        {
            _teamsHandler.TeamRemoved -= GameOver;
            _teamsHandler.AllTeamsCreated -= _transitionController.TriggerSceneOpening;

        }

        void GameOver()
        {
            if (_teamsHandler.AllTeams.Count >1) return;
            
            StartGameOverSequence();
        }

        void StartGameOverSequence()
        {
            StartCoroutine(GameOverSequence());
        }
        IEnumerator GameOverSequence()
        {
            yield return new WaitForSeconds(_gameOverWaitTime);
            _transitionController.TriggerSceneCloser();
        }
    }
    
}
