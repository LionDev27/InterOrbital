using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InterOrbital.Player;
using InterOrbital.Utils;
using System;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public int inventoryIndex;
    
    private Transform GetRootParent()
    {
        Transform root=transform.parent;
        for(int i=0; i<10; i++)
        {
            if(root.GetComponent<Canvas>() != null)
            {
                return root;
            }
            else
            {
                root = root.parent;
            }
        }

        return root;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(GetRootParent());
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            transform.position = Input.mousePosition;
            image.ChangueAlphaColor(0.7f);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            image.ChangueAlphaColor(1f);
        }
    }
}
