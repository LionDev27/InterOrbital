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

            // Verifica si la barra actual debe estar llena o vac�a
            if (i < filledBars)
            {
                // La barra est� llena
                SetBarFillAmount(lifeBarImg, 1f);
            }
            else if (i == filledBars)
            {
                // La barra actual est� parcialmente llena
                float fillAmount = (float)(currentLife % lifePerCircle) / lifePerCircle;
                SetBarFillAmount(lifeBarImg, fillAmount);
            }
            else
            {
                // La barra est� vac�a
                SetBarFillAmount(lifeBarImg, 0f);
            }
        }
    }

    private void SetBarFillAmount(Image barImage, float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }
}
