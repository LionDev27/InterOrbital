using UnityEngine;
using DG.Tweening;
using InterOrbital.Item;
using InterOrbital.Player;
using UnityEngine.UI;

namespace InterOrbital.UI
{
    public class UIManager : MonoBehaviour
    {
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

        private bool _somethingOpen;
        public static UIManager Instance = null;
        [HideInInspector] public bool isChestOpen;
        public ChestInventory chestInventory;
        public GameObject bagUI;
        public GameObject craftUI;
        public GameObject spaceshipUI;
        public GameObject storageUI;
        public GameObject blackout;
        
        
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
                blackout.SetActive(false);
                PlayerComponents.Instance.PlayerEnergy.ResumeLoseEnergyOverTime();
                ui.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear).Play().OnComplete(() =>
                {
                    _somethingOpen = false;
                });  
            }
            else if(!_somethingOpen)
            {
                blackout.SetActive(true);
                PlayerComponents.Instance.PlayerEnergy.StopLoseEnergyOverTime();
                _somethingOpen = true;
                ui.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).Play();
            }
            
           
        }

        public void OpenInventory(bool openChest)
        {

            if(PlayerComponents.Instance.Inventory.isHide && !_somethingOpen)
            {
                _fastCraft.ProccessSelection();
                if (!_openInventory.IsActive())
                {
                    if (openChest)
                    {
                        storageUI.SetActive(true);
                        isChestOpen = true;
                    }
                    else
                    {
                        PlayerComponents.Instance.InputHandler.ChangeActionMap();
                        storageUI.SetActive(false);
                        isChestOpen = false;
                    }
                    blackout.SetActive(true);
                    PlayerComponents.Instance.PlayerEnergy.StopLoseEnergyOverTime();
                    _somethingOpen = true;
                    _openInventory = bagUI.transform.DOMoveY(Screen.height / 2, 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = false;
                    });
                    
                }    
            }
            else if(!PlayerComponents.Instance.Inventory.isHide)
            {
                if (!_openInventory.IsActive())
                {
                    _somethingOpen = false;
                    if (!openChest)
                    {
                        PlayerComponents.Instance.InputHandler.ChangeActionMap();
                    }
                    blackout.SetActive(false);
                    PlayerComponents.Instance.PlayerEnergy.ResumeLoseEnergyOverTime();
                    _openInventory = bagUI.transform.DOMoveY(_inventoryInitPosition.transform.position.y , 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = true;
                        if (openChest)
                        {
                            storageUI.SetActive(false);
                            isChestOpen = false;
                        }
                    });
                }
            }
        }

        public void UpdateEnergyUI(int maxEnergy,int currentEnergy)
        {
            _energyUIController.GetEnergyTierBarsUIController().UpdateEnergy(maxEnergy,currentEnergy);
        }
        
        public void UpdateLifeUI(int maxLife,int currentLife)
        {
            _lifeUIController.GetLifeTierBarUIController().UpdateLife(maxLife,currentLife);
        }

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
    }
}



