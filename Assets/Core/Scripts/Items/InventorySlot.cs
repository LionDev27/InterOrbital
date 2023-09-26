using InterOrbital.Player;
using InterOrbital.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (!PlayerComponents.Instance.Inventory.isHide)
        {
            
            GameObject dropped = eventData.pointerDrag;
            
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                
            DraggableItem switchItem = GetComponentInChildren<DraggableItem>();

            if(dropped.GetComponent<Image>().sprite != PlayerComponents.Instance.Inventory.itemVoid.itemSprite)
            {
                //Debug.Log(PlayerComponents.Instance.Inventory.GetTypeItemByIndex(draggableItem.inventoryIndex).ToString() + "----------" + switchItem.transform.parent.tag);
                //Debug.Log(draggableItem.parentAfterDrag.tag + "----------" + PlayerComponents.Instance.Inventory.GetTypeItemByIndex(switchItem.inventoryIndex).ToString());

                bool cantSwitch = false;

                if (switchItem != null)
                {
                    if (PlayerComponents.Instance.Inventory.GetItemObjectByIndex(draggableItem.inventoryIndex).itemSo.type.ToString() != "Bullet" && switchItem.transform.parent.CompareTag("BulletSlot"))
                    {
                        cantSwitch = true;
                    }
                    else if (draggableItem.parentAfterDrag.CompareTag("BulletSlot") && PlayerComponents.Instance.Inventory.GetItemObjectByIndex(switchItem.inventoryIndex).itemSo.type.ToString() != "Bullet")
                    {
                        cantSwitch = true;
                    }
                }




                if (transform.childCount != 0 && !cantSwitch)
                {
                    Transform aux = draggableItem.parentAfterDrag;
                    int auxIndex = draggableItem.inventoryIndex;
                    draggableItem.parentAfterDrag = transform;
                    draggableItem.inventoryIndex = switchItem.inventoryIndex;
                    switchItem.transform.SetParent(aux);
                    switchItem.inventoryIndex = auxIndex;
                    if (dropped.tag != gameObject.tag)
                    {
                        string auxTag = switchItem.tag;

                        switchItem.tag = dropped.tag;
                        dropped.tag = auxTag;
                        if (dropped.CompareTag("Chest"))
                        {
                            PlayerComponents.Instance.Inventory.SwitchItemWithChest(switchItem.inventoryIndex, draggableItem.inventoryIndex);
                        }
                        else if (!gameObject.CompareTag("BulletSlot"))
                        {
                            PlayerComponents.Instance.Inventory.SwitchItemWithChest(draggableItem.inventoryIndex, switchItem.inventoryIndex);
                        }
                        else
                        {
                            PlayerComponents.Instance.Inventory.SwitchItems(switchItem.inventoryIndex, draggableItem.inventoryIndex);
                            BulletSelector.Instance.UpdateBulletSelectorUI();
                        }
                    }
                    else
                    {
                        PlayerComponents.Instance.Inventory.SwitchItems(switchItem.inventoryIndex, draggableItem.inventoryIndex);
                        BulletSelector.Instance.UpdateBulletSelectorUI();
                    }


                }
            }

            
        }
      
        
    }
}
