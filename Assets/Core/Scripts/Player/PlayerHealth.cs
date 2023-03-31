using InterOrbital.Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _loseHealthTimerDefaultValue;
    private float _loseHealthTimer;

    private void Update()
    {
        LoseHealthOverTime();
    }

    private void LoseHealthOverTime()
    {
        bool energyEmpty = PlayerComponents.Instance.PlayerEnergy.EnergyEmpty;
        if (energyEmpty)
        {
            if (_loseHealthTimer > 0)
            {
                _loseHealthTimer -= Time.deltaTime;
            }
            else
            {
                _currentHealth = Mathf.Clamp(_currentHealth - 1, 0, _maxHealth);
                ResetTimer();
                CheckHealth();
            }
        }
    }

    public void RestoreEnergy(int healthAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + healthAmount, 0, _maxHealth);
        ResetTimer();
        CheckHealth();
    }

    public void LoseEnergy(int healthAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - healthAmount, 0, _maxHealth);
        ResetTimer();
        CheckHealth();
    }

    public void UpgradeHealth(int healthAmount)
    {
        _maxHealth += healthAmount;
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0)
        {
            ResetTimer();
        }
        else
            Dead();
    }

    private void ResetTimer()
    {
        _loseHealthTimer = _loseHealthTimerDefaultValue;
    }

    private void Dead()
    {
        //Método que genera el evento de muerte
    }
}
