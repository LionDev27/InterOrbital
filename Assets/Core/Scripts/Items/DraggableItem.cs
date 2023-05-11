using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InterOrbital.Player;


    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public int inventoryIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            transform.position = Input.mousePosition;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }
}
