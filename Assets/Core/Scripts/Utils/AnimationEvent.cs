using UnityEngine;
using UnityEngine.Events;

namespace InterOrbital.Utils
{
    public class AnimationEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;

        public void PlayEvent()
        {
            _event?.Invoke();
        }
    }
}
