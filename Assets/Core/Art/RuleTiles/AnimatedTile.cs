using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTile : MonoBehaviour
{
    public List<Sprite> sprites;
    public float animationTime;
    
    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        timer = animationTime;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0 ) 
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
            timer = animationTime;
        }
    }
}
