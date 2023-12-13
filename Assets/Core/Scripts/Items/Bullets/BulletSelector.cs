using InterOrbital.Item;
using InterOrbital.Player;
using System.Collections.Generic;
using DG.Tweening;
using InterOrbital.Mission;
using UnityEngine;

namespace InterOrbital.UI
{
    public class BulletSelector : MonoBehaviour
    {
        private const int MAXIMUM_BULLETS = 4;

        [SerializeField] private List<BulletSlot> bulletsSlots;
        [SerializeField] private List<ItemBulletScriptableObject> bulletsItems;
        [SerializeField] private ItemBulletScriptableObject emptyBullet;
        [SerializeField] private ItemBulletScriptableObject _defaultBullet;

        [SerializeField] private Sprite bulletSlotSelectedImage;
        [SerializeField] private Sprite bulletSlotNoSelectedImage;

        [SerializeField] private UIAnimationStruct _selectAnimation, _deselectAnimation;
        [SerializeField] private float _animationDuration = 0.5f;

        public static BulletSelector Instance;

        private RectTransform _rectTransform;
        private int _selectedBulletIndex = 0;
        private bool _equipMissionCompleted;
        private MissionCreator _missionCreator;
        private PlayerAttack _playerAttack;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }

            _rectTransform = GetComponent<RectTransform>();
            _missionCreator = FindObjectOfType<MissionCreator>();
        }

        private void Start()
        {
            _playerAttack = PlayerComponents.Instance.PlayerAttack;
            InitializeBulletSelector();
        }

        private void Update()
        {
            CheckBulletMission();
        }

        private void CheckBulletMission()
        {
            if (_equipMissionCompleted || !bulletsItems.Contains(_defaultBullet)) return;
            _equipMissionCompleted = true;
            _missionCreator.UpdateMission(1, "Equip");
        }

        private void InitializeBulletSelector()
        {
            for (int i = 0; i < MAXIMUM_BULLETS; i++)
            {
                bulletsItems.Add(emptyBullet);
                bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
                bulletsSlots[i].SetBackgroundSprite(bulletSlotNoSelectedImage);
                bulletsSlots[i].SetBulletAmount(0);
            }

            bulletsSlots[_selectedBulletIndex].SetBackgroundSprite(bulletSlotSelectedImage);
            ResetSelectedBulletsTransparency();
            SetupCooldowns();
            ChangePlayerBullet();
        }

        private void SetupCooldowns()
        {
            _playerAttack.SetupCooldowns(bulletsItems, bulletsSlots);
        }

        private void ResetSelectedBulletsTransparency()
        {
            for (int i = 0; i < bulletsSlots.Count; i++)
            {
                if (i == _selectedBulletIndex)
                {
                    bulletsSlots[i].SetBulletAsSelected();
                }
                else
                {
                    bulletsSlots[i].SetBulletAsNoSelected();
                }
            }
        }

        public void UpdateBulletSelectorUI()
        {
            for (int i = 0; i < bulletsSlots.Count; i++)
            {

                int index = PlayerComponents.Instance.Inventory.GetStartIndexBulletSlot() + i;

                ItemObject itemInInventory = PlayerComponents.Instance.Inventory.GetItemObjectByIndex(index);
                if (itemInInventory.itemSo != PlayerComponents.Instance.Inventory.itemVoid)
                {
                    bulletsItems[i] = itemInInventory.itemSo as ItemBulletScriptableObject;
                }
                else
                {
                    bulletsItems[i] = emptyBullet;
                }

                bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
                bulletsSlots[i].SetBulletAmount(itemInInventory.amount);
            }
            ResetSelectedBulletsTransparency();
            ChangePlayerBullet();
        }

        public void ChangeBulletInList(int index, ItemBulletScriptableObject bullet)
        {
            if (index < bulletsItems.Count)
            {
                bulletsItems[index] = bullet;
            }
        }

        public void UpdateSelectedBullet(int index)
        {
            var lastSelectedBulletIndex = _selectedBulletIndex;
            _selectedBulletIndex = index;

            bulletsSlots[lastSelectedBulletIndex].SetBackgroundSprite(bulletSlotNoSelectedImage);
            bulletsSlots[_selectedBulletIndex].SetBackgroundSprite(bulletSlotSelectedImage);
            ResetSelectedBulletsTransparency();
            ChangePlayerBullet();
        }

        public void ChangePlayerBullet()
        {
            if (_selectedBulletIndex < bulletsItems.Count)
            {
                _playerAttack.ChangeBullet(bulletsItems[_selectedBulletIndex].bulletPrefab,
                    bulletsItems[_selectedBulletIndex].shotSFX, _selectedBulletIndex);
            }
        }

        public void SubstractBullet()
        {
            int j = _selectedBulletIndex;
            PlayerComponents.Instance.Inventory.SubstractBulletInInventory(j);
            UpdateBulletSelectorUI();
        }

        public void SelectAnimation()
        {
            AudioManager.Instance.PlaySFX("UIMenu");
            _rectTransform.DOKill();
            _rectTransform.DOMove(_selectAnimation.position, _animationDuration).SetEase(Ease.OutBack);
            _rectTransform.DOScale(_selectAnimation.scale, _animationDuration).SetEase(Ease.OutBack);
        }

        public void DeselectAnimation()
        {
            AudioManager.Instance.PlaySFX("UIMenuReverse");
            _rectTransform.DOKill();
            _rectTransform.DOMove(_deselectAnimation.position, _animationDuration).SetEase(Ease.OutBack);
            _rectTransform.DOScale(_deselectAnimation.scale, _animationDuration).SetEase(Ease.OutBack);
        }
    }
}
