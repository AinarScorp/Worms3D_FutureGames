using System;
using System.Collections.Generic;

namespace WormsGame.Units
{
    [Serializable]
    public class TeamInfo
    {
        TeamAlliance _teamAlliance;
        List<Unit> _availableUnits = new List<Unit>();
        int _teamsFullHealth;
        int _teamsCurrentHealth;
        public static event Action<Unit> UnitAdded;
        public TeamAlliance TeamAlliance => _teamAlliance;

        public List<Unit> AvailableUnits => _availableUnits;

        public TeamInfo(TeamAlliance teamAlliance)
        {
            _teamAlliance = teamAlliance;
        }


        public void AddUnit(Unit unit)
        {
            _availableUnits.Add(unit);
            
        }

        public void RemoveUnit(Unit unit)
        {
            _availableUnits.Remove(unit);

        }

        public void StoreMaxTeamHealth()
        {
            int fullHealth = 0;
            foreach (var unit in _availableUnits)
            {
                fullHealth += unit.GetStartingHealth();
            }

            _teamsFullHealth = fullHealth;
            _teamsCurrentHealth = fullHealth;
        }
    }
    public enum TeamAlliance
    {
        Slimes = 1,
        Rabbits= 2,
        Bats= 3,
        Ghosts= 4
    }
    
}
