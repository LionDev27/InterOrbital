using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void FlipX(int value)
    {
        bool flip = value != 0;
        _spriteRenderer.flipX = flip;
    }
}
