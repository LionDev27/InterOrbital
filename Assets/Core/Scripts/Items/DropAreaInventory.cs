using InterOrbital.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            GameObject dropped = eventData.pointerDrag;
           
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            PlayerComponents.Instance.Inventory.DropItem(PlayerComponents.Instance.PlayerAttack.attackPoint.position, PlayerComponents.Instance.transform.position, draggableItem.inventoryIndex, null);
        }
    }
}
