using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarsUIController : MonoBehaviour
{
    [SerializeField] private List<Image> energyBars = new List<Image>();

    private float _blinkSpeed = 2f; // Controla la velocidad del parpadeo
    private Color _blinkColor = Color.yellow; // Color de parpadeo
    private Coroutine _blinkCoroutine; // Referencia a la rutina de parpadeo actual


    public void UpdateEnergy(int maxEnergy, int currentEnergy)
    {
        int energyPerBar = maxEnergy / energyBars.Count;
        int filledBars = currentEnergy / energyPerBar;

        for (int i = 0; i < energyBars.Count; i++)
        {
            Image energyBarImg = energyBars[i];

            // Verifica si la barra actual debe estar llena o vac�a
            if (i < filledBars)
            {
                // La barra est� llena
                SetBarFillAmount(energyBarImg, 1f);
            }
            else if (i == filledBars)
            {
                // La barra actual est� parcialmente llena
                float fillAmount = (float)(currentEnergy % energyPerBar) / energyPerBar;
                SetBarFillAmount(energyBarImg, fillAmount);
            }
            else
            {
                // La barra est� vac�a
                SetBarFillAmount(energyBarImg, 0f);
            }
        }
    }

    public void StartBlink()
    {
        if (_blinkCoroutine == null)
        {
            _blinkCoroutine = StartCoroutine(Blink());
        }
    }

    public void StopBlink()
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
            GetComponent<Image>().color = Color.white;
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            GetComponent<Image>().color = _blinkColor;
            yield return new WaitForSeconds(1 / _blinkSpeed);
            GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(1 / _blinkSpeed);
        }
    }

    private void SetBarFillAmount(Image barImage, float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }

}
