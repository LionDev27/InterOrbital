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

namespace InterOrbital.Item
{
    public class CraftingTable : CraftingItem
    {
        [SerializeField] private Image _itemCraftImage;
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _textAmount;
        [SerializeField] private GameObject _craftContent;
        [SerializeField] private Transform dropPosition;
        [SerializeField] private TypeTableCraft typeTable;
        private Queue<CraftAmountItem> _queueCraft;
        private MissionCreator _missionCreator;
        private bool _isCrafting;


        private void Awake()
        {
            _missionCreator = FindObjectOfType<MissionCreator>();
        }

        protected override void Start()
        {
            switch (typeTable)
            {
                case TypeTableCraft.Craft:
                    _craftUI = UIManager.Instance.craftUI;
                    break;
                case TypeTableCraft.Fundition:
                    _craftUI = UIManager.Instance.funditionUI;
                    break;
                case TypeTableCraft.Bullet:
                    _craftUI = UIManager.Instance.bulletUI;
                    break;
            }
           
            base.Start();
            _queueCraft = new Queue<CraftAmountItem>();
        }

        public override void Craft(ItemCraftScriptableObject itemCraft, int amount)
        {
            _queueCraft.Enqueue(new CraftAmountItem(itemCraft, amount));
            _missionCreator.UpdateMission(amount, itemCraft.itemName);
            if (!_isCrafting)
            {
                StartCoroutine(CreateItem());
            }
        }

        public IEnumerator CreateItem()
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
        }

        public void Interact()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
            ProccessSelection();
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_craftUI);
        }

    }
}