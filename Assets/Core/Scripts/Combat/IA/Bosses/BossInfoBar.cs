using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Combat.IA
{
    public class BossInfoBar : MonoBehaviour
    {
        public static Action<string, int, int> OnActivateBoss;
        public static Action OnDeactivateBoss;
        public static Action<int> OnUpdateLifeBar;

        [SerializeField] private GameObject _bossCanvas;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _lifeBarFill;
        [SerializeField] private TextMeshProUGUI _lifeText;
        private int _maxHealth;

        private void Start()
        {
            _bossCanvas.SetActive(false);
        }

        private void OnEnable()
        {
            OnActivateBoss += ActivateBoss;
            OnDeactivateBoss += DeactivateBoss;
            OnUpdateLifeBar += UpdateLifeBar;
        }

        private void OnDisable()
        {
            OnActivateBoss -= ActivateBoss;
            OnDeactivateBoss -= DeactivateBoss;
            OnUpdateLifeBar -= UpdateLifeBar;
        }

        private void ActivateBoss(string bossName, int currentHealth ,int maxHealth)
        {
            _name.SetText(bossName);
            _lifeText.SetText(currentHealth.ToString());
            _maxHealth = maxHealth;
            UpdateLifeBar(currentHealth);
            _bossCanvas.SetActive(true);
        }

        private void UpdateLifeBar(int currentHealth)
        {
            _lifeBarFill.fillAmount = (float)currentHealth / _maxHealth;
            _lifeText.text = currentHealth.ToString();
        }

        private void DeactivateBoss()
        {
            _bossCanvas.SetActive(false);
        }
    }
}