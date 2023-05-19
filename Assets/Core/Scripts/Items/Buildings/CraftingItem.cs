using System;
using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour, IInteractable
    {
        private GameObject _craftUI;
        private CraftGrid _craftGrid;
        private CraftCreator _craftCreator;
        [SerializeField] private Image itemCraftImage;
        [SerializeField] private Image progressBar;
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

        public void CreateItem(ItemCraftScriptableObject itemCraft, int amount)
        {
            itemCraftImage.sprite = itemCraft.itemSprite;
            progressBar.fillAmount = 0;
            progressBar.DOFillAmount(1f, itemCraft.timeToCraft).SetEase(Ease.Linear);
            
        }
    }
}
