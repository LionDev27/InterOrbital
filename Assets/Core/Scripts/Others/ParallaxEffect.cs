using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Others
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Material _material;

        private void Awake()
        {
            _material = TryGetComponent(out SpriteRenderer rend) ? rend.material : GetComponent<Image>().material;
        }

        private void Start()
        {
            ResetOffset();
        }

        private void Update()
        {
            _material.mainTextureOffset += (_speed * Time.unscaledDeltaTime * Vector2.right);
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
