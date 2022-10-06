using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.SceneManagement;
using WormsGame.Units;

//this is more UI manager
namespace WormsGame.UI
{
    public class GameManager : MonoBehaviour
    {
        [Header("Tutorial")] 
        [SerializeField][TextArea] string _firstMessage;
        [SerializeField][TextArea] string _secondMessage;
        [SerializeField] float _firstMessageTimer, _secondMessageTimer;
        [SerializeField] float _timeBetweenMessages;

        [Header("Game over")]
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
                _teamsHandler.InitOnlyOneReamRemaining(GameOver);
            if (_transitionController != null)
                _teamsHandler.AllTeamsCreated += _transitionController.TriggerSceneOpening;
            if (sceneHandler != null)
            {
                _transitionController.SceneHasOpened += StartTutorial;
                _transitionController.SceneHasClosed += sceneHandler.LoadNextScene;
            }
        }

        void StartTutorial()
        {
            StartCoroutine(Tutorial());
        }

        IEnumerator Tutorial()
        {
            PopUpMessage.Instance.DisplayPopUpMessage(_firstMessage,_firstMessageTimer);
            yield return new WaitForSeconds(_firstMessageTimer +  _timeBetweenMessages);
            PopUpMessage.Instance.DisplayPopUpMessage(_secondMessage,_secondMessageTimer);

        }

        void OnDestroy()
        {
            _teamsHandler.AllTeamsCreated -= _transitionController.TriggerSceneOpening;
        }

        void GameOver()
        {
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
