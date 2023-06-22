using UnityEngine;

namespace InterOrbital.Player
{
    public class RecollectorTransitionController : MonoBehaviour
    {
        private PlayerRecollector _playerRecollector;

        private void Awake()
        {
            _playerRecollector = GetComponentInParent<PlayerRecollector>();
        }

        public void EndTransition(int value)
        {
            _playerRecollector.SetTransitionStatus(value > 0);
        }
    }
}