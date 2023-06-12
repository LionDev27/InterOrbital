using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;

[RequireComponent(typeof(Collider2D))]
public class SpriteMaskController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _playerSpriteRenderer;

    [SerializeField]
    private SpriteMask _spriteMask;

    private Collider2D _spriteMaskCollider;

    private List<SpriteRenderer> _otherRenderes;

    public bool checking = false;

    private void Awake()
    {
        _otherRenderes = new List<SpriteRenderer>();
        _spriteMaskCollider = GetComponent<Collider2D>();
        _spriteMaskCollider.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checking)
        {
            foreach(SpriteRenderer renderer in _otherRenderes)
            {
                if(_playerSpriteRenderer.sortingLayerName == renderer.sortingLayerName
                    && _playerSpriteRenderer.sortingOrder <= renderer.sortingOrder
                    && _playerSpriteRenderer.transform.position.y > renderer.transform.position.y)
                {
                    //_spriteMask.enabled = true;
                    //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    renderer.ChangueSpriteAlphaColor(0.4f);
                    return;
                }
                else
                {
                    renderer.ChangueSpriteAlphaColor(1f);
                    //_spriteMask.enabled = false;
                    //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            _otherRenderes.Add(spriteRenderer);
            checking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            checking = false;
            spriteRenderer.ChangueSpriteAlphaColor(1f);
            //_spriteMask.enabled = false;
            //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None; 
        }
    }
}
