using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Units;

namespace WormsGame.MainMenu
{
    public class StartGameBtn : MonoBehaviour
    {
        MenuController _menuController;
        void Awake()
        {
            _menuController = FindObjectOfType<MenuController>();
        }
        public void StartGame()
        {
            bool canStart = true;
            List<TeamAlliance> alliances = new List<TeamAlliance>();
            List<UnitOptions> _activeUnitOptions = _menuController.ActiveUnitOptions;
            foreach (var _activeUnitOption in _activeUnitOptions)
            {
                if (alliances.Contains(_activeUnitOption.SelectedAlliance))
                {
                    canStart = false;
                    _activeUnitOption.TriggerShaking();
                    break;
                }
                alliances.Add(_activeUnitOption.SelectedAlliance);
            }

            if (!canStart) return;


            PassInSettings(_activeUnitOptions);
            
            _menuController.StartTheGame();
        }

        void PassInSettings(List<UnitOptions> _activeUnitOptions)
        {
            foreach (var _activeUnitOption in _activeUnitOptions)
            {
                switch (_activeUnitOption.SelectedAlliance)
                {
                    case TeamAlliance.Bats:
                        TeamsStartingHealth.BatsStartHealth = _activeUnitOption.SelectedHealth;
                        UnitSpawner.BatUnitCount = _activeUnitOption.SelectedUnitCount;
                        break;
                    case TeamAlliance.Slimes:
                        TeamsStartingHealth.SlimesStartHealth = _activeUnitOption.SelectedHealth;
                        UnitSpawner.SlimeUnitCount = _activeUnitOption.SelectedUnitCount;
                        break;
                    case TeamAlliance.Rabbits:
                        TeamsStartingHealth.RabbitsStartHealth = _activeUnitOption.SelectedHealth;
                        UnitSpawner.RabbitUnitCount = _activeUnitOption.SelectedUnitCount;
                        break;
                    case TeamAlliance.Ghosts:
                        TeamsStartingHealth.GhostsStartHealth = _activeUnitOption.SelectedHealth;
                        UnitSpawner.GhostUnitCount = _activeUnitOption.SelectedUnitCount;
                        break;
                }
            }
        }
    }
}
