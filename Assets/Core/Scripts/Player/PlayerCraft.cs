using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.UI;

namespace InterOrbital.Player
{      
    public class PlayerCraft : PlayerComponents
    {
        public GameObject craftUI;
        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnOpenCraft += UpdateCraft;
        }
       
        private void UpdateCraft()
        {
            UIManager.Instance.ActivateOrDesactivateUI(craftUI);
        }
    }
}
