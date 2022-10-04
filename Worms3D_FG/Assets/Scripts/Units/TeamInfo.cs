using System;
using System.Collections.Generic;

namespace WormsGame.Units
{
    [Serializable]
    public class TeamInfo
    {
        int _teamsFullHealth;
        TeamAlliance _teamAlliance;
        List<Unit> _availableUnits = new List<Unit>();

        #region Properties

        public int TeamsFullHealth => _teamsFullHealth;
        public TeamAlliance TeamAlliance => _teamAlliance;
        public List<Unit> AvailableUnits => _availableUnits;

        #endregion

        public TeamInfo(TeamAlliance teamAlliance)
        {
            _teamAlliance = teamAlliance;
        }

        
        public void AddUnit(Unit unit) => _availableUnits.Add(unit);

        public void RemoveUnit(Unit unit) => _availableUnits.Remove(unit);

        public int GetTeamCurrentHealth()
        {
            int currentHealth = 0;
            foreach (var unit in _availableUnits)
            {
                currentHealth += unit.CurrentHealth;
            }

            return currentHealth;
        }
        public void StoreTeamHealthCombined()
        {
            int fullHealth = 0;
            foreach (var unit in _availableUnits)
            {
                fullHealth += unit.GetStartingHealth();
            }

            _teamsFullHealth = fullHealth;
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
