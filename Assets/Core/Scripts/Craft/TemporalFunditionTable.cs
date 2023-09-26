using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InterOrbital.Item;
using InterOrbital.Mission;
using InterOrbital.Player;
using InterOrbital.UI;
using InterOrbital.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace InterOrbital.Item
{
    public class TemporalFunditionTable : CraftingTable
    {
        private const int maxCraftsAllowed = 3;
        private int _currentCraftsAmount = 0;

        protected override void Start()
        {
            _currentCraftsAmount = 0;
            base.Start();
            _craftCreator.SetMaxCraftsAmount(maxCraftsAllowed);
            _craftCreator.SetCraftsLeftAmount(maxCraftsAllowed - _currentCraftsAmount);
        }

        public override void Craft(ItemCraftScriptableObject itemCraft, int amount)
        {
            if(amount <= (maxCraftsAllowed - _currentCraftsAmount)) 
            {
                _currentCraftsAmount += amount;
                _craftCreator.SetCraftsLeftAmount(maxCraftsAllowed - _currentCraftsAmount);
                if(_currentCraftsAmount >= maxCraftsAllowed)
                {
                    PlayerComponents.Instance.GetComponent<PlayerInteraction>().Interact();
                }
                base.Craft(itemCraft, amount);
            }
        }

        public override IEnumerator CreateItem()
        {
            _craftContent.SetActive(true);
            _isCrafting = true;

            while (_queueCraft.Count > 0)
            {
                CraftAmountItem craftItem = _queueCraft.Dequeue();
                _itemCraftImage.sprite = craftItem.item.itemSprite;
                int i = 0;
                while (i < craftItem.amount)
                {
                    _textAmount.text = "x" + (craftItem.amount - i).ToString();
                    _progressBar.fillAmount = 0;
                    _progressBar.DOFillAmount(1f, craftItem.item.timeToCraft).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        for (int j = 0; j < craftItem.item.amountToCraft; j++)
                        {
                            PlayerComponents.Instance.Inventory.DropItem(dropPosition.position, transform.position, -1, craftItem.item);
                        }
                    });

                    yield return new WaitUntil(() => _progressBar.fillAmount == 1);
                    i++;
                }
            }

            _craftContent.SetActive(false);
            _isCrafting = false;
            if(_currentCraftsAmount >= maxCraftsAllowed)
            {
                Destroy(gameObject);
            }
        }

        public override void Interact()
        {
            Debug.Log(_currentCraftsAmount);
            Debug.Log(maxCraftsAllowed);

            if(_currentCraftsAmount < maxCraftsAllowed)
            {
                UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
                ProccessSelection();
            }
        }

        public override void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
        }

    }
}