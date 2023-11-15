using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace InterOrbital.Player
{
    public class RecollectorUpgrades : MonoBehaviour
    {
        public static Action<int> OnUpgradeRecollector;
        public static Action OnEndUpgrade;

        [SerializeField] private List<RecollectorTier> _tiers;
        private int _currentTier;

        private void OnEnable()
        {
            OnUpgradeRecollector += Upgrade;
            OnEndUpgrade += EndUpgrade;
        }

        private void OnDisable()
        {
            OnUpgradeRecollector -= Upgrade;
            OnEndUpgrade -= EndUpgrade;
        }

        private void Start()
        {
            EndUpgrade();
        }

        private void Upgrade(int tier)
        {
            if (tier <= _tiers.Count)
                ChangeTier(tier);
        }

        private void EndUpgrade()
        {
            ChangeTier(0);
        }

        private void ChangeTier(int tier)
        {
            _currentTier = tier;
            RecollectorTier current = _tiers[tier];
            PlayerComponents.Instance.PlayerRecollector.ChangeTier(current, _currentTier);
        }
    }

    [Serializable]
    public struct RecollectorTier
    {
        public AnimatorController controller;
        public int usages;
    }
}