using System.Collections;
using System.Collections.Generic;
using InterOrbital.WorldSystem;
using UnityEngine;

public class OrderInLayerController : MonoBehaviour
{
    [SerializeField] private bool isStatic;
    [SerializeField] private int _offset;
    
    private SpriteRenderer _spriteRenderer;
    private bool _canChange;
    private int _maxY;
    private int _lastOrder;
    private float _timer;
    private float _timerMax = .1f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        _canChange = true;
        //Multiplicamos por 2 para no llegar nunca al order 0, por si acaso.
        _maxY = GridLogic.Instance.height * 2;
        ChangeSortingOrder();
    }

    void LateUpdate()
    {
        if (_timer <= 0f)
        {
            if (CurrentOrder() != _lastOrder && _canChange)
                ChangeSortingOrder();
        }
        else
            _timer -= Time.deltaTime;
    }

    private void ChangeSortingOrder()
    {
        _spriteRenderer.sortingOrder = CurrentOrder();
        _lastOrder = _spriteRenderer.sortingOrder;
        _timer = _timerMax;
    }

    private int CurrentOrder()
    {
        return _maxY - (int)transform.position.y - _offset;
    }

    public void SetCanChange(bool v)
    {
        _canChange = v;
    }
}
