using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class TypewriterEffects : MonoBehaviour
    {
        [Header("Pitch Effect")]
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
        private AbstractTypewriterEffect _typewriter;

        private void Awake()
        {
            _typewriter = GetComponent<AbstractTypewriterEffect>();
        }

        public void OnCharacterType()
        {
            RandomPitch();
        }

        private void RandomPitch()
        {
            _typewriter.audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        }
    }
}