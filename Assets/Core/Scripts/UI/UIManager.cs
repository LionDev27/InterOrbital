using UnityEngine;
using DG.Tweening;
using InterOrbital.Item;
using InterOrbital.Others;
using InterOrbital.Player;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InterOrbital.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool SomethingOpen => _somethingOpen;
        
        [SerializeField] private Transform _inventoryInitPosition;
        private Tween _openInventory;
        [SerializeField] private EnergyUIController _energyUIController;
        [SerializeField] private LifeUIController _lifeUIController;
        [SerializeField] private CraftingItem _fastCraft;
        [SerializeField] private MinimapController _minimapController;
        [SerializeField] private ItemActionController _itemActionController;
        [SerializeField] private CanvasGroup _tagButtonsInventory;
        [SerializeField] private GameObject _fastingCraft;
        [SerializeField] private GameObject _bulletSelector;
        [SerializeField] private GameObject _clockTime;
        [SerializeField] private Transform _warnPanelInitPosition;
        [SerializeField] private Transform _warnPanelFinalPosition;
        private bool _somethingOpen;
        private GameObject _currentUI;
        
        public static UIManager Instance = null;
        [HideInInspector] public bool isChestOpen;
        [HideInInspector] public bool animating;
        public ChestInventory chestInventory;
        public GameObject bagUI;
        public GameObject craftUI;
        public GameObject funditionUI;
        public GameObject temporalFunditionUI;
        public GameObject bulletUI;
        public GameObject spaceshipUI;
        public GameObject storageUI;
        public GameObject tablesBlackout;
        public GameObject inventoryBlackout;
        public GameObject warnPanel;
        public GameObject pauseUI;

        private void Awake()
        {
            if(Instance == null) 
                Instance = this; 
            else if(Instance != this) 
                Destroy(gameObject);

            _tagButtonsInventory = bagUI.GetComponentInChildren<CanvasGroup>();
        }
        
        public void ActivateOrDesactivateUI(GameObject ui)
        {
            if (ui.transform.localScale == Vector3.one)
            {
                animating = true;
                AudioManager.Instance.PlaySFX("UIMenuReverse");
                tablesBlackout.SetActive(false);
                PlayerComponents.Instance.PlayerEnergy.ResumeLoseEnergyOverTime();
                ui.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear).Play().OnComplete(() =>
                {
                    _somethingOpen = false;
                    animating = false;
                });
            }
            else if(!_somethingOpen)
            {
                animating = true;
                AudioManager.Instance.PlaySFX("UIMenu");
                tablesBlackout.SetActive(true);
                PlayerComponents.Instance.PlayerEnergy.StopLoseEnergyOverTime();
                _somethingOpen = true;
                ui.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).Play().OnComplete(() => { animating = false; });
            }
        }

        public void OpenInventory(bool openChest)
        {
            if (PlayerComponents.Instance.Inventory.isHide && !_somethingOpen)
            {
                animating = true;
                AudioManager.Instance.PlaySFX("Inventory");
                _fastCraft.ProccessSelection();
                if (!_openInventory.IsActive())
                {
                    if (openChest)
                    {
                        storageUI.SetActive(true);
                        isChestOpen = true;
                        if (_fastingCraft.transform.localScale != Vector3.zero)
                        {
                            CloseFastCraft();
                            _currentUI = _fastingCraft;
                        }
                        else
                        {
                            CloseBulletSelector();
                            _currentUI = _bulletSelector;
                        }
                    }
                    else
                    {
                        PlayerComponents.Instance.InputHandler.ChangeActionMap();
                        storageUI.SetActive(false);
                        isChestOpen = false;
                        OpenLastUI();
                    }
                    inventoryBlackout.SetActive(true);
                    PlayerComponents.Instance.PlayerEnergy.StopLoseEnergyOverTime();
                    _somethingOpen = true;
                    _openInventory = bagUI.transform.DOMoveY(Screen.height / 2, 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = false;
                        animating = false;
                    });
                    
                }    
            }
            else if(!PlayerComponents.Instance.Inventory.isHide)
            {
                if (!_openInventory.IsActive())
                {
                    animating = true;
                    AudioManager.Instance.PlaySFX("Inventory");
                    _somethingOpen = false;
                    if (!openChest)
                    {
                        PlayerComponents.Instance.InputHandler.ChangeActionMap();
                    }
                    inventoryBlackout.SetActive(false);
                    PlayerComponents.Instance.PlayerEnergy.ResumeLoseEnergyOverTime();
                    _openInventory = bagUI.transform.DOMoveY(_inventoryInitPosition.transform.position.y , 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = true;
                        if (openChest)
                        {
                            storageUI.SetActive(false);
                            isChestOpen = false;
                            OpenLastUI();
                        }
                        animating = false;
                    });
                }
            }
        }

        private void OpenLastUI()
        {
            if (_currentUI == null)
            {
                OpenFastCraft();
                _currentUI = _fastingCraft;
                return;
            }
            if (_currentUI == _fastingCraft)
                OpenFastCraft();
            else
                OpenBulletSelector();
        }

        public void UpdateEnergyUI(int maxEnergy,int currentEnergy)
        {
            _energyUIController.GetEnergyTierBarsUIController().UpdateEnergy(maxEnergy,currentEnergy);
        }
        
        public void UpgradeEnergyUI()
        {
            _energyUIController.UpgradeEnergyTier();
        }

        public void EnergyBlink(bool blink)
        {
            if (blink)
            {
                _energyUIController.GetEnergyTierBarsUIController().StartBlink();
            }
            else
            {
                _energyUIController.GetEnergyTierBarsUIController().StopBlink();
            }
        }

        public int RemainingEnergyTiers => _energyUIController.RemainingTiers;
        
        public void UpdateLifeUI(int maxLife,int currentLife)
        {
            _lifeUIController.GetLifeTierBarUIController().UpdateLife(maxLife,currentLife);
        }

        public void UpgradeLifeUI()
        {
            _lifeUIController.UpgradeLifeTier();
        }
        
        public int RemainingLifeTiers => _lifeUIController.RemainingTiers;

        public void OpenFastCraft()
        {
           _fastingCraft.transform.localScale = Vector3.one;
            _tagButtonsInventory.interactable = false;
        }

        public void CloseFastCraft()
        {
            _fastingCraft.transform.localScale = Vector3.zero;
            _tagButtonsInventory.interactable = true;
        }


        public void OpenBulletSelector()
        {
            _bulletSelector.transform.localScale = Vector3.one;
            _tagButtonsInventory.interactable = false;
        }

        public void CloseBulletSelector()
        {
            _bulletSelector.transform.localScale = Vector3.zero;
            _tagButtonsInventory.interactable = true;
        }

        public void ToggleMinimap()
        {
            _minimapController.ToggleMinimap();
        }

        public void ChangeActionUI(ItemScriptableObject item)
        {
            _itemActionController.ChangeActionImage(item);
        }

        public void ToggleClockTime(bool show)
        {
            _clockTime.SetActive(show);
        }

        public void WarnPanelShowOrHide(bool show)
        {
            if (show)
                warnPanel.transform.DOMoveY(_warnPanelFinalPosition.transform.position.y, 1f).Play();
            else
                warnPanel.transform.DOMoveY(_warnPanelInitPosition.transform.position.y, 1f).Play();
        }

        public void PauseGame(bool value)
        {
            pauseUI.SetActive(value);
            Time.timeScale = value ? 0f : 1f;
        }

        public void MainMenu()
        {
            Time.timeScale = 1f;
            LevelManager.Instance.BackMenu(false);
        }
    }
}



