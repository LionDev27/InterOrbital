using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerController : MonoBehaviour
{

    [SerializeField] private bool isStatic;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        _spriteRenderer.sortingOrder = (int) transform.position.y;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStatic)
        {
            _spriteRenderer.sortingOrder = (int)transform.position.y;
        }
    }
}
