using System;
using InterOrbital.Item;
using InterOrbital.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace InterOrbital.Player
{
    public class PlayerInteraction : PlayerComponents
    {
        [SerializeField] private float _interactionRange;
        private BaseInteractable _currentInteractable;
        private UIManager _uiManager;
        private bool _isInteracting;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnInteractPerformed += Interact;
        }

        private void Start()
        {
            _uiManager = UIManager.Instance;
        }

        private void Update()
        {
            if (!_isInteracting)
            {
                CheckInteractables();
            }
        }

        private void CheckInteractables()
        {
            RaycastHit2D[] hitColliders =
                Physics2D.RaycastAll(transform.position, PlayerAim.AimDir(), _interactionRange);
            foreach (var hit in hitColliders)
            {
                if (hit.collider.TryGetComponent(out BaseInteractable raycastInteractable))
                {
                    ChangeInteractable(raycastInteractable);
                    return;
                }
            }

            if (_currentInteractable)
                ChangeInteractable(null);
        }

        private void ChangeInteractable(BaseInteractable interactable)
        {
            if (interactable)
            {
                if (_currentInteractable != null && _currentInteractable != interactable)
                    _currentInteractable.ShowInteraction(false);
                _currentInteractable = interactable;
                _currentInteractable.ShowInteraction(true);
            }
            else
            {
                _currentInteractable.ShowInteraction(false);
                _currentInteractable = null;
            }
        }
        
        public void Interact()
        {
            if (!CanInteract()) return;
            
            if (_currentInteractable.InteractioShowUI())
            {
                InputHandler.ChangeActionMap();
                if (_isInteracting)
                    _currentInteractable.EndInteraction();
                else
                    _currentInteractable.Interact();
                _isInteracting = !_isInteracting;
            }
            else
            {
                _currentInteractable.Interact();
            }
            
        }

        private bool CanInteract()
        {
            return !PlayerDash.IsDashing() && _currentInteractable && !_uiManager.animating;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }
    }
}