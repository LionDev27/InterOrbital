using InterOrbital.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            
            GameObject dropped = eventData.pointerDrag;
            
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

            if (transform.childCount != 0)
            {
                
                DraggableItem switchItem = GetComponentInChildren<DraggableItem>();
                Transform aux = draggableItem.parentAfterDrag;
                int auxIndex = draggableItem.inventoryIndex;
                draggableItem.parentAfterDrag = transform;
                draggableItem.inventoryIndex = switchItem.inventoryIndex;
                switchItem.transform.SetParent(aux);
                switchItem.inventoryIndex = auxIndex;
                if(dropped.tag != gameObject.tag)
                {
                    string auxTag = switchItem.tag;
                    switchItem.tag = dropped.tag;
                    dropped.tag = auxTag;
                    if (dropped.CompareTag("Chest"))
                    {
                        PlayerComponents.Instance.Inventory.SwitchItemWithChest(switchItem.inventoryIndex, draggableItem.inventoryIndex);
                    }
                    else
                    {
                        PlayerComponents.Instance.Inventory.SwitchItemWithChest(draggableItem.inventoryIndex, switchItem.inventoryIndex);
                    }
                }
                else
                {
                     PlayerComponents.Instance.Inventory.SwitchItems(switchItem.inventoryIndex, draggableItem.inventoryIndex);
                }
            }
        }
      
        
    }
}
