using System;
using DG.Tweening;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneAim : MonoBehaviour
    {
        public Transform AttackPoint => _attackPoint;
        
        [SerializeField] private Transform _gunSpriteT;
        [SerializeField] private Transform _attackPoint;
        private CloneAgent _agent;
        private SpriteRenderer _gunSprite;
        private float _aimOffset;
        private float _gunSpriteOffset;

        private void Awake()
        {
            _agent = GetComponent<CloneAgent>();
            _gunSprite = _gunSpriteT.GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _aimOffset = _attackPoint.localPosition.x;
            _gunSpriteOffset = _gunSpriteT.transform.localPosition.x;
        }

        private void Update()
        {
            Aim();
        }
        
        private void Aim()
        {
            if (_agent.AimDir() == Vector2.zero) return;
            _attackPoint.localPosition = _agent.AimDir() * _aimOffset;
            HandleSprites();
        }
        
        private void HandleSprites()
        {
            //Rotacion de la pistola.
            _gunSpriteT.transform.localPosition = _agent.AimDir() * _gunSpriteOffset;
            var lookAtPos = _attackPoint.localPosition;
            if (_agent.AimDir().x > 0f)
            {
                _gunSprite.flipX = false;
                _gunSpriteT.right = lookAtPos - _gunSpriteT.localPosition;
            }
            else if (_agent.AimDir().x < 0f)
            {
                _gunSprite.flipX = true;
                _gunSpriteT.right = -lookAtPos - _gunSpriteT.localPosition;
            }
        }
        
        public void ShowGun(bool show)
        {
            if (show)
            {
                _gunSpriteT.gameObject.SetActive(true);
                _gunSpriteT.transform.DOScale(Vector2.one, 0.1f).Play();
            }
            else
                _gunSpriteT.transform.DOScale(Vector2.zero, 0.1f)
                    .OnComplete(() => _gunSpriteT.gameObject.SetActive(false)).Play();
        }
    }
}