using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Combat.IA;


namespace InterOrbital.Events
{
    public class DarkBallEvent : EventBase
    {
        [SerializeField] private float _raidusSpawn;
        [SerializeField] private GameObject _darkBallPrefab;
        [SerializeField] private float _timeToSpawn;
        [SerializeField] private int _numMaxEnemies = 0;
        private Transform _targetToAttack;
        private int _numEnemies=0;
        private bool _eventIsActive = false;

        private IEnumerator SpawnDarkBalls()
        {
            _numEnemies = 0;
            while (_eventIsActive)
            {
                yield return new WaitForSeconds(_timeToSpawn);

                if (_numEnemies < _numMaxEnemies && _eventIsActive && EventsManager.Instance.currentTime >= 5 )
                {

                    Vector3 spawnPosition;
                    do
                    {
                        Vector2 centerPosition = _targetToAttack.transform.position;
                        float randomAngle = Random.Range(0f, 2 * Mathf.PI);
                        float x = centerPosition.x + _raidusSpawn * Mathf.Cos(randomAngle);
                        float y = centerPosition.y + _raidusSpawn * Mathf.Sin(randomAngle);
                        spawnPosition = new Vector3(x, y, 0);
                    } while (!(spawnPosition.x >= 0 && spawnPosition.x < GridLogic.Instance.width && spawnPosition.y >= 0 && spawnPosition.y < GridLogic.Instance.height));

                    DarkBall _darkBall = Instantiate(_darkBallPrefab, spawnPosition, Quaternion.identity).GetComponent<DarkBall>();
                    _darkBall.SetEventSpawner(this);
                    _numEnemies++;
                }
            }
           
           
           
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                EndEvent();
            }
        }
        private void DestroyAllEnemies()
        {
            DarkBall[] darkBalls = FindObjectsOfType<DarkBall>();

            foreach (DarkBall dk in darkBalls)
            {
                dk.DeathDarkBall();
            }
        }

        public override void StartEvent()
        {
            base.StartEvent();
            _eventIsActive = true;
            _targetToAttack = FindObjectOfType<InterOrbital.Player.PlayerAim>().transform;
            StartCoroutine(SpawnDarkBalls());
        }

        public override void EndEvent()
        {
            base.EndEvent();
            _eventIsActive = false;
            StopCoroutine(SpawnDarkBalls());
            DestroyAllEnemies();

        }

        public void RestEnemy()
        {
            _numEnemies--;
            if (_numEnemies < 0)
                _numEnemies = 0;

        }


        private void OnDrawGizmos()
        {
            if (_targetToAttack)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(_targetToAttack.position, _raidusSpawn);
            }
        }
    }
}


