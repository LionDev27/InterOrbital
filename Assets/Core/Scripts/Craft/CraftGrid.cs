using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Item;

public class CraftGrid : MonoBehaviour
{
    public GameObject gridPrefab;
    public List<ItemCraftScriptableObject> itemCraft;


    private void Start()
    {
        for(int i=0; i<itemCraft.Count; i++)
        {
            var newCraft= Instantiate(gridPrefab);
            newCraft.transform.SetParent(gameObject.transform, false);
            newCraft.GetComponent<CraftSlot>().SetItemCraft(itemCraft[i]);
        }
    }

}
