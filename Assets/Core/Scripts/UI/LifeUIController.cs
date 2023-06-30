using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [SerializeField] List<GameObject> lifePrefabs;
    private int lifeTier = 0;

    private void Start()
    {
        lifePrefabs[lifeTier].SetActive(true);
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if(Input.GetKeyDown(KeyCode.H)) {
                UpgradeLifeTier();
            }
        }
    }

    private void ActiveLifeBarTier()
    {
        if(lifePrefabs[lifeTier] != null)
        {
            lifePrefabs[lifeTier].SetActive(true);
        }
    }

    private void DesactiveLifeBarTier()
    {
        if (lifePrefabs[lifeTier] != null)
        {
            lifePrefabs[lifeTier].SetActive(false);
        }
    }

    private void UpgradeLifeTier()
    {
        if (lifeTier + 1 < lifePrefabs.Count)
        {
            DesactiveLifeBarTier();
            lifeTier = Mathf.Clamp(lifeTier + 1, 0, lifePrefabs.Count);
            ActiveLifeBarTier();
        }
    }

    public LifeBarUIController GetLifeTierBarUIController()
    {
        if (lifePrefabs[lifeTier] != null)
        {
            return lifePrefabs[lifeTier].GetComponent<LifeBarUIController>();
        }
        return null;
    }
}
