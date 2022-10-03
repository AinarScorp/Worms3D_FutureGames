using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WormsGame.Core;
using WormsGame.Units;

namespace WormsGame.UI
{
    public class TeamHealthBar : MonoBehaviour
    {
        [SerializeField] Image displayHealthImage;
        TeamInfo _thisTeam;
        HealthBarController _healthBarController;

        public void SetupHealthBar(HealthBarController healthBarController,TeamInfo thisTeam)
        {
            _thisTeam = thisTeam;
            _healthBarController = healthBarController;
            displayHealthImage.color = _thisTeam.AvailableUnits[0].TeamColor;
            foreach (var unit in _thisTeam.AvailableUnits)
            {
                unit.HealthModifed+= UpdateTeamHealth;
            }
        }

        public void UpdateTeamHealth(int unitStartHealth, int unitModifiedHealth)
        {
            displayHealthImage.fillAmount = (float)_thisTeam.GetTeamCurrentHealth() / (float)_healthBarController.LargestTeamHealth;
        }
    }
    
}
