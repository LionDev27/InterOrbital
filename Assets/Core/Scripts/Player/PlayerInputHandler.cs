using System;
using UnityEngine;
using UnityEngine.InputSystem;
using InterOrbital.UI;
using UnityEngine.EventSystems;

namespace InterOrbital.Player
{
    public class PlayerInputHandler : PlayerComponents
    {
        private bool BulletsMenuSelected = false;
        private bool _uiOpened;
        public enum InputType {Keyboard, Gamepad}
        
        public Vector2 MoveDirection { get; private set; }
        public Vector2 AimPosition { get; private set; }
        public Vector2 AimDirection { get; private set; }

        public int InventoryPositionValue { get; private set; }
        public int BulletsPositionValue { get; private set; }

        //Para los botones, ejecutaremos un Action que asignaremos en otro script.
        public Action OnAttack;
        public Action OnUseItems;

        public Action OnOpenCraft;

        public Action OnDashPerformed;
        public Action OnInteractPerformed;


        private void Start()
        {
            InventoryPositionValue = 1;
            BulletsPositionValue = 0;
            BulletsMenuSelected = false;
        }
        //Haremos un metodo nuevo que se llame igual que el nuevo input introducido en los Input Settings.
        //A su vez, haremos una propiedad para obtener el valor del input en otro script.
        private void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        private void OnAimPosition(InputValue value)
        {
            AimPosition = value.Get<Vector2>();
        }

        private void OnAimDirection(InputValue value)
        {
            AimDirection = value.Get<Vector2>();
        }

        private void OnScrollY(InputValue value)
        {
            if (value.Get<float>() < 0)
            {
                if (BulletsMenuSelected)
                {
                    BulletsPositionValue += 1;
                    BulletsPositionValue = BulletsPositionValue % 4;
                    BulletSelector.Instance.UpdateSelectedBullet(BulletsPositionValue);
                }
                else
                {
                    InventoryPositionValue += 1;
                    if (InventoryPositionValue > 5)
                        InventoryPositionValue = 5;
                }
            }
            else if(value.Get<float>() > 0)
            {
                if (BulletsMenuSelected)
                {
                    BulletsPositionValue -= 1;
                    BulletsPositionValue = (BulletsPositionValue + 4) % 4;
                    BulletSelector.Instance.UpdateSelectedBullet(BulletsPositionValue);
                }
                else
                {
                    InventoryPositionValue -= 1;
                    if (InventoryPositionValue < 1)
                        InventoryPositionValue = 1;
                }
            }
        }

        private void OnSelectNumeric(InputValue value)
        {
            if (value.isPressed)
            {
                if (BulletsMenuSelected)
                {
                    int newValue = (int)value.Get<float>();
                    if (newValue <= 4 && newValue >= 0)
                    {
                        BulletsPositionValue = newValue - 1;
                        BulletSelector.Instance.UpdateSelectedBullet(BulletsPositionValue);
                    }
                }
                else
                {
                    InventoryPositionValue = (int)value.Get<float>();
                }
            }
        }

        private void OnBulletsMenu(InputValue value)
        {
            if(value.isPressed)
            {
                BulletsMenuSelected = !BulletsMenuSelected;
                if (BulletsMenuSelected)
                    BulletSelector.Instance.SelectAnimation();
                else
                    BulletSelector.Instance.DeselectAnimation();
            }
        }

        private void OnFire()
        {
            OnAttack();

            if(BuildGrid.Instance.IsBuilding())
            {
                BuildGrid.Instance.Build(BuildGrid.Instance.GetPosToBuild());
            }
        }
        
        private void OnInventory()
        {
            BulletsMenuSelected = false;
            if (!UIManager.Instance.isChestOpen)
            {
                UIManager.Instance.OpenInventory(false);
            }
        }

        private void OnMinimap()
        {
            UIManager.Instance.ToggleMinimap();
        }

        private void OnUseItem()
        {
            PlayerComponents.Instance.Inventory.UseItem();
        }

        private void OnDash()
        {
            OnDashPerformed();
        }

        private void OnInteract()
        {
            BulletsMenuSelected = false;
            OnInteractPerformed();
        }

        private void OnExitMenu()
        {
            var ui = UIManager.Instance;
            if ((ui.SomethingOpen && Inventory.isHide) || (!Inventory.isHide && ui.isChestOpen))
            {
                PlayerInteraction.Interact();
                return;
            }
            if (!Inventory.isHide)
                ui.OpenInventory(false);
        }

        public void DeactivateControls()
        {
            PlayerInput.enabled = false;
            enabled = false;
            Rigidbody.velocity = Vector2.zero;
        }

        public void ActivateControls()
        {
            enabled = true;
            PlayerInput.enabled = true;
        }

        public void OnConversationStart()
        {
            if (PlayerInput.currentActionMap.name == "UI")
            {
                _uiOpened = true;
                ChangeActionMap();
            }
            DeactivateControls();
        }

        public void OnConversationEnd()
        {
            ActivateControls();
            if (!_uiOpened) return;
            ChangeActionMap();
            _uiOpened = false;
        }
        
        public InputType CurrentInput()
        {
            return PlayerInput.currentControlScheme == "Gamepad" ? InputType.Gamepad : InputType.Keyboard;
        }

        public string CurrentActionMap()
        {
            return PlayerInput.currentActionMap.name;
        }

        public void ChangeActionMap()
        {
            string actionMap = PlayerInput.currentActionMap.name == "Player" ? "UI" : "Player";
            PlayerInput.SwitchCurrentActionMap(actionMap);
            CursorController.Instance.SetAlpha(actionMap == "Player");
        }

        private void OnCraft()
        {
            OnOpenCraft();
        }
    }
    
}
