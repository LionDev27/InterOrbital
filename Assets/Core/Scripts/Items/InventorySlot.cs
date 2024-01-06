using InterOrbital.Player;
using InterOrbital.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            GameObject dropped = eventData.pointerDrag;
            PlayerComponents.Instance.Inventory.ChangeSlots(dropped, gameObject, false);
        }   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DraggableItem dg = GetComponentInChildren<DraggableItem>();
        PlayerComponents.Instance.Inventory.ClickSwapInventory(dg.inventoryIndex, gameObject.CompareTag("Chest"));
    }
}
