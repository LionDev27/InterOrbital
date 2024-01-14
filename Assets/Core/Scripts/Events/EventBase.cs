using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace InterOrbital.Events
{
    public class EventBase : MonoBehaviour
    {  
        [SerializeField] private string _name;
        [SerializeField] private string _planetName;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _planetSprite;
        [SerializeField] private int _duration;
        [SerializeField] protected Light2D _globalLight;
        [SerializeField] protected float _lightTransitionTime;
        [SerializeField] private float _lightIntensity = 0.15f;

        public int Duration => _duration;
        public string EventName => _name;
        public string Description => _description;

        public string PlanetName => _planetName;

        public virtual void StartEvent()
        {
            DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x, _lightIntensity, _lightTransitionTime).Play();
        }
        public virtual void EndEvent()
        {
            DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x, 1f, _lightTransitionTime).Play();
        }

        public Sprite GetPlanetSprite()
        {
            return _planetSprite;
        }

    }

}
