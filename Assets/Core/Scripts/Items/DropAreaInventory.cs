using InterOrbital.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
       
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        PlayerComponents.Instance.Inventory.DropItem(draggableItem.inventoryIndex, null);
    }
}