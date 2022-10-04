using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.SceneManagement;
using WormsGame.Units;

namespace WormsGame.Core
{
    public class GameManager : MonoBehaviour
    {
        TeamsHandler _teamsHandler;
        void Awake()
        {
            _teamsHandler = FindObjectOfType<TeamsHandler>();
            _teamsHandler.TeamRemoved += GameOver;
        }
        
        void OnDestroy()
        {
            _teamsHandler.TeamRemoved -= GameOver;
        }

        void GameOver()
        {
            if (_teamsHandler.AllTeams.Count <=1)
            {
                Debug.Log("Game Over");
                _teamsHandler.TeamRemoved -= GameOver;
                GameObject.FindWithTag("SceneManager").TryGetComponent(out SceneHandler sceneHandler);
                sceneHandler?.LoadNextScene();
                
            }
            
        }
    }
    
}
