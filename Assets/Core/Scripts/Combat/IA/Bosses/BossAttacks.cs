using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAttacks : MonoBehaviour
    {
        public bool Attacking => _attacking;
        public float attackTime;
        
        [Tooltip("Ataque que realizará cuando el jugador se encuentre cerca.")]
        [SerializeField] private GameObject _closeAttack;
        [SerializeField] private List<GameObject> _attacks;
        private bool _attacking;

        private void Start()
        {
            DeactivateAttacks();
        }

        public void RandomAttack()
        {
            var index = Random.Range(0, _attacks.Count);
            ActivateAttack(_attacks[index]);

        }

        public void CloseAttack()
        {
            ActivateAttack(_closeAttack);
        }

        public void StartAttack()
        {
            _attacking = true;
        }

        public void EndAttack()
        {
            _attacking = false;
        }

        private void ActivateAttack(GameObject attack)
        {
            attack.SetActive(true);
        }

        private void DeactivateAttacks()
        {
            _closeAttack.SetActive(false);
            foreach (var attack in _attacks)
                attack.SetActive(false);
        }
    }
}