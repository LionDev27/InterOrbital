using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] protected int _maxHealth;
        protected int _currentHealth;

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void GetDamage(int damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            CheckHealth(); 
        }

        public virtual void RestoreHealth(int healthAmount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + healthAmount, 0, _maxHealth);
        }

        public virtual void UpgradeHealth(int healthAmount)
        {
            _maxHealth += healthAmount;
        }

        protected virtual void CheckHealth()
        {
            if (_currentHealth <= 0)
            {
                Death();
            }
        }

        protected virtual void Death(){}
    }
}
