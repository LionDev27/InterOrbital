using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.UI;
using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBag : MonoBehaviour
{
    public List<Items> playerItems;
    [SerializeField] private GameObject _minimapSprite;

    private void Start()
    {
        PlayerComponents.Instance.GetComponent<PlayerInventory>().FillDeathBag(this);
        PlayerComponents.Instance.GetComponent<PlayerInventory>().ResetInventory();
        UIManager.Instance.AddToMinimapSprites(_minimapSprite);
    }

    public void FillPlayerInventory()
    {
        foreach (var item in playerItems)
        {
            PlayerComponents.Instance.GetComponent<PlayerInventory>().AddOneItemSOwithAmount(item.item,item.amountRequired);
        }
        UIManager.Instance.RemoveFromMinimapSprites(_minimapSprite);
        Destroy(gameObject);
    }
}
