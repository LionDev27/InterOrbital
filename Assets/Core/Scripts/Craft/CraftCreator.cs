using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;

public class CraftCreator : MonoBehaviour
{
    public void SelectCraft(ItemCraftScriptableObject itemCraft)
    {
        Debug.Log("Tiene "+itemCraft.itemsRequired.Count);
    }
}
