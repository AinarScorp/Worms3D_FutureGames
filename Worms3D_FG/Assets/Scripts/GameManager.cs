using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        void GameOver()
        {
            if (_turnHandler.AllTeams.Count <=1)
            {
                Debug.Log("Game Over");
                _turnHandler.TeamRemoved -= GameOver;

            }
            
        }
    }
    
}
