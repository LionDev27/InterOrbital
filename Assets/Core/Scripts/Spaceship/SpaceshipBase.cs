using InterOrbital.UI;
using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Spaceship
{
    public class SpaceshipBase : MonoBehaviour, IInteractable
    {
        private GameObject _spaceshipUI;
        private SpaceshipGrid _spaceshipGrid;
        private SpaceshipEnergyManager _spaceshipEnergyManager;
        // Start is called before the first frame update


        void Start()
        {
            _spaceshipUI = UIManager.Instance.spaceshipUI;
            _spaceshipGrid = _spaceshipUI.GetComponentInChildren<SpaceshipGrid>();
            _spaceshipEnergyManager = _spaceshipUI.GetComponentInChildren<SpaceshipEnergyManager>();
        }

        public void Interact()
        {
            _spaceshipGrid.UpdateFeedBack();
            _spaceshipEnergyManager.UpdateEnergyBar();
            UIManager.Instance.ActivateOrDesactivateUI(_spaceshipUI);
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_spaceshipUI);
        }

    }

}
