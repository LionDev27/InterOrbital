using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using AnimationEvent = InterOrbital.Utils.AnimationEvent;

namespace InterOrbital.Combat.IA
{
    public class WolfAnimationEvent : AnimationEvent
    {
        [SerializeField] protected UnityEvent _secondEvent;

        public void PlaySecondEvent()
        {
            _secondEvent?.Invoke();
        }
    }
}