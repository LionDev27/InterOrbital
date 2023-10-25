using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace InterOrbital.Events
{
    public class TotalDarkEvent : EventBase
    {
        public override void StartEvent()
        {
            base.StartEvent();
            DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x, 0f, _lightTransitionTime).Play();
        }

        public override void EndEvent()
        {
            base.EndEvent();
            DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x, 1f, _lightTransitionTime).Play();
        }

    }

}
