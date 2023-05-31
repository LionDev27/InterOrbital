using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipEnergy : MonoBehaviour
{
    [SerializeField] private int _currentEnergy;
    [SerializeField] private int _maxEnergy;
    [SerializeField] private Image _spaceshipEnergyImg;

    private void Start()
    {
        _currentEnergy = _maxEnergy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoseEnergy(5);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            RestoreEnergy(1);
        }
    }

    public void RestoreEnergy(int energyAmount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy + energyAmount, 0, _maxEnergy);
        UpdateSpaceshipEnergyUI();
    }

    public void LoseEnergy(int energyAmount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy - energyAmount, 0, _maxEnergy);
        UpdateSpaceshipEnergyUI();
    }

    public void UpgradeEnergy(int energyAmount)
    {
        _maxEnergy += energyAmount;
    }

    public int GetCurrentSpaceshipEnergy() 
    { 
        return _currentEnergy; 
    }

    private void UpdateSpaceshipEnergyUI()
    {
        float fillAmount = _currentEnergy / (float)_maxEnergy;
        _spaceshipEnergyImg.fillAmount = fillAmount;
    }
}
