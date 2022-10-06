using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Core;
using WormsGame.Units;

namespace WormsGame.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] UnitHealthBar unitHealthBar;
        [SerializeField] TeamHealthBar _teamHealthPrefab;
        [SerializeField] Transform _unitsHealthContainer;
        [SerializeField] Transform _teamHealthContainer;
        [SerializeField] float _shwoingTeamsHealthTime = 2.0f;


        List<TeamHealthBar> _createdTeamHealthBars = new List<TeamHealthBar>();
        int _largestTeamHealth;


        void Awake()
        {
            Unit.UnitSpawned += AddHealthBar;
            TurnHandler turnHandler = FindObjectOfType<TurnHandler>();
            turnHandler.TurnFinished += DisplayTeamsHealthWhenTurnEnded;
            TeamsHandler teamsHandler = FindObjectOfType<TeamsHandler>();
            teamsHandler.TeamCreated += AdjustLargestHealth;
            teamsHandler.AllTeamsCreated += CreateTeamBars;
        }

        void Start()
        {
            ToggleTeamHealth(false);
        }

        void OnDestroy()
        {
            Unit.UnitSpawned -= AddHealthBar;
            
        }


        void AddHealthBar(Unit unit)
        {
            UnitHealthBar newUnitHealthBar = Instantiate(unitHealthBar, _unitsHealthContainer);
            newUnitHealthBar.SetupHealthBar(unit);
        }

        void AdjustLargestHealth(TeamInfo team) // change name
        {
            TeamHealthBar teamHealthbar = Instantiate(_teamHealthPrefab, _teamHealthContainer);
            _createdTeamHealthBars.Add(teamHealthbar);
            if (_largestTeamHealth < team.TeamsFullHealth)
            {
                _largestTeamHealth = team.TeamsFullHealth;
            }
            
            teamHealthbar.SetupHealthBar(team);
        }

        void CreateTeamBars()
        {
            foreach (var teamHealthBar in _createdTeamHealthBars)
            {
                teamHealthBar.AssignLargestHealth(_largestTeamHealth);
            }

        }
        void ToggleTeamHealth(bool activate)
        {
            _teamHealthContainer.gameObject.SetActive(activate);
        }

        void DisplayTeamsHealthWhenTurnEnded()
        {
            StartCoroutine(DisplayingTeamsHealth());
        }


        IEnumerator DisplayingTeamsHealth()
        {
            ToggleTeamHealth(true);
            yield return new WaitForSeconds(_shwoingTeamsHealthTime);
            ToggleTeamHealth(false);
        }
    }
}