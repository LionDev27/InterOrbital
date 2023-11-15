using System;
using InterOrbital.Combat.IA;
using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InterOrbital.Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        [HideInInspector] public bool canSpawn;
        [SerializeField] protected Transform player; // Referencia al transform del jugador
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected float _distanceBetween = 3f;
        [SerializeField] protected float _spawnRadius = 15f;
        [SerializeField] protected float _visibleDistance = 15f;
        [SerializeField] protected int _maxEntitiesSpawn;
        protected int currentEntitiesSpawned = 0;


        protected bool _playerInRadius;
        protected float _playerNearSpawnTimer = -1;
        protected float _spawnTimer;
        

        protected void Start()
        {
            player = PlayerComponents.Instance.transform;
            canSpawn = true;
        }

        protected void Update()
        {
            SpawnEntities();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInRadius = true;
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInRadius = false;
                if (_playerNearSpawnTimer < 0)
                {
                    _playerNearSpawnTimer = 20f;
                }
            }
        }

        protected bool CanSpawn()
        {
            return canSpawn && _playerNearSpawnTimer < 0 && NotVisibleInCamera();
        }

        protected bool NotVisibleInCamera()
        {
            return (Vector3.Distance(transform.position, player.position) >= _spawnRadius + _visibleDistance) &&
                   _playerInRadius;
        }

        protected void SpawnEntities()
        {
            if (CanSpawn())
            {
                if (currentEntitiesSpawned < _maxEntitiesSpawn)
                    SpawnAllEntities();
                else
                    canSpawn = false;
            }

            if (_playerNearSpawnTimer >= 0)
            {
                _playerNearSpawnTimer -= Time.deltaTime;
            }
        }

        protected abstract void SpawnAllEntities();

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
    }
}