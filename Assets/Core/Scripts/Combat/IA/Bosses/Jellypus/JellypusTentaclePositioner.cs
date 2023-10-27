using System.Collections;
using System.Collections.Generic;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusTentaclePositioner : TargetPositioner
    {
        public int TentaclesCount => _tentacles.Count;
        public float waitPerTentacle;
        [HideInInspector]
        public AnimationClip anim;
        
        [SerializeField] private List<Transform> _tentacles;

        private void OnEnable()
        {
            HideTentacles();
            StartCoroutine(MoveTentacles());
        }

        private IEnumerator MoveTentacles()
        {
            if (target == null || anim == null) yield break;
            foreach (var tentacle in _tentacles)
            {
                tentacle.transform.position = target.position;
                tentacle.gameObject.SetActive(true);
                AudioManager.Instance.PlaySFX("BossTentacleAttack");
                yield return new WaitForSeconds(waitPerTentacle);
            }
        }

        public void HideTentacles()
        {
            foreach (var tentacle in _tentacles)
                tentacle.gameObject.SetActive(false);
        }
    }
}