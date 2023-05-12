using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Item;
using InterOrbital.Player;

public class CraftGrid : MonoBehaviour
{
    private List<CraftSlot> _craftSlots;
    public GameObject gridPrefab;
    public List<ItemCraftScriptableObject> itemsCraft;
    public CraftingItem currentCraftingItem;

    private void Awake()
    {
        _craftSlots = new List<CraftSlot>();
    }

    private void Start()
    {
        for(int i=0; i<itemsCraft.Count; i++)
        {
            var newCraft= Instantiate(gridPrefab);
            newCraft.transform.SetParent(gameObject.transform, false);
            CraftSlot craftSlot =  newCraft.GetComponent<CraftSlot>();
            craftSlot.SetItemCraft(itemsCraft[i]);
            _craftSlots.Add(craftSlot);
        }        
    }

    public void UpdateFeedback()
    {
        for(int i=0; i<_craftSlots.Count; i++)
        {
            _craftSlots[i].SetCurrentCraftingItem(currentCraftingItem);
            _craftSlots[i].CheckCanCraft();
        }
    }

    public void SelectLast()
    {
        int index = itemsCraft.IndexOf(currentCraftingItem.currentCraftSelected);

        if(index == -1)
        {
            index = 0;
        }
        
        _craftSlots[index].SelectCraft();
    }
   

}
