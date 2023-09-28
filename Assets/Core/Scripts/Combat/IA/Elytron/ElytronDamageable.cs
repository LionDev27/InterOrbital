using DG.Tweening;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytronDamageable : EnemyDamageable
    {
        [SerializeField] private GameObject _elytrinPrefab;
        [Range(2, 5)]
        [SerializeField] private int _elytrinCount;
        
        protected override void AfterDeath()
        {
            for (int i = 0; i < _elytrinCount; i++)
            {
                Vector2 elytrinOriginPos = transform.position;
                GameObject tempElytrin = Instantiate(_elytrinPrefab, elytrinOriginPos, Quaternion.identity);
                tempElytrin.transform.DOMove(RandomDropDir() * 2f + elytrinOriginPos, 0.5f).SetEase(Ease.OutQuint).Play();
            }
        }
    }
}
