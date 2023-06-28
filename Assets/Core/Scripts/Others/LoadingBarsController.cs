using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InterOrbital.Others
{
    public class LoadingBarsController : MonoBehaviour
    {
        [SerializeField] private List<Image> fillBars;

        public void UpdateBarFills(float maxValue, float currentValue)
        {
            float energyPerBar = maxValue / fillBars.Count;
            float filledBars = currentValue / energyPerBar;

            for (int i = 0; i < fillBars.Count; i++)
            {
                Image energyBarImg = fillBars[i];

                // Verifica si la barra actual debe estar llena o vac�a
                if (i < filledBars)
                {
                    // La barra est� llena
                    SetBarFillAmount(energyBarImg, 1f);
                }
                else if (i == filledBars)
                {
                    // La barra actual est� parcialmente llena
                    float fillAmount = (float)(currentValue % energyPerBar) / energyPerBar;
                    SetBarFillAmount(energyBarImg, fillAmount);
                }
                else
                {
                    // La barra est� vac�a
                    SetBarFillAmount(energyBarImg, 0f);
                }
            }
        }

        private void SetBarFillAmount(Image barImage, float fillAmount)
        {
            barImage.fillAmount = fillAmount;
        }
    }
}