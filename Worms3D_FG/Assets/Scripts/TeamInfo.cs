using System;
using System.Collections.Generic;

namespace WormsGame.Core
{
    [Serializable]
    public class TeamInfo
    {
        TeamAlliance _teamAlliance;
        List<Unit> _availableUnits = new List<Unit>();
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
    }
    public enum TeamAlliance
    {
        Slimes,
        Rabbits,
        Bats,
        Ghosts
    }
    
}
