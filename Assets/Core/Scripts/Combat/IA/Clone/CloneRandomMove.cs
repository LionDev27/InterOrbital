using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneRandomMove : EnemyStateBase
    {
        [SerializeField] private float _minChangeDirTime;
        [SerializeField] private float _maxChangeDirTime;
        private CloneAgent _currentAgent;
        private float _currentTime;
        private float _timer;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as CloneAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool("PlayerRunning", true);
            ChangePos();
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (Vector2.Distance(transform.position, _currentAgent.Target.position) >= _currentAgent.NormalDistance)
                _currentAgent.ChangeState(1);
            if (_currentAgent.ArrivedDestination())
                ChangePos();
            if (TimerEnded())
            {
                ChangePos();
                return;
            }
            _timer -= Time.deltaTime;
        }

        private void ChangePos()
        {
            _currentAgent.NavMeshAgent.SetDestination(RandomPos());
            SetTimer();
        }
        
        private bool TimerEnded()
        {
            return _timer <= 0f;
        }
        
        private void SetTimer()
        {
            _currentTime = GetRandomTime();
            _timer = _currentTime;
        }
        
        private float GetRandomTime()
        {
            return Random.Range(_minChangeDirTime, _maxChangeDirTime);
        }

        private Vector2 RandomPos()
        {
            if (_currentAgent.Target == null) return transform.position;
            Vector2 targetPos = _currentAgent.Target.position;
            Vector2 currentPos = transform.position;
            
            var distanceToPlayer = Vector2.Distance(targetPos, currentPos);
            var positive = Random.value;
            distanceToPlayer = positive > 0.5f ? -distanceToPlayer : distanceToPlayer;
            
            var dir = (targetPos - currentPos).normalized;
            var perpendicular = new Vector2(-dir.y, dir.x).normalized;
            // Calcular la posición del objetivo (punto a la distancia deseada en la dirección perpendicular)
            var pos = targetPos + (distanceToPlayer * perpendicular);
            return pos;
        }
    }
}