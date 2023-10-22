using DG.Tweening;
using InterOrbital.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Events
{
    public class EventsManager : MonoBehaviour
    {
        [SerializeField] private List<EventBase> _eventsPool;
        private int _actualIndex;
        [SerializeField] private int _timeBetweenEvents;
        [SerializeField] private Transform _endRotation;
        [SerializeField] private Transform _startRotation;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private Image _planetImage;
        [SerializeField] private TextMeshProUGUI _timeText;
        public static EventsManager Instance = null;
        [HideInInspector] public int currentTime;
        



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
                _rotationPoint.rotation = _startRotation.rotation;
                _actualIndex++;
            }
          
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
   
