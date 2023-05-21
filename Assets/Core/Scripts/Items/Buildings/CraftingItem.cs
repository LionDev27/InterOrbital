using System;
using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using InterOrbital.Player;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour, IInteractable
    {
        private GameObject _craftUI;
        private CraftGrid _craftGrid;
        private CraftCreator _craftCreator;
        [SerializeField] private Image _itemCraftImage;
        [SerializeField] private Image _progressBar;
        [SerializeField] private GameObject _craftContent;
        [SerializeField] private Transform dropPosition;
        [HideInInspector]
        public ItemCraftScriptableObject currentCraftSelected;
        

        private void Start()
        {
            _craftUI = UIManager.Instance.craftUI;
            _craftGrid = _craftUI.GetComponentInChildren<CraftGrid>();
            _craftCreator = _craftUI.GetComponentInChildren<CraftCreator>();
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
            StartCoroutine(CreateItem(itemCraft, amount));
        }

        public IEnumerator CreateItem(ItemCraftScriptableObject itemCraft, int amount)
        {
            _itemCraftImage.sprite = itemCraft.itemSprite;
            int i = 0;
            _craftContent.SetActive(true);
            while (i < amount)
            {
                _progressBar.fillAmount = 0;
                _progressBar.DOFillAmount(1f, itemCraft.timeToCraft).SetEase(Ease.Linear).OnComplete(() =>
                { 
                    PlayerComponents.Instance.Inventory.DropItem(dropPosition.position,transform.position, -1, itemCraft);  
                }); ;
                
                yield return new WaitUntil(() => _progressBar.fillAmount == 1);
                i++;

            }
            
            _craftContent.SetActive(false);

           
            
            
        }
    }
}
