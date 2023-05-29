using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InterOrbital.Player;
using InterOrbital.Utils;
using System;
using InterOrbital.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Transform _rootParent;
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public int inventoryIndex;

    private void Awake()
    {  
        _rootParent = GetRootParent();
    }


    private void Start()
    { 
       inventoryIndex = Utils.ObtainNumName(gameObject.transform.parent.gameObject);
    }

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
            Debug.Log(inventoryIndex);
            parentAfterDrag = transform.parent;
            transform.SetParent(_rootParent);
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
