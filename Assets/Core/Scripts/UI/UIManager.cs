using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null; 
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
            ui.SetActive(!ui.activeSelf);
        }
    }
}



