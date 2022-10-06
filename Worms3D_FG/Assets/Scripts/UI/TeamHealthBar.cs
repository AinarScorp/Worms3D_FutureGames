using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WormsGame.Units;

namespace WormsGame.UI
{
    public class TeamHealthBar : MonoBehaviour
    {
        [SerializeField] Image displayHealthImage;
        int _largestTeamHealth;
        
        TeamInfo _thisTeam;
        
        public void SetupHealthBar(TeamInfo thisTeam)
        {
            _thisTeam = thisTeam;
            displayHealthImage.color = _thisTeam.AvailableUnits[0].TeamColor;
            foreach (var unit in _thisTeam.AvailableUnits)
            {
                unit.HealthModifed += UpdateTeamHealth;
            }

            FindObjectOfType<TeamsHandler>().TeamRemoved += RemoveThisBar;
        }

        public void AssignLargestHealth(int largestTeamHealth)
        {
            _largestTeamHealth = largestTeamHealth;
        }

        public void UpdateTeamHealth(int unitStartHealth, int unitModifiedHealth)
        {
            displayHealthImage.fillAmount =
                (float)_thisTeam.GetTeamCurrentHealth() / (float)_largestTeamHealth;
        }

        void RemoveThisBar()
        {
            Destroy(this.gameObject);
        }
    }
}