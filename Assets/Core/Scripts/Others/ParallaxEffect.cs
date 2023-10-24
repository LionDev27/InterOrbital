using UnityEngine;

namespace InterOrbital.Others
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Start()
        {
            ResetOffset();
        }

        private void Update()
        {
            _material.mainTextureOffset += (_speed * Time.deltaTime * Vector2.right);
        }

        private void OnDisable()
        {
            ResetOffset();
        }

        [ContextMenu("Reset Offset")]
        public void ResetOffset()
        {
            _material.mainTextureOffset = Vector2.zero;
        }
    }
}
