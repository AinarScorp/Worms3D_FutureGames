using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

using WormsGame.Combat;
using WormsGame.Units;

//TO DO What happens if a certain team does not exist and you try to activate 0th plater? look into activateTeamMember function
namespace WormsGame.Core
{
    public class TurnHandler : MonoBehaviour
    {
        bool _hasFired;
        
        int _currentUnitIndex;
        Unit _currentUnit;
        TeamInfo _currentTeamTurn;
        //cached
        TeamsHandler _teamsHandler;
        //Events
        public event Action TurnFinished;
        public event Action TurnStarted;
        public event Action<Unit> UnitChanged;

        #region Properties

        
        public Unit CurrentUnit => _currentUnit;
        public bool HasFired => _hasFired;
        public List<TeamInfo> AllTeams => _teamsHandler.AllTeams;

        #endregion

        void Awake()
        {
            //Weapon.HasFired += FinishTurn;
            _teamsHandler = FindObjectOfType<TeamsHandler>();
        }

        // void OnDestroy()
        // {
        //     Weapon.HasFired -= FinishTurn;
        // }

        public void ActivateRandomTeam()
        {
            //activate first team member
            if (AllTeams.Count <= 0)
            {
                Debug.LogError("No units on the map");
                return;
            }
            
            int rnd = Random.Range(0, AllTeams.Count);

            _currentTeamTurn = AllTeams[rnd];
            TurnStarted?.Invoke();
            ActivateTeamMember(0);
        }
        void ActivateTeamMember(int unitToActivate)
        {
            _hasFired = false;
            if (_currentUnit != null)
                _currentUnit.ToggleActivation(false);
            
            if (GetUnitList(_currentTeamTurn).Count <=0)
                return;
            
            _currentUnit = GetUnitList(_currentTeamTurn)[unitToActivate];
            
            _currentUnitIndex = GetUnitList(_currentTeamTurn).IndexOf(_currentUnit);
            
            _currentUnit.ToggleActivation(true);
            UnitChanged?.Invoke(_currentUnit);
            
        }
        List<Unit> GetUnitList(TeamInfo teamInfo) => teamInfo.AvailableUnits;


        int GetUnitIndex(int index)
        {
            int countInList = GetUnitList(_currentTeamTurn).Count;
            
            if (index <0) return countInList - 1;
            if (index >=countInList ) return 0;
            
            return index;
        }

        public void TimeIsUp()
        {
            if (_currentUnit != null)
                _currentUnit.ToggleActivation(false);
            FinishTurn();
        }
        public void FinishTurn()
        {
            TurnFinished?.Invoke();
            StartCoroutine(FinishingTurn());
        }

        
        
        public IEnumerator FinishingTurn()
        {
            _hasFired = true;
            yield return new WaitForSeconds(5);
            
            SelectNextTeam();
            int index = GetUnitIndex(_currentUnitIndex + 1);

            ActivateTeamMember(index);
            TurnStarted?.Invoke();

        }

        void SelectNextTeam()
        {
            int currentTeamIndex = -1;
            foreach (var team in AllTeams)
            {
                if (team == _currentTeamTurn)
                {
                    currentTeamIndex = AllTeams.IndexOf(team);
                    break;
                }
            }

            if (currentTeamIndex < 0)
                Debug.LogWarning("Please report");


            currentTeamIndex++;
            if (currentTeamIndex >= AllTeams.Count)
                currentTeamIndex = 0;

            _currentTeamTurn = AllTeams[currentTeamIndex];
        }

        #region InputSystem

        //Unit selections

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

        #region CommentedOut
        //public event Action TeamRemoved;
        //public event Action<TeamInfo> TeamCreated;
        
        //List<TeamInfo> _allTeams = new List<TeamInfo>();

        
        // public void FindAllUnits()
        // {
        //     Unit[] allUnits = FindObjectsOfType<Unit>();
        //     foreach (var unit in allUnits)
        //     {
        //         if (unit == null)
        //         {
        //             Debug.Log("Check this part");
        //             return;
        //         }
        //         AddToTeamsList(unit);
        //         DeactivateUnit(unit);
        //         unit.Dying += RemoveUponDeath;
        //     }
        //
        //     foreach (var team in _allTeams)
        //     {
        //         team.StoreMaxTeamHealth();
        //         TeamCreated?.Invoke(team);
        //     }
        //     ActivateRandomTeam();
        //
        // }
        

        // void AddToTeamsList(Unit unit)
        // {
        //     FindUnitsTeam(unit, out var belongingTeam, out var teamExists);
        //
        //     if (!teamExists)
        //     {
        //         TeamInfo newTeam = new TeamInfo(unit.Alliance);
        //         _allTeams.Add(newTeam);
        //         belongingTeam = newTeam;
        //     }
        //     belongingTeam.AddUnit(unit);
        //     
        // }
        
        // void RemoveUponDeath(Unit unit)
        // {
        //     FindUnitsTeam(unit, out var belongingTeam, out var teamExists);
        //
        //     if (!teamExists)
        //         Debug.LogError("sth is wrong");
        //     
        //     belongingTeam.RemoveUnit(unit);
        //     
        //     RemoveTeam(belongingTeam);
        //     
        // }
        //
        // void RemoveTeam(TeamInfo belongingTeam)
        // {
        //     if (belongingTeam.AvailableUnits.Count <= 0)
        //         _allTeams.Remove(belongingTeam);
        //     
        //     TeamRemoved?.Invoke();
        // }

        // void FindUnitsTeam(Unit unit, out TeamInfo belongingTeam, out bool teamExists)
        // {
        //     TeamAlliance alliance = unit.Alliance;
        //     belongingTeam = null;
        //     teamExists = false;
        //     foreach (TeamInfo team in _allTeams)
        //     {
        //         if (team.TeamAlliance == alliance)
        //         {
        //             belongingTeam = team;
        //             teamExists = true;
        //             break;
        //         }
        //     }
        // }

        // void DeactivateUnit(Unit unit)
        // {
        //     unit.ToggleActivation(false);
        //     unit.CombatController.CurrentWeapon?.DestroyOldWeapon();
        // }
        #endregion

  
        
    }
    
}