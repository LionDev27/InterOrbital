using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InterOrbital.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;
        private Vector3 _inventoryInitPosition;

        public GameObject bagUI;
        private void Awake()
        {
            if(Instance == null) 
                Instance = this; 
            else if(Instance != this) 
                Destroy(gameObject); 
           // DontDestroyOnLoad(gameObject); 
        }

        private void Start()
        {
            _inventoryInitPosition = new Vector3();
            _inventoryInitPosition = bagUI.transform.position;
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
            if(bagUI.transform.position == _inventoryInitPosition)
            {
                bagUI.transform.DOMoveY(Screen.height/2, 0.5f).Play();
            }
            else
            {
                bagUI.transform.DOMoveY(_inventoryInitPosition.y, 0.5f).Play();
            }
        }
    }
}



