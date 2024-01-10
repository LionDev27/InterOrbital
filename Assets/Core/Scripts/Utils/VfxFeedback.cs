using UnityEngine;

namespace InterOrbital.Utils
{
    [RequireComponent(typeof(Animator))]
    public class VfxFeedback : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Init(AnimatorOverrideController controller)
        {
            _animator.runtimeAnimatorController = controller;
        }

        public void End()
        {
            Destroy(gameObject);
        }
    }
}