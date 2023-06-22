using InterOrbital.Item;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerInteraction : PlayerComponents
    {
        [SerializeField] private float _interactionRange;
        [SerializeField] private float _interactionColdown;
        private BaseInteractable _currentInteractable;
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
            CheckInteractables();
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

            if (_currentInteractable && !_isInteracting)
                ChangeInteractable(null);
        }

        private void ChangeInteractable(BaseInteractable interactable)
        {
            if (interactable)
            {
                _currentInteractable = interactable;
                _currentInteractable.ShowInteraction(true);
            }
            else
            {
                _currentInteractable.ShowInteraction(false);
                _currentInteractable = null;
            }
        }
        
        private void Interact()
        {
            if (_timer >= 0 || PlayerDash.IsDashing() || !_currentInteractable) return;
            
            _timer = _interactionColdown;
            InputHandler.ChangeActionMap();
            if (_isInteracting)
                _currentInteractable.EndInteraction();
            else
                _currentInteractable.Interact();
            _isInteracting = !_isInteracting;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }
    }
}