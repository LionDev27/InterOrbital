using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class SlowTimeEffect : MonoBehaviour
    {
        public static SlowTimeEffect Instance;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        public IEnumerator Play(float duration)
        {
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }
    }
}