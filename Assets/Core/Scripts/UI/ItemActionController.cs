using InterOrbital.Item;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionController : MonoBehaviour
{
    public Sprite _buildableItem;
    public Sprite _energyRestoreItem;
    public Sprite _healthRestoreItem;
    public Sprite _recollectUpgrade;
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

            if (item.consumableValues.consumableType == ConsumableType.Recollector)
            {
                _actionImage.sprite = _recollectUpgrade;
            }
        }

        if(item.type == ItemType.Bullet)
        {
            _actionImage.sprite = _emptyImage;
        }

        if (item.type == ItemType.Upgrade)
        {
            if (item.upgradeType == UpgradeType.Elytrum)
            {
                _actionImage.sprite = _energyRestoreItem;
            }
            if (item.upgradeType == UpgradeType.Health)
            {
                _actionImage.sprite = _healthRestoreItem;
            }
        }

        if (item.type == ItemType.None)
        {
            _actionImage.sprite = _emptyImage;
        }
    }
}
