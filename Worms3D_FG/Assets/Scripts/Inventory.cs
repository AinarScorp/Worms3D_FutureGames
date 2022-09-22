using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Combat;
using WormsGame.Core;
using UnityEngine.InputSystem;

namespace WormsGame.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] List<Weapon> _availableWeapons = new List<Weapon>();
        [SerializeField] Transform _inventoryUI;
        TurnHandler _turnHandler;

        private void Awake()
        {
            _turnHandler = FindObjectOfType<TurnHandler>(_turnHandler);
        }

        public void ChooseNewWeapon(Weapon newWeapon)
        {
            if (_turnHandler.TurnFinished)
                return;
            
            newWeapon.SpawnWeapon(_turnHandler.CurrentUnit.HandTransform);
            _turnHandler.CurrentUnit.GetComponent<CombatController>().AssignNewWeapon(newWeapon);
            ToggleInventoryUI();
        }

        public void OpenInventory(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (_turnHandler.TurnFinished)
                    return;

                ToggleInventoryUI();
            }
        }

        void ToggleInventoryUI()
        {
            bool inventoryIsOpen = _inventoryUI.gameObject.activeInHierarchy;
            _turnHandler.CurrentUnit.ToggleUnit(inventoryIsOpen);
            _inventoryUI.gameObject.SetActive(!inventoryIsOpen);
            Cursor.visible = !inventoryIsOpen;
        }
    }
    
}
