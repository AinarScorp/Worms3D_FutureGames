using System;
using System.Collections.Generic;
using WormsGame.UI;

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

        public int TeamsFullHealth => _teamsFullHealth;

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

        public int GetTeamCurrentHealth()
        {
            int currentHealth = 0;
            foreach (var unit in _availableUnits)
            {
                currentHealth += unit.CurrentHealth;
            }

            return currentHealth;
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
