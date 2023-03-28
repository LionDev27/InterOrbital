using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerEnergy : MonoBehaviour
{
    public bool energyEmpty;

    [SerializeField]
    private int _currentEnergy;
    [SerializeField]
    private int _maxEnergy;
    [SerializeField] private float _loseEnergyTimerDefaultValue;
    private float _loseEnergyTimer;

    public Image img;
    private void Start()
    {
        _currentEnergy = _maxEnergy;
        energyEmpty = false;
        ResetTimer();
    }

    private void Update()
    {
        LoseEnergyByTime();
        img.fillAmount = (float)_currentEnergy / _maxEnergy;

        if (Input.GetKeyDown(KeyCode.R))
            RestoreEnergy(5);

        if (Input.GetKeyDown(KeyCode.L))
            LoseEnergy(3);
    }

    private void LoseEnergyByTime() {
        if (!energyEmpty)
        {
            if (_loseEnergyTimer > 0)
            {
                _loseEnergyTimer -= Time.deltaTime;
            }
            else
            {
                _currentEnergy = (int)Mathf.Clamp(_currentEnergy - 1, 0, _maxEnergy);
                ResetTimer();
                CheckEnergy();
            }
        }
    }


    public void RestoreEnergy(float energy)
    {
        _currentEnergy = (int)Mathf.Clamp(_currentEnergy + energy,0,_maxEnergy);
        ResetTimer();
        CheckEnergy();
    }

    public void LoseEnergy(float energy)
    {
        _currentEnergy = (int)Mathf.Clamp(_currentEnergy - energy, 0, _maxEnergy);
        ResetTimer();
        CheckEnergy();
    }

    private void CheckEnergy()
    {
        if (_currentEnergy <= 0)
        {
            energyEmpty = true;
            ResetTimer();
        }
        else
            energyEmpty = false;
    }

    private void ResetTimer() {
        _loseEnergyTimer = _loseEnergyTimerDefaultValue;
    }

    
}
