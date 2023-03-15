using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        DraggableItem switchItem = GetComponentInChildren<DraggableItem>() ;

      
        Transform aux = draggableItem.parentAfterDrag;
        draggableItem.parentAfterDrag = transform;
        switchItem.transform.SetParent(aux);
        
       
    }
}
