using Cinemachine;
using UnityEngine;

namespace InterOrbital.Player
{
    public class CameraShake : MonoBehaviour
    {
        private CinemachineImpulseSource _impulseSource;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private float _shakeTimer;
        private float _currentShakeDuration;
        private float _currentShakeAmplitude;
        private float _currentShakeIntensity;

        public static CameraShake Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }

            _impulseSource = GetComponent<CinemachineImpulseSource>();
            _multiChannelPerlin = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        public void Shake(float intensity = 1f)
        {
            _impulseSource.GenerateImpulse(intensity);
        }

        // OLD SHAKE
        // private void Update()
        // {
        //     if (_shakeTimer > 0)
        //     {
        //         _shakeTimer -= Time.deltaTime;
        //         _multiChannelPerlin.m_AmplitudeGain =
        //             Mathf.Lerp(_currentShakeAmplitude, 0f, 1 - (_shakeTimer / _currentShakeDuration));
        //         _multiChannelPerlin.m_FrequencyGain =
        //             Mathf.Lerp(_currentShakeIntensity, 0f, 1 - (_shakeTimer / _currentShakeDuration));
        //     }
        // }

        // public void Shake(float amplitude = 5f, float intensity = 1f, float duration = 1f)
        // {
        //     _multiChannelPerlin.m_AmplitudeGain = amplitude;
        //     _multiChannelPerlin.m_FrequencyGain = intensity;
        //     _currentShakeAmplitude = amplitude;
        //     _currentShakeIntensity = intensity;
        //     _shakeTimer = duration;
        //     _currentShakeDuration = duration;
        // }

        
    }
}