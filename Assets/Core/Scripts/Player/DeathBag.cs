using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBag : MonoBehaviour
{
    public List<Items> playerItems;

    private void Start()
    {
        PlayerComponents.Instance.GetComponent<PlayerInventory>().FillDeathBag(this);
        PlayerComponents.Instance.GetComponent<PlayerInventory>().ResetInventory();
    }

    public void FillPlayerInventory()
    {
        foreach (var item in playerItems)
        {
            PlayerComponents.Instance.GetComponent<PlayerInventory>().AddOneItemSOwithAmount(item.item,item.amountRequired);
        }
        Destroy(gameObject);
    }
}
