using InterOrbital.Item;
using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelector : MonoBehaviour
{
    private const int MAXIMUM_BULLETS = 4;

    [SerializeField] private List<BulletSlot> bulletsSlots;
    [SerializeField] private List<ItemBulletScriptableObject> bulletsItems;
    [SerializeField] private ItemBulletScriptableObject emptyBullet;

    [SerializeField] private Sprite bulletSlotSelectedImage;
    [SerializeField] private Sprite bulletSlotNoSelectedImage;

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
        InitializeBulletSelector();
    }

    private void InitializeBulletSelector()
    {
        for (int i = 0; i < MAXIMUM_BULLETS; i++)
        {
            bulletsItems.Add(emptyBullet);
            bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
            bulletsSlots[i].SetBackgroundSprite(bulletSlotNoSelectedImage);
            bulletsSlots[i].SetBulletAmount(0);
        }
        bulletsSlots[_selectedBulletIndex].SetBackgroundSprite(bulletSlotSelectedImage);
        ChangePlayerBullet();
    }

    private void UpdateBulletSelectorUI()
    {
        for (int i = 0; i < bulletsSlots.Count; i++)
        {
            bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
        }
    }

    public void ChangeBulletInList(int index, ItemBulletScriptableObject bullet)
    {
        if (index < bulletsItems.Count)
        {
            bulletsItems[index] = bullet;
        }
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
