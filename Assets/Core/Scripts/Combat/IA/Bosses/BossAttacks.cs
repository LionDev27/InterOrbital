using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAttacks : MonoBehaviour
    {
        public bool Attacking => _attacking;

        [Tooltip("Tiempo que tarda en atacar")] [SerializeField]
        private float _attackTime;

        [Tooltip("Ataque que realizará cuando el jugador se encuentre cerca.")] [SerializeField]
        private GameObject _closeAttack;

        [SerializeField] private List<GameObject> _attacks;
        private bool _attacking;

        private void Start()
        {
            DeactivateAttacks();
        }

        public void RandomAttack()
        {
            if (_attacking) return;
            var index = Random.Range(0, _attacks.Count);
            StartCoroutine(StartAttack(_attacks[index]));
        }

        public void CloseAttack()
        {
            if (_attacking) return;
            StartCoroutine(StartAttack(_closeAttack));
        }

        public void EndAttack()
        {
            _attacking = false;
        }

        private IEnumerator StartAttack(GameObject attack)
        {
            _attacking = true;
            yield return new WaitForSeconds(_attackTime);
            ActivateAttack(attack);
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