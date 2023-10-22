using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class AttackSpriteOrderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _referenceRenderer;
        private SpriteRenderer _ownRenderer;
        private int _referenceRendererOrder;

        private void Awake()
        {
            _ownRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _referenceRendererOrder = _referenceRenderer.sortingOrder;
        }

        private void Update()
        {
            _ownRenderer.sortingOrder = transform.position.y > _referenceRenderer.transform.position.y
                ? _referenceRendererOrder - 1
                : _referenceRendererOrder + 1;
        }
    }
}
