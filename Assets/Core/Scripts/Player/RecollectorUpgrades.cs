using System;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;

namespace InterOrbital.Player
{
    public class RecollectorUpgrades : MonoBehaviour
    {
        public static Action<int> OnUpgradeRecollector;
        public static Action OnEndUpgrade;

        [SerializeField] private List<RecollectorTier> _tiers;
        private List<RecollectorTier> _currentTiers = new();
        private int _currentTierIndex;

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
            _currentTiers = _tiers;
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
            RecollectorTier newTier = _tiers[tier];
            var components = PlayerComponents.Instance;
            var currentUsages = components.PlayerRecollector.Usages;
            //Si es el mismo upgrade, añadimos los usos.
            if (_currentTierIndex == tier)
                newTier.usages += currentUsages;
            //Si es un upgrade distinto al que ya tenemos.
            else
            {
                //Si tenemos usos aún, los guardamos e instanciamos un objeto..
                if (currentUsages > 0)
                {
                    var currentTier = _currentTiers[_currentTierIndex];
                    currentTier.usages = currentUsages;
                    _currentTiers[_currentTierIndex] = currentTier;
                    if (currentTier.upgradeData != null)
                        components.Inventory.DropItem(components.PlayerAttack.attackPoint.position,
                            components.transform.position, -1, currentTier.upgradeData);
                }
                else
                    _currentTiers[_currentTierIndex] = _tiers[_currentTierIndex];

                //Si el upgrade que vamos a hacer ya tenía usos, los asignamos.
                newTier.usages = _currentTiers[tier].usages;
            }
            SetTier(tier, newTier);
        }

        private void SetTier(int tier, RecollectorTier current)
        {
            _currentTierIndex = tier;
            PlayerComponents.Instance.PlayerRecollector.ChangeTier(current, _currentTierIndex);
        }
    }

    [Serializable]
    public struct RecollectorTier
    {
        public AnimatorOverrideController controller;
        public ItemCraftScriptableObject upgradeData;
        public AudioClip sfx;
        public int damage;
        public int usages;
    }
}