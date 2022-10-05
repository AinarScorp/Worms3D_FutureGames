using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WormsGame.Combat;
using WormsGame.Core;

namespace WormsGame.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] List<Weapon> _availableWeapons = new List<Weapon>();
        [SerializeField] Transform _inventoryUI;
        TurnHandler _turnHandler;

        void Awake()
        {
            _turnHandler = FindObjectOfType<TurnHandler>();
        }

        public void ChooseNewWeapon(Weapon newWeapon)
        {
            if (_turnHandler.HasFired)
                return;
            
            //newWeapon.SpawnWeapon(_turnHandler.CurrentUnit.HandTransform);
            _turnHandler.CurrentUnit.CombatController.AssignNewWeapon(newWeapon); //think about this
            ToggleInventoryUI();
        }

        public void OpenInventory(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (_turnHandler.HasFired)
                    return;

                ToggleInventoryUI();
            }
        }

        void ToggleInventoryUI()
        {
            bool inventoryIsOpen = _inventoryUI.gameObject.activeInHierarchy;
            _turnHandler.CurrentUnit.ToggleActivation(inventoryIsOpen);
            _inventoryUI.gameObject.SetActive(!inventoryIsOpen);
            Cursor.visible = !inventoryIsOpen;
        }
    }
    
}
