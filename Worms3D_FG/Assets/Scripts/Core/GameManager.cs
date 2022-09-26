using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.SceneManagement;

namespace WormsGame.Core
{
    public class GameManager : MonoBehaviour
    {
        TurnHandler _turnHandler;
        void Awake()
        {
            _turnHandler = FindObjectOfType<TurnHandler>();
            _turnHandler.TeamRemoved += GameOver;
        }

        void OnDestroy()
        {
            _turnHandler.TeamRemoved -= GameOver;
        }

        void GameOver()
        {
            if (_turnHandler.AllTeams.Count <=1)
            {
                Debug.Log("Game Over");
                _turnHandler.TeamRemoved -= GameOver;
                GameObject.FindWithTag("SceneManager").TryGetComponent(out SceneHandler sceneHandler);
                sceneHandler?.LoadNextScene();
                
            }
            
        }
    }
    
}
