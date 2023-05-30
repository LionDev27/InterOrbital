using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Player;
using InterOrbital.Utils;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerInteraction : PlayerComponents
    {
        [SerializeField] private float _interactionRange;
        private bool _isInteracting;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnInteractPerformed += Interact;
        }

       

        private void Interact()
        {
            if(UIManager.Instance.IsAnimating)
            {
                return;
            }
            if (PlayerDash.IsDashing()) return;
            var colliders = Physics2D.OverlapCircleAll(transform.position, _interactionRange);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                   InputHandler.ChangeActionMap();
                   if (_isInteracting)
                        interactable.EndInteraction();
                    else
                        interactable.Interact();
                    _isInteracting = !_isInteracting;
                    return;
                }
            }
        }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }
}

}