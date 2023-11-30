using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Utils
{
    public class HologramShaderController : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _image.material.SetFloat("_UnscaledTime", 0);
        }

        private void Update()
        {
            _image.material.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}
