using UnityEngine;

namespace InterOrbital.Utils
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        public void PlaySfx()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(_clip);
        }
    }
}