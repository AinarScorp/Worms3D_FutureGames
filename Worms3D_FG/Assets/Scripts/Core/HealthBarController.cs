using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WormsGame.UI;
using WormsGame.Units;

namespace WormsGame.Core
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] UnitHealthBar unitHealthBar;
        [SerializeField] TeamHealthBar _teamHealthPrefab;
        [SerializeField] Transform _unitsHealthContainer;
        [SerializeField] Transform _teamHealthContainer;
        [SerializeField] float _shwoingTeamsHealthTime = 2.0f;

        int _largestTeamHealth;

        public int LargestTeamHealth => _largestTeamHealth;

        void Awake()
        {
            Unit.UnitSpawned += AddHealthBar;
            TurnHandler turnHandler = FindObjectOfType<TurnHandler>();
            turnHandler.TurnFinished += DisplayTeamsHealthWhenTurnEnded;
            TeamsHandler teamsHandler = FindObjectOfType<TeamsHandler>();
            teamsHandler.TeamCreated += AddTeamHealthBar;
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

        void AddTeamHealthBar(TeamInfo team)
        {
            TeamHealthBar teamHealthbar = Instantiate(_teamHealthPrefab, _teamHealthContainer);
            if (_largestTeamHealth < team.TeamsFullHealth)
            {
                _largestTeamHealth = team.TeamsFullHealth;
            }

            teamHealthbar.SetupHealthBar(this, team);
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