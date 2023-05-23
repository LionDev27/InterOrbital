using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUIController : MonoBehaviour
{
    [SerializeField] private List<Image> energyBars = new List<Image>();

    public void UpdateEnergy(int maxEnergy, int currentEnergy)
    {
        int energyPerBar = maxEnergy / energyBars.Count;
        int filledBars = currentEnergy / energyPerBar;

        for (int i = 0; i < energyBars.Count; i++)
        {
            Image energyBarImg = energyBars[i];

            // Verifica si la barra actual debe estar llena o vacía
            if (i < filledBars)
            {
                // La barra está llena
                SetBarFillAmount(energyBarImg, 1f);
            }
            else if (i == filledBars)
            {
                // La barra actual está parcialmente llena
                float fillAmount = (float)(currentEnergy % energyPerBar) / energyPerBar;
                SetBarFillAmount(energyBarImg, fillAmount);
            }
            else
            {
                // La barra está vacía
                SetBarFillAmount(energyBarImg, 0f);
            }
        }
    }

    private void SetBarFillAmount(Image barImage, float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }

}
