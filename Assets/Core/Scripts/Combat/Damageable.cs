using UnityEngine;

namespace InterOrbital.Combat
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        private int _currentHealth;

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void GetDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log($"{gameObject.name} current health: {_currentHealth}");
            if (_currentHealth <= 0)
                Death();
        }

        protected virtual void Death(){}
    }
}
