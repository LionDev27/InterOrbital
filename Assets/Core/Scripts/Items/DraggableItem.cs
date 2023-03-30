using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public int inventoryIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
