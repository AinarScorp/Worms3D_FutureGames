using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Core;

namespace WormsGame.Units
{
    public class TeamsHandler : MonoBehaviour
    {
        public event Action<TeamInfo> TeamCreated;
        public event Action<TeamInfo> TeamRemoved;
        
        public event Action AllTeamsCreated; // deactivated for now

        Action OneTeamRemaining;
        //Action AllTeamsCreated;
        List<TeamInfo> _allTeams = new List<TeamInfo>();

        #region Properties

        public List<TeamInfo> AllTeams => _allTeams;

        #endregion


        public void FindAllUnits()
        {
            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach (var unit in allUnits)
            {
                if (unit == null)
                {
                    Debug.LogError("Check this part");
                    return;
                }

                AddToTeamsList(unit);
                unit.ToggleActivation(false);
                unit.Dying += RemoveUponDeath;
            }

            foreach (var team in _allTeams)
            {
                team.StoreTeamHealthCombined();
                TeamCreated?.Invoke(team);
            }
            AllTeamsCreated?.Invoke();
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

        void RemoveUponDeath(Unit unit)
        {
            FindUnitsTeam(unit, out TeamInfo belongingTeam, out var teamExists);

            if (!teamExists)
                Debug.LogError("sth is wrong");

            belongingTeam.RemoveUnit(unit);

            RemoveTeam(belongingTeam);
        }

        void RemoveTeam(TeamInfo belongingTeam)
        {
            if (belongingTeam.AvailableUnits.Count > 0) return;

            _allTeams.Remove(belongingTeam);

            TeamRemoved?.Invoke(belongingTeam);
            if (_allTeams.Count >1) return;

            OneTeamRemaining();
        }

        public void InitOnlyOneReamRemaining(Action callback)
        {
            OneTeamRemaining = callback;
        }
    }
}