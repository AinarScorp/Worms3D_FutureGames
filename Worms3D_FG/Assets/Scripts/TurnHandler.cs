using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using WormsGame.Cameras;
using WormsGame.Movement;
using Random = UnityEngine.Random;

//TO DO What happens if a certain team does not exist and you try to activate 0th plater? look into activateTeamMember function
namespace WormsGame.Core
{
    public class TurnHandler : MonoBehaviour
    {
        int _currentUnitIndex;
        bool _turnFinished;
        
        CameraManager _cameraManager;
        TeamInfo _currentTeamTurn;
        Unit _currentUnit;
        public event Action TeamRemoved;
        
        List<TeamInfo> _allTeams = new List<TeamInfo>();
        public Unit CurrentUnit => _currentUnit;
        public bool TurnFinished => _turnFinished;

        public List<TeamInfo> AllTeams => _allTeams;

        void Awake()
        {
            _cameraManager = FindObjectOfType<CameraManager>();
        }

        void Start()
        {
            FindAllUnits();
            ActivateRandomTeam();
        }
        
        void FindAllUnits()
        {
            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach (var unit in allUnits)
            {
                if (unit == null)
                {
                    Debug.Log("Check this part");
                    return;
                }
                AddToTeamsList(unit);
                DeactivateUnit(unit);
                unit.Dying += RemoveUponDeath;
            }
        }
        void ActivateRandomTeam()
        {
            //activate first team member
            if (_allTeams.Count <= 0)
            {
                Debug.LogError("No units on the map");
                return;
            }
            
            int rnd = Random.Range(0, _allTeams.Count);

            _currentTeamTurn = _allTeams[rnd];
            ActivateTeamMember(0);
        }
        void ActivateTeamMember(int unitToActivate)
        {
            if (_currentUnit != null)
                DeactivateUnit(_currentUnit);
            
            //Unit unit = GetTeamListFromTeamsTurn(_currentTeamTurn)[unitToActivate];
            if (GetUnitList(_currentTeamTurn).Count <=0)
                return;
            
            Unit unit = GetUnitList(_currentTeamTurn)[unitToActivate];

            _currentUnit = unit;
            //_currentUnitIndex = GetTeamListFromTeamsTurn(_currentTeamTurn).IndexOf(unit);
            _currentUnitIndex = GetUnitList(_currentTeamTurn).IndexOf(unit);
            unit.ToggleUnit(true);

            PlayerController controller = unit.GetComponent<PlayerController>();
            _cameraManager.FocusOnCurrentPlayer(controller);
        }

        void AddToTeamsList(Unit unit)
        {
            FindUnitsTeam(unit, out var belongingTeam, out var teamExists);

            if (!teamExists)
            {
                TeamInfo newTeam = new TeamInfo(unit.Alliance);
                _allTeams.Add(newTeam);
                belongingTeam = newTeam;
            }
            belongingTeam.AddUnit(unit);
            
        }
        
        void RemoveUponDeath(Unit unit)
        {
            FindUnitsTeam(unit, out var belongingTeam, out var teamExists);
        
            if (!teamExists)
                Debug.LogError("sth is wrong");
            
            belongingTeam.RemoveUnit(unit);
            
            RemoveTeam(belongingTeam);
            
        }

        void RemoveTeam(TeamInfo belongingTeam)
        {
            if (belongingTeam.AvailableUnits.Count <= 0)
                _allTeams.Remove(belongingTeam);
            
            TeamRemoved?.Invoke();
        }

        void FindUnitsTeam(Unit unit, out TeamInfo belongingTeam, out bool teamExists)
        {
            TeamAlliance alliance = unit.Alliance;
            belongingTeam = null;
            teamExists = false;
            foreach (TeamInfo team in _allTeams)
            {
                if (team.TeamAlliance == alliance)
                {
                    belongingTeam = team;
                    teamExists = true;
                    break;
                }
            }
        }

        void DeactivateUnit(Unit unit) => unit.ToggleUnit(false);
        List<Unit> GetUnitList(TeamInfo teamInfo) => teamInfo.AvailableUnits;


        int GetUnitIndex(int index)
        {
            int countInList = GetUnitList(_currentTeamTurn).Count;
            
            if (index <0) return countInList - 1;
            if (index >=countInList ) return 0;
            
            return index;
        }

        public IEnumerator FinishTurn()
        {
            _turnFinished = true;
            yield return new WaitForSeconds(5);
            
            int currentTeamIndex = -1;
            foreach (var team in _allTeams)
            {
                if (team == _currentTeamTurn)
                {
                    currentTeamIndex = _allTeams.IndexOf(team);
                    break;
                }
            }

            if (currentTeamIndex <0)
                Debug.LogWarning("Please report");
            

            currentTeamIndex++;
            if (currentTeamIndex >=_allTeams.Count)
                currentTeamIndex = 0;
            
            _currentTeamTurn = _allTeams[currentTeamIndex];
            ActivateTeamMember(0);
            _turnFinished = false;
        }
        
        
        #region InputSystem

        

        public void NextUnit(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                //what if he died???
                if (!_currentUnit.InputHandler.enabled)
                    return;

                int index = GetUnitIndex(_currentUnitIndex + 1);
                ActivateTeamMember(index);
            }
        }
        public void PrevUnit(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (!_currentUnit.InputHandler.enabled)
                    return;
                int index = GetUnitIndex(_currentUnitIndex - 1);
                ActivateTeamMember(index);


            }
        }
        #endregion

        
        
    }
    
}