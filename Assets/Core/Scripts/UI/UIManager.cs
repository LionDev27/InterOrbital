using UnityEngine;
using DG.Tweening;
using InterOrbital.Player;

namespace InterOrbital.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform _inventoryInitPosition;
        private Tween _openInventory;


        [SerializeField] private EnergyUIController _energyUIController;
        [SerializeField] private LifeUIController _lifeUIController;


        private bool _somethingOpen;
        public static UIManager Instance = null;
        [HideInInspector] public bool isChestOpen;
        public ChestInventory chestInventory;
        public GameObject bagUI;
        public GameObject craftUI;
        public GameObject storageUI;
        public bool IsAnimating { get; private set; }
        
        
        private void Awake()
        {
            if(Instance == null) 
                Instance = this; 
            else if(Instance != this) 
                Destroy(gameObject);

        }

       
        public void ActivateOrDesactivateUI(GameObject ui)
        {
            if (ui.activeSelf)
            {
                IsAnimating = true;
                ui.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear).Play().OnComplete(() =>
                {
                    IsAnimating = false;
                    ui.SetActive(false);
                    _somethingOpen = false;
                });  
            }
            else if(!_somethingOpen)
            {
                ui.SetActive(true);
                _somethingOpen = true;
                IsAnimating = true;
                ui.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).Play().OnComplete(() =>
                {
                    IsAnimating = false;
                });
            }
            
           
        }

        public void OpenInventory(bool openChest)
        {

            if(PlayerComponents.Instance.Inventory.isHide && !_somethingOpen)
            {
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
                    _somethingOpen = true;
                    IsAnimating = true;
                    _openInventory = bagUI.transform.DOMoveY(Screen.height / 2, 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = false;
                        IsAnimating = false;
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
                    IsAnimating = true;
                    _openInventory = bagUI.transform.DOMoveY(_inventoryInitPosition.transform.position.y , 0.5f).Play().OnComplete(() =>
                    {
                        IsAnimating = false;
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


    }
}



