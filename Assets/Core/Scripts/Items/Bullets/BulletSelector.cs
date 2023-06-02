using InterOrbital.Item;
using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelector : MonoBehaviour
{
    [SerializeField] private List<BulletSlot> bulletsSlots;
    [SerializeField] private List<ItemBulletScriptableObject> bulletsItems;

    [SerializeField] private Sprite bulletSlotSelectedImage;
    [SerializeField] private Sprite bulletSlotNoSelectedImage;
    [SerializeField] private Sprite emptySlotImage;

    public static BulletSelector Instance;

    private int _selectedBulletIndex = 0;

    private void Awake()
    {
        if(!Instance) 
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FillBulletSelector();    
    }

    private void FillBulletSelector()
    {
        for(int i = 0; i < bulletsSlots.Count;  i++)
        {
            if (i < bulletsItems.Count)
            {
                bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
            }
            else
            {
                bulletsSlots[i].SetBulletSprite(emptySlotImage);
            }
            bulletsSlots[i].SetBackgroundSprite(bulletSlotNoSelectedImage);
        }
        bulletsSlots[_selectedBulletIndex].SetBackgroundSprite(bulletSlotSelectedImage);
    }

    public void UpdateSelectedBullet(int index)
    {
        var lastSelectedBulletIndex = _selectedBulletIndex;
        _selectedBulletIndex = index;

        bulletsSlots[lastSelectedBulletIndex].SetBackgroundSprite(bulletSlotNoSelectedImage);
        bulletsSlots[_selectedBulletIndex].SetBackgroundSprite(bulletSlotSelectedImage);
        ChangePlayerBullet();
    }

    public void ChangePlayerBullet()
    {
        if(_selectedBulletIndex < bulletsItems.Count)
        {
            PlayerComponents.Instance.PlayerAttack.ChangeBullet(bulletsItems[_selectedBulletIndex].bulletPrefab);
        }
    }
}
