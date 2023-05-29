using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Player;
using InterOrbital.Item;
using TMPro;

public class ChestInventory : Inventory
{
   
    private void Start()
    {
        InitSlots();
        StartInventory();
    }
   
    public void SetChest(ItemObject[] chestAux)
    {
        for (int i = 0; i < chestAux.Length; i++)
        {
            //Debug.Log("iTEM NAME " + chestAux[i].itemSo.id);
            _items[i] = chestAux[i];
            _itemsSlot[i].sprite = chestAux[i].itemSo.itemSprite;
            _textAmount[i].text = chestAux[i].amount == 0 ? "" :chestAux[i].amount.ToString();
        }
    }

    public ItemObject[] GetItems()
    {
        ItemObject[] aux = new ItemObject[_items.Length];
        for (int i = 0; i < aux.Length; i++)
            aux[i] = _items[i];

        return aux;
    }




}
