using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUIController : MonoBehaviour
{
    public int RemainingTiers => energyPrefabs.Count - energyTier;
    
    [SerializeField] List<GameObject> energyPrefabs;
    private int energyTier = 0;

    private void Start()
    {
        energyPrefabs[energyTier].SetActive(true);
    }

    private void ActiveEnergyBarTier()
    {
        if(energyPrefabs[energyTier] != null)
        {
            energyPrefabs[energyTier].SetActive(true);
        }
    }

    private void DesactiveEnergyBarTier()
    {
        if (energyPrefabs[energyTier] != null)
        {
            energyPrefabs[energyTier].SetActive(false);
        }
    }

    public void UpgradeEnergyTier()
    {
        if (energyTier + 1 < energyPrefabs.Count)
        {
            DesactiveEnergyBarTier();
            energyTier = Mathf.Clamp(energyTier + 1, 0, energyPrefabs.Count);
            ActiveEnergyBarTier();
        }
    }

    public EnergyBarsUIController GetEnergyTierBarsUIController()
    {
        if (energyPrefabs[energyTier] != null)
        {
            return energyPrefabs[energyTier].GetComponent<EnergyBarsUIController>();
        }
        return null;
    }
}
