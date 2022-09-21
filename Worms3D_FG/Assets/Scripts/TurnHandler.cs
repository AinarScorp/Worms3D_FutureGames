using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WormsGame.Cameras;
using WormsGame.Movement;

//TO DO What happens if a certain team does not exist and you try to activate 0th plater? look into activateTeamMember function
namespace WormsGame.Core
{
    public class TurnHandler : MonoBehaviour
    {
        [SerializeField] TeamAlliance _startingTeam;
        List<Unit> _teamSlimes = new List<Unit>();
        List<Unit> _teamRabbits = new List<Unit>();
        List<Unit> _teamBats = new List<Unit>();
        List<Unit> _teamGhosts = new List<Unit>();
        CameraManager _cameraManager;
        TeamAlliance _teamsTurn; // whose turn it is, think of a better name
        Unit _currentUnit;
        int _currentUnitIndex;

        public Unit CurrentUnit => _currentUnit;

        void Awake()
        {
            _cameraManager = FindObjectOfType<CameraManager>();
        }

        void Start()
        {
            _teamsTurn = _startingTeam;
            FindAllUnits();
            //activate first team member
            ActivateTeamMember(0);
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

        void RemoveUponDeath(Unit unit)
        {
            TeamAlliance alliance = unit.Alliance;
            switch (alliance)
            {
                case TeamAlliance.Slimes:
                    _teamSlimes.Remove(unit);
                    break;
                case TeamAlliance.Rabbits:
                    _teamRabbits.Remove(unit);
                    break;
                case TeamAlliance.Bats:
                    _teamBats.Remove(unit);
                    break;
                case TeamAlliance.Ghosts:
                    _teamGhosts.Remove(unit);
                    break;
                default: 
                    Debug.Log("Allinace is not assigned");
                    break;
            }
        }
        void AddToTeamsList(Unit unit)
        {
            TeamAlliance alliance = unit.Alliance;
            switch (alliance)
            {
                case TeamAlliance.Slimes:
                    _teamSlimes.Add(unit);
                    break;
                case TeamAlliance.Rabbits:
                    _teamRabbits.Add(unit);
                    break;
                case TeamAlliance.Bats:
                    _teamBats.Add(unit);
                    break;
                case TeamAlliance.Ghosts:
                    _teamGhosts.Add(unit);
                    break;
                default: 
                    Debug.Log("Allinace is not assigned");
                    break;
            }
        }

        void DeactivateUnit(Unit unit) => unit.ToggleUnit(false);

        List<Unit> GetTeamListFromTeamsTurn(TeamAlliance turnOwner)
        {
            switch (turnOwner)
            {
                case TeamAlliance.Slimes:
                    return _teamSlimes;
                case TeamAlliance.Rabbits:
                    return _teamRabbits;
                case TeamAlliance.Bats:
                    return _teamBats;
                case TeamAlliance.Ghosts:
                    return _teamGhosts;
            }
            Debug.Log("should never happen, check this");
            return null;
        }

        

        void ActivateTeamMember(int unitToActivate)
        {
            if (_currentUnit != null)
                DeactivateUnit(_currentUnit);
            
            Unit unit = GetTeamListFromTeamsTurn(_teamsTurn)[unitToActivate];
            unit.ToggleUnit(true);
            _currentUnit = unit;
            _currentUnitIndex = GetTeamListFromTeamsTurn(_teamsTurn).IndexOf(unit);
            PlayerController controller = unit.GetComponent<PlayerController>();
            _cameraManager.FocusOnCurrentPlayer(controller);
        }

        int GetUnitIndex(int index)
        {
            int countInList = GetTeamListFromTeamsTurn(_teamsTurn).Count;
            if (index <0)
                return countInList - 1;

            if (index >=countInList )
                return 0;

            return index;

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