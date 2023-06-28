using InterOrbital.Item;
using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionController : MonoBehaviour
{
    public Sprite _buildableItem;
    public Sprite _energyRestoreItem;
    public Sprite _healthRestoreItem;
    public Sprite _emptyImage;
    public Image _actionImage;

    public void ChangeActionImage(ItemScriptableObject item)
    {
        if(item.type == ItemType.Build)
        {
            _actionImage.sprite = _buildableItem;
        }

        if(item.type == ItemType.Consumable)
        {
            if(item.consumableValues.consumableType == ConsumableType.Elytrum)
            {
                _actionImage.sprite = _energyRestoreItem;
            }
            if (item.consumableValues.consumableType == ConsumableType.Health)
            {
                _actionImage.sprite = _healthRestoreItem;
            }
        }

        if(item.type == ItemType.Bullet)
        {
            _actionImage.sprite = _emptyImage;
        }

        if(item.type == ItemType.None)
        {
            _actionImage.sprite = _emptyImage;
        }
    }
}
