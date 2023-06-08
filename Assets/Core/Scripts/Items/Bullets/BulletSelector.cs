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

    public void UpdateBulletSelectorUI()
    {
        int j = bulletsSlots.Count - 1;
        for (int i =  0; i < bulletsSlots.Count; i++)
        {
            
            int index = PlayerComponents.Instance.Inventory.GetTotalNumberOfSlots() - (j+1);
            j--;

            ItemObject itemInInventory = PlayerComponents.Instance.Inventory.GetItemObjectByIndex(index);
            if(itemInInventory.itemSo != PlayerComponents.Instance.Inventory.itemVoid)
            {
                bulletsItems[i] = (ItemBulletScriptableObject) itemInInventory.itemSo;
            }
            else
            {
                bulletsItems[i] = emptyBullet;
            }
                bulletsSlots[i].SetBulletSprite(bulletsItems[i].itemSprite);
                bulletsSlots[i].SetBulletAmount(itemInInventory.amount);
        }

        ChangePlayerBullet();
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

    public void SubstractBullet()
    {
        int j = bulletsSlots.Count - (_selectedBulletIndex + 1);
        PlayerComponents.Instance.Inventory.SubstractBulletInInventory(j);
        UpdateBulletSelectorUI();
    }
}
