using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;

        private Image _image;
        private Camera _camera;

        public static CursorController Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(transform.parent);
            }
            else
                Destroy(transform.parent);
            
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _camera = Camera.main;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdatePosition();
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                StartCoroutine(ClickAnimation());
            }

            if (_camera == null)
                _camera = Camera.main;
        }
        
        private IEnumerator ClickAnimation()
        {
            for (int i = 1; i < _sprites.Length; i++)
            {
                _image.sprite = _sprites[i];
                yield return new WaitForSeconds(0.05f);
            }
            StartCoroutine(EndAnimation());
        }

        private IEnumerator EndAnimation()
        {
            for (int i = _sprites.Length - 2; i >= 0; i--)
            {
                _image.sprite = _sprites[i];
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void UpdatePosition()
        {
            transform.position = Mouse.current.position.value;
        }

        public void SetAlpha(bool transparent)
        {
            var tempColor = _image.color;
            tempColor.a = transparent ? 0.6f : 1f;
            _image.color = tempColor;
        }
    }
}
