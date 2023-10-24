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
            // Obtiene la duraci�n de la animaci�n
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0]; // Si solo tienes un AnimationClip
            float duration = clip.length;

            // Programa la destrucci�n del GameObject despu�s de la duraci�n de la animaci�n
            Destroy(gameObject, duration);
        }
    }
}
