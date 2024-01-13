using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private GameObject intro1;
    [SerializeField] private GameObject intro2;
    [SerializeField] private GameObject intro3;
    [SerializeField] private GameObject intro4;
    [SerializeField] private GameObject intro5;
    [SerializeField] private GameObject intro6;

    private void Start()
    {
        PrepareStart();
        StartCoroutine(FirstSequence());
    }

    private void PrepareStart()
    {
        intro1.GetComponent<CanvasGroup>().alpha = 0f;
        intro2.GetComponent<Image>().fillAmount = 0f;
        intro3.GetComponent<Image>().fillAmount = 0f;
        intro4.GetComponent<CanvasGroup>().alpha = 0f;
        intro5.GetComponent<Image>().fillAmount = 0f;
        intro6.GetComponent<CanvasGroup>().alpha = 0f;
        AudioManager.Instance.ModifyMusicVolume(-20f);
    }

    public IEnumerator FirstSequence()
    {
        float shakeDuration = 6f;
        float shakeStrength = 15f;
        RectTransform rectTransform = intro1.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = intro1.GetComponent<CanvasGroup>();
        Vector3 originalPosition = rectTransform.localPosition;

        Sequence firstSequence = DOTween.Sequence();
        firstSequence.AppendInterval(2f);
        firstSequence.Append(rectTransform.DOShakePosition(shakeDuration, shakeStrength));
        firstSequence.Join(canvasGroup.DOFade(1f, 4f)); 
        firstSequence.OnComplete(() => {
            rectTransform.localPosition = originalPosition;
            StartCoroutine(SecondSequence());
        });
        firstSequence.Play();
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySFX("Intro1");
        yield return null;
    }

    public IEnumerator SecondSequence()
    {
        float fillDuration = 4f;
        Image img2 = intro2.GetComponent<Image>();

        Sequence secondSequence = DOTween.Sequence();
        secondSequence.Append(img2.DOFillAmount(1.0f, fillDuration));
        secondSequence.AppendInterval(0.5f);
        secondSequence.OnComplete(() => StartCoroutine(ThirdSequence()));
        secondSequence.Play();
        yield return null;
    }

    public IEnumerator ThirdSequence()
    {
        float fillDuration = 4f;
        //float shakeDuration = 5f;
        //float shakeStrength = 15f;
        RectTransform rectTransform = intro3.GetComponent<RectTransform>();
        Image img3 = intro3.GetComponent<Image>();
        Vector3 originalPosition = rectTransform.localPosition;

        Sequence thirdSequence = DOTween.Sequence();
        //thirdSequence.Append(rectTransform.DOShakePosition(shakeDuration, shakeStrength));
        thirdSequence.Append(img3.DOFillAmount(1.0f, fillDuration));
        thirdSequence.AppendInterval(0.5f);
        thirdSequence.OnComplete(() => {
            rectTransform.localPosition = originalPosition;
            StartCoroutine(FourthSequence());
        }); 
        thirdSequence.Play();
        AudioManager.Instance.ModifySFXVolume(20f);
        AudioManager.Instance.PlaySFX("Intro3");
        yield return null;
    }

    public IEnumerator FourthSequence()
    {
        float shakeDuration = 7f;
        float shakeStrength = 30f;
        RectTransform rectTransform = intro4.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = intro4.GetComponent<CanvasGroup>();
        Vector3 originalPosition = rectTransform.localPosition;

        Sequence fourthSequence = DOTween.Sequence();
        fourthSequence.Append(rectTransform.DOShakePosition(shakeDuration, shakeStrength));
        fourthSequence.Join(canvasGroup.DOFade(1f, 3f));
        fourthSequence.OnComplete(() => {
            rectTransform.localPosition = originalPosition;
            StartCoroutine(FifthSequence());
        });
        fourthSequence.Play();
        AudioManager.Instance.ModifySFXVolume(0f);
        AudioManager.Instance.PlaySFX("Intro4");
        yield return null;
    }

    public IEnumerator FifthSequence()
    {
        float fillDuration = 5f;
        //float shakeDuration = 5f;
        //float shakeStrength = 10f;
        RectTransform rectTransform = intro5.GetComponent<RectTransform>();
        Vector3 originalPosition = rectTransform.localPosition;
        Image img5 = intro5.GetComponent<Image>();

        Sequence fifthSequence = DOTween.Sequence();
        //fifthSequence.Append(rectTransform.DOShakePosition(shakeDuration, shakeStrength));
        fifthSequence.Append(img5.DOFillAmount(1.0f, fillDuration));
        fifthSequence.AppendInterval(0.5f);
        fifthSequence.OnComplete(() => {
            rectTransform.localPosition = originalPosition;
            StartCoroutine(SixthSequence());
        }); 
        fifthSequence.Play();
        AudioManager.Instance.PlaySFX("Intro5");
        yield return null;
    }

    public IEnumerator SixthSequence()
    {
        float shakeDuration = 4f;
        float shakeStrength = 10f;
        RectTransform rectTransform = intro6.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = intro6.GetComponent<CanvasGroup>();
        Vector3 originalPosition = rectTransform.localPosition;

        Sequence sixthSequence = DOTween.Sequence();
        sixthSequence.Append(rectTransform.DOShakePosition(shakeDuration, shakeStrength));
        sixthSequence.Join(canvasGroup.DOFade(1f, 3f));
        sixthSequence.OnComplete(() => {
            rectTransform.localPosition = originalPosition;
            AudioManager.Instance.ModifyMusicVolume(0f);
        });
        sixthSequence.Play();
        AudioManager.Instance.PlaySFX("Intro6");
        yield return null;
    }
}
