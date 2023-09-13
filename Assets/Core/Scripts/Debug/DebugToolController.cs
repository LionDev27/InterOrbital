using InterOrbital.Player;
using InterOrbital.Spaceship;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Test
{
    public class DebugToolController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        
        private void Update()
        {
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.P))
                    ToggleEnableDebugMenu();
            }
        }

        private void ToggleEnableDebugMenu()
        {
            _panel.SetActive(!_panel.activeInHierarchy);
            PlayerComponents.Instance.InputHandler.ChangeActionMap();
        }

        public void ToggleGodMode()
        {
            PlayerComponents.Instance.PlayerDamageable.ToggleGodMode();
            PlayerComponents.Instance.PlayerEnergy.ToggleLoseEnergy();
            
        }

        public void SpawnResourcesAndEnemies()
        {

        }

        public void RestoreSpaceshipEnergy()
        {
            FindObjectOfType<SpaceshipEnergy>().RestoreEnergy(1);
        }

        public void LoseSpaceshipEnergy()
        {
            FindObjectOfType<SpaceshipEnergy>().LoseEnergy(5);
        }
    }
}
