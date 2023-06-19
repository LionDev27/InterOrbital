using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace InterOrbital.Player
{
    public class CameraShake : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private float _shakeTimer;
        private float _currentShakeDuration;
        private float _currentShakeIntensity;

        public static CameraShake Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }

            _multiChannelPerlin = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                _multiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(_currentShakeIntensity, 0f, 1 - (_shakeTimer / _currentShakeDuration));
            }
        }

        public void Shake(float intensity, float duration)
        {
            _multiChannelPerlin.m_AmplitudeGain = intensity;
            _currentShakeIntensity = intensity;
            _shakeTimer = duration;
            _currentShakeDuration = duration;
        }
    }
}