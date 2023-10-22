using DG.Tweening;
using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.Recollectables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecollectableRestricted : Recollectable
{
    [SerializeField] private ItemCraftScriptableObject _objectToCheck;
    [SerializeField] private int _amountNeeded;
    [SerializeField] private SpriteRenderer _toast;
    [SerializeField] private SpriteRenderer _spriteItemNeeded;

    private float animationDuration = 1f;
    private bool _toastFadeOut;
    private float _fadeOutTimer;

    protected override void Awake()
    {
        _spriteItemNeeded.sprite = _objectToCheck.itemSprite;
        _toast.color = new Color(1, 1, 1, 0);
        _spriteItemNeeded.color = new Color(1, 1, 1, 0);
        base.Awake();
    }

    protected override void Update()
    {
        FadeOutToast();
        base.Update();
    }

    public override void Recollect()
    {
        if (PlayerHaveCheckObject(_amountNeeded))
        {
            SubstractItemDropped(_objectToCheck, _amountNeeded);
            base.Recollect();
        }
        else
        {
            FadeInToast();
        }
    }

    private bool PlayerHaveCheckObject(int amount)
    {
        int playerItemAmount = PlayerComponents.Instance.GetComponent<PlayerInventory>().GetTotalItemAmount(_objectToCheck);

        if(playerItemAmount >= amount)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }

    private void SubstractItemDropped(ItemCraftScriptableObject item, int amount)
    {
        PlayerComponents.Instance.GetComponent<PlayerInventory>().SubstractItems(item, amount);
    }

    private void FadeInToast()
    {
        _toast.DOFade(1.0f, animationDuration);
        _spriteItemNeeded.DOFade(1.0f, animationDuration);
        _toastFadeOut = true;
        _fadeOutTimer = 3f;
    }

    private void FadeOutToast() 
    {
        if (_toastFadeOut)
        {
            if(_fadeOutTimer <= 0)
            {
                _toast.DOFade(0f, animationDuration);
                _spriteItemNeeded.DOFade(0f, animationDuration);
                _toastFadeOut = false;
            }
            _fadeOutTimer -= Time.deltaTime;
        }
    }
}
