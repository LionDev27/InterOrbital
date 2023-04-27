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
        }

        private void Update()
        {
            UpdateCraft();
        }

        private void UpdateCraft()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIManager.Instance.ActivateOrDesactivateUI(craftUI);
            }
        }
    }
}
