using System;
using UnityEngine;
using InterOrbital.Recollectables;

namespace InterOrbital.Player
{
    public class PlayerRecollector : PlayerComponents
    {
        [SerializeField] private float _recollectionRange = 5f;
        [SerializeField] private float _recollectionCooldownInSeconds = 1f;
        [SerializeField] private float _recollectionWidth = 3f;

        private LineRenderer _lineRenderer;
        private float _timer;

        protected override void Awake()
        {
            base.Awake();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _lineRenderer.startWidth = _recollectionWidth;
            _lineRenderer.endWidth = _recollectionWidth;
            _lineRenderer.enabled = false;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (CanRecollect())
                {
                    Recollect();
                    _timer = 0f;
                }
                ActivateLineRenderer();
            }
            else if (_lineRenderer.enabled)
            {
                _lineRenderer.enabled = false;
            }
        }

        private void ActivateLineRenderer()
        {
            // Configura la posición inicial en el jugador
            _lineRenderer.SetPosition(0, transform.position);

            // Obtiene la dirección de apuntado en función del input
            Vector2 aimDirection = PlayerAim.AimDir();

            // Calcula la posición final en la dirección de apuntado con el rango dado
            Vector2 endPoint = (Vector2)transform.position + aimDirection * _recollectionRange;

            // Configura la posición final en la dirección de apuntado
            _lineRenderer.SetPosition(1, endPoint);

            // Activa el Line Renderer
            _lineRenderer.enabled = true;
        }

        private void Recollect()
        {
            Vector2 dir = PlayerAim.AimDir();
            Vector2 boxcastSize = new Vector2(_recollectionRange, _recollectionWidth);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxcastSize, 0f, dir, _recollectionRange);
            
            if(hits.Length <= 0) return;
            
            foreach (var hit in hits)
            {
                Recollectable recollectable = hit.collider.GetComponent<Recollectable>();
                if (recollectable != null)
                {
                    recollectable.Recollect();
                }
            }
        }

        private bool CanRecollect()
        {
            return _timer >= _recollectionCooldownInSeconds;
        }
    }
}
