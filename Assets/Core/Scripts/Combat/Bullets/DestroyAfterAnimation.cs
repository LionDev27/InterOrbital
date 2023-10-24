using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (animator != null)
        {
            // Obtiene la duración de la animación
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0]; // Si solo tienes un AnimationClip
            float duration = clip.length;

            // Programa la destrucción del GameObject después de la duración de la animación
            Destroy(gameObject, duration);
        }
    }
}
