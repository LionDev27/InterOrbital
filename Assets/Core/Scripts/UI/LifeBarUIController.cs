using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUIController : MonoBehaviour
{
    [SerializeField] private List<Image> lifeCircles = new List<Image>();

    public void UpdateLife(int maxLife, int currentLife)
    {
        int lifePerCircle = maxLife / lifeCircles.Count;
        int filledBars = currentLife / lifePerCircle;

        for (int i = 0; i < lifeCircles.Count; i++)
        {
            Image lifeBarImg = lifeCircles[i];

            // Verifica si la barra actual debe estar llena o vacía
            if (i < filledBars)
            {
                // La barra está llena
                SetBarFillAmount(lifeBarImg, 1f);
            }
            else if (i == filledBars)
            {
                // La barra actual está parcialmente llena
                float fillAmount = (float)(currentLife % lifePerCircle) / lifePerCircle;
                SetBarFillAmount(lifeBarImg, fillAmount);
            }
            else
            {
                // La barra está vacía
                SetBarFillAmount(lifeBarImg, 0f);
            }
        }
    }

    private void SetBarFillAmount(Image barImage, float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }
}
