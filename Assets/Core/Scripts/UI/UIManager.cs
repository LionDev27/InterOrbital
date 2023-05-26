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

        public static UIManager Instance = null;

        public GameObject bagUI;
        public GameObject craftUI;
        public GameObject storageUI;
        
        private void Awake()
        {
            if(Instance == null) 
                Instance = this; 
            else if(Instance != this) 
                Destroy(gameObject); 
           // DontDestroyOnLoad(gameObject); 
        }

       

        public void ActivateOrDesactivateUI(GameObject ui)
        {
            if (ui.activeSelf)
            {
                ui.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear).Play().OnComplete(() =>
                {
                    ui.SetActive(false);
                });  
            }
            else
            {
                ui.SetActive(true);
                ui.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).Play();
            }
           
        }

        public void OpenInventory()
        {
            if(PlayerComponents.Instance.Inventory.isHide)
            {
                if (!_openInventory.IsActive())
                {
                    PlayerComponents.Instance.InputHandler.ChangeActionMap();
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
                    PlayerComponents.Instance.InputHandler.ChangeActionMap();
                    _openInventory = bagUI.transform.DOMoveY(_inventoryInitPosition.transform.position.y , 0.5f).Play().OnComplete(() =>
                    {
                        PlayerComponents.Instance.Inventory.isHide = true;
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



