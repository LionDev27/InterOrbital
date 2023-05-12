using System;
using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour, IInteractable
    {
        [HideInInspector]
        public ItemCraftScriptableObject currentCraftSelected;
        private GameObject _craftUI;
        private CraftGrid _craftGrid;

        private void Start()
        {
            _craftUI = UIManager.Instance.craftUI;
            _craftGrid = _craftUI.GetComponentInChildren<CraftGrid>();
        }

        public void Interact()
        {
            Debug.Log("Interacting");
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
            if (_craftUI.activeSelf)
            {
                _craftGrid.currentCraftingItem = this;
                _craftGrid.UpdateFeedback();
                _craftGrid.SelectLast();
            }
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
            if (_craftUI.activeSelf)
            {
                _craftGrid.UpdateFeedback();
                _craftGrid.SelectLast();
                _craftGrid.currentCraftingItem = null;
            }
        }
    }
}
