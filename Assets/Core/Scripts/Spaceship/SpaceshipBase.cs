using InterOrbital.UI;
using InterOrbital.Item;
using UnityEngine;

namespace InterOrbital.Spaceship
{
    public class SpaceshipBase : BaseInteractable
    {
        private GameObject _spaceshipUI;
        private SpaceshipGrid _spaceshipGrid;
        private SpaceshipEnergyManager _spaceshipEnergyManager;

        protected override void Start()
        {
            _spaceshipUI = UIManager.Instance.spaceshipUI;
            _spaceshipGrid = _spaceshipUI.GetComponentInChildren<SpaceshipGrid>();
            _spaceshipEnergyManager = _spaceshipUI.GetComponentInChildren<SpaceshipEnergyManager>();
        }

        public override void Interact()
        {
            _spaceshipGrid.UpdateFeedBack();
            _spaceshipEnergyManager.UpdateEnergyBar();
            UIManager.Instance.ActivateOrDesactivateUI(_spaceshipUI);
        }

        public override void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_spaceshipUI);
        }

    }

}
