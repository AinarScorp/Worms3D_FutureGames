using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.SceneManagement;

namespace WormsGame.UI
{
    public class GameOverSceneManager : MonoBehaviour
    {
        TransitionController _transitionController;
        SceneHandler _sceneHandler;
        
        void Awake()
        {
            _sceneHandler = FindObjectOfType<SceneHandler>();
            _transitionController = FindObjectOfType<TransitionController>();
            _transitionController.SceneHasClosed += _sceneHandler.LoadFirstScene;
        }

        void Start()
        {
            Cursor.visible =true;
            _transitionController.TriggerSceneOpening();
        }

        public void GoToMainMenu()
        {
            _transitionController.TriggerSceneCloser();
            
        }
    }
    
}
