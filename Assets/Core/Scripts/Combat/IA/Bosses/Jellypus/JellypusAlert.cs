using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusAlert : JellypusStateBase
    {
        [SerializeField] private float _losePlayerTime;
        private float _losePlayerTimer;
        
        public override void OnStateEnter()
        {
            Debug.Log("Alert");
            ResetTimer();
        }

        public override void Execute()
        {
            if (_currentAgent.IsDetectingPlayer())
            {
                //Ataques.
            }
            else
            {
                _losePlayerTimer -= Time.deltaTime;
                if (_losePlayerTimer <= 0f)
                    _currentAgent.ChangeState(_currentAgent.States[0]);
            }
        }

        private void ResetTimer()
        {
            _losePlayerTimer = _losePlayerTime;
        }
    }
}