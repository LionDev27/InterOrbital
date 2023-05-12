using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour, IInteractable
    {
        public ItemCraftScriptableObject currentCraftSelected;
        public GameObject craftUI;
        public CraftGrid craftGrid;
        
        public void Interact()
        {
            Debug.Log("Interacting");
            UIManager.Instance.ActivateOrDesactivateUI(craftUI);
            if (craftUI.activeSelf)
            {
                craftGrid.currentCraftingItem = this;
                craftGrid.UpdateFeedback();
                craftGrid.SelectLast();
            }
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(craftUI);
            if (craftUI.activeSelf)
            {
                craftGrid.UpdateFeedback();
                craftGrid.SelectLast();
                craftGrid.currentCraftingItem = null;
            }
        }
    }
}
