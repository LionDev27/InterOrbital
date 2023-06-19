using InterOrbital.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Spaceship
{
    public class SpaceshipGrid : MonoBehaviour
    {
        private List<SpaceshipFuelSlot> _fuelSlots;
        private SpaceshipEnergyManager _spaceShipEnergyManager;
        [SerializeField] private List<ItemFuelScriptableObject> _fuelItems;
        [SerializeField] private GameObject _gridFuelPrefab;

        private void Awake()
        {
            _spaceShipEnergyManager = GetComponentInParent<SpaceshipEnergyManager>();
        }

        private void Start()
        {
            _fuelSlots = new List<SpaceshipFuelSlot>();
            for (int i = 0; i < _fuelItems.Count; i++)
            {
                var newFuelItem = Instantiate(_gridFuelPrefab);
                newFuelItem.transform.SetParent(gameObject.transform, false);
                SpaceshipFuelSlot fuelSlot = newFuelItem.GetComponent<SpaceshipFuelSlot>();
                fuelSlot.SetFuelItem(_fuelItems[i]);
                fuelSlot.SetSpaceShipManager(_spaceShipEnergyManager);
                _fuelSlots.Add(fuelSlot);
            }
        }

        public void UpdateFeedBack()
        {
            for(int i=0; i < _fuelSlots.Count; i++)
            {
                _fuelSlots[i].FindInInventory();
            }
        }

    }

}
