using UnityEngine;
using UnityEngine.Events;

namespace InterOrbital.Utils
{
    public class AnimationEvent : MonoBehaviour
    {
        [SerializeField] protected UnityEvent _event;

        public void PlayEvent()
        {
            _event?.Invoke();
        }
    }
}
