using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Player;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerInteraction : PlayerComponents
    {
        [SerializeField] private float _interactionRange;
        [SerializeField] private float _interactionColdown;
        private bool _isInteracting;
        private float _timer;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnInteractPerformed += Interact;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        private void Interact()
        {
            if (_timer >= 0)
            {
                return;
            }

            if (PlayerDash.IsDashing()) return;
            RaycastHit2D[] hitColliders =
                Physics2D.RaycastAll(transform.position, PlayerAim.AimDir(), _interactionRange);
            foreach (var hit in hitColliders)
            {
                if (hit.collider.TryGetComponent(out IInteractable raycastInteractable))
                {
                    Interact(raycastInteractable);
                    return;
                }
            }

            Debug.Log("InteractingAll");
            var colliders = Physics2D.OverlapCircleAll(transform.position, _interactionRange);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    Interact(interactable);
                    return;
                }
            }
        }

        private void Interact(IInteractable interactable)
        {
            _timer = _interactionColdown;
            InputHandler.ChangeActionMap();
            if (_isInteracting)
                interactable.EndInteraction();
            else
                interactable.Interact();
            Debug.Log($"Selected object: {interactable}");
            _isInteracting = !_isInteracting;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }
    }
}