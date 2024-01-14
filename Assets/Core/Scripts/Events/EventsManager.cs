using DG.Tweening;
using InterOrbital.UI;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.WorldSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using InterOrbital.Utils;

namespace InterOrbital.Events
{
    public class EventsManager : MonoBehaviour
    {
        [SerializeField] private List<EventBase> _eventsPool;
        private int _actualIndex;
        private bool _isWarning = false;

        //Evento parte de la izquierda
        [SerializeField] private int _timeBetweenEvents;
        [SerializeField] private Transform _endRotation;
        [SerializeField] private Transform _startRotation;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private Image _planetImage;
        [SerializeField] private TextMeshProUGUI _timeText;
        public static EventsManager Instance = null;
        [HideInInspector] public int currentTime;


        //Avisador de evento
        [SerializeField] private Image _warnPlanetImage;
        [SerializeField] private TextMeshProUGUI _planetName;
        [SerializeField] private TextMeshProUGUI _eventName;
        [SerializeField] private TextMeshProUGUI _eventDescription;
        [SerializeField] private Image _signalImage;
        [SerializeField] private Light2D _globalLight;
        [SerializeField] private Light2D _playerLight;

        private float _timeToAlert = 15f;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            _actualIndex = _eventsPool.Count;
            _rotationPoint.rotation = _startRotation.rotation;
            StartCoroutine(StartOrbit());
        }

        private IEnumerator StartOrbit()
        {
            while(true)
            {
                if (_actualIndex == _eventsPool.Count)
                {
                    DisorderList();
                    _actualIndex = 0;
                }

                UIManager.Instance.ToggleClockTime(true);
                //Contador para el siguiente evento
                for (currentTime = _timeBetweenEvents; currentTime >= 0; currentTime--)
                {
                    UpdateTimeText(currentTime);
                    if(currentTime <= _timeToAlert && !_isWarning)
                    {
                        _isWarning = true;
                        RedLightWarn();
                    }
                    yield return new WaitForSeconds(1f);
                }

                _planetImage.sprite = _eventsPool[_actualIndex].GetPlanetSprite();
                _eventsPool[_actualIndex].StartEvent();
                UIManager.Instance.ToggleClockTime(false);
                _rotationPoint.DORotate(new Vector3(0, 0, _endRotation.localEulerAngles.z), _eventsPool[_actualIndex].Duration).SetEase(Ease.Linear);


                //Contador del evento.
                for (currentTime= _eventsPool[_actualIndex].Duration; currentTime >= 0; currentTime--)
                {
                    yield return new WaitForSeconds(1f);
                }

                _eventsPool[_actualIndex].EndEvent();
                _isWarning = false;
                _rotationPoint.rotation = _startRotation.rotation;
                _actualIndex++;
                GridLogic.Instance.RespawnSpawners();
            }
        }

        private void ModifyLightRadius(bool isWarn)
        {
            if (isWarn)
            {
                _playerLight.pointLightOuterRadius = 30;
            }
            else
            {
                _playerLight.pointLightOuterRadius = 6;
            }
        }

        private void RedLightWarn()
        {
            ModifyLightRadius(true);

            AudioManager.Instance.PlaySFX("EventAlert");
            AudioManager.Instance.ModifyMusicVolume(-20);
            AudioManager.Instance.ModifySFXVolume(10);
            DOTween.To(() => _globalLight.color, x => _globalLight.color = x, Color.red, 0.8f).
                From(Color.white).
                SetEase(Ease.Linear).
                SetLoops(8, LoopType.Yoyo).
                OnStepComplete( () => AudioManager.Instance.PlaySFX("EventAlert")).
                Play().OnComplete(() => {
                    StartCoroutine(WarnEvent());
                    AudioManager.Instance.ModifyMusicVolume(20);
                    AudioManager.Instance.ModifySFXVolume(-10);
                });
        }

        private IEnumerator WarnEvent()
        {
            ModifyLightRadius(false);
            AudioManager.Instance.PlaySFX("EventWarn");
            _warnPlanetImage.sprite = _eventsPool[_actualIndex].GetPlanetSprite();
            _planetName.text = _eventsPool[_actualIndex].PlanetName;
            _eventDescription.text = _eventsPool[_actualIndex].Description;
            _eventName.text = _eventsPool[_actualIndex].EventName;

            UIManager.Instance.WarnPanelShowOrHide(true);
            Tween triangleWarn = _signalImage.DOFade(0, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            yield return new WaitForSeconds(2f);
            _eventName.DOFade(1, 1f).SetEase(Ease.Linear).Play();
            yield return new WaitForSeconds(4f);
            _eventName.DOFade(0, 1f).SetEase(Ease.Linear).Play();
            yield return new WaitForSeconds(1.5f);
            _eventDescription.DOFade(1, 1f).SetEase(Ease.Linear).Play();
            yield return new WaitForSeconds(8f);
            _eventDescription.DOFade(0, 1f).SetEase(Ease.Linear).Play().OnComplete(() => { 
                triangleWarn.Kill();
                _signalImage.ChangueAlphaColor(1);
            });
            UIManager.Instance.WarnPanelShowOrHide(false);
            AudioManager.Instance.PlaySFX("EventWarn");
        }

        private void DisorderList()
        {
            _eventsPool.Sort((a, b) => Random.Range(-1, 2));
        }

        private void UpdateTimeText(int time)
        {
            int minutos = time / 60;
            int segundosRestantes = time % 60;
            _timeText.text = string.Format("{0:D2}:{1:D2}", minutos, segundosRestantes);
        }


    }
}
   
