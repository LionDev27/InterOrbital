using System;
using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using InterOrbital.Player;
using TMPro;
using System.Collections.Generic;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour, IInteractable
    {
        private GameObject _craftUI;
        private CraftGrid _craftGrid;
        private CraftCreator _craftCreator;
        private Queue<CraftAmountItem> _queueCraft;
        private bool _isCrafting;
        [SerializeField] private Image _itemCraftImage;
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _textAmount;
        [SerializeField] private GameObject _craftContent;
        [SerializeField] private Transform dropPosition;

        [HideInInspector]
        public ItemCraftScriptableObject currentCraftSelected;
        

        

        private void Start()
        {
            _craftUI = UIManager.Instance.craftUI;
            _craftGrid = _craftUI.GetComponentInChildren<CraftGrid>();
            _craftCreator = _craftUI.GetComponentInChildren<CraftCreator>();
            _queueCraft = new Queue<CraftAmountItem>();
        }

        public void Interact()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
            if (_craftUI.activeSelf)
            {
                _craftCreator.SetCraftingItem(this);
                _craftGrid.currentCraftingItem = this;
                _craftGrid.UpdateFeedback();
                _craftGrid.SelectLast();
            }
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
            if (_craftUI.activeSelf)
            {
                _craftGrid.UpdateFeedback();
                _craftGrid.SelectLast();
                _craftGrid.currentCraftingItem = null;
            }
        }

        public void Craft(ItemCraftScriptableObject itemCraft, int amount)
        {
            _queueCraft.Enqueue(new CraftAmountItem(itemCraft, amount));
            if (!_isCrafting)
            {
                StartCoroutine(CreateItem());
            }
        }

        public IEnumerator CreateItem()
        {
            _craftContent.SetActive(true);
            _isCrafting = true;

            while(_queueCraft.Count > 0)
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
                        PlayerComponents.Instance.Inventory.DropItem(dropPosition.position, transform.position, -1, craftItem.item);
                    }); ;

                    yield return new WaitUntil(() => _progressBar.fillAmount == 1);
                    i++;

                }
            }
            
            _craftContent.SetActive(false);
            _isCrafting = false;
        }
    }
}
