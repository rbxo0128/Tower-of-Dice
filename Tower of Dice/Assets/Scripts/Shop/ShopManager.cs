using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] UseItemSO useItemSO;
    public List<Item> ShopItemList;
    public List<UsingItem> ShopUsingItemList;
    public List<ImageCard> ShopCardList;
    public List<UseItem> ShopUseItemList;

    PlayData playData = PlayData.Inst;
    
    // Start is called before the first frame update
    void Start()
    {
        SetShopItems();
        SetShopUseItems();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheatKey();
#if UNITY_EDITOR
        //InputCheatKey();
#endif
    }

    void SetShopItems()
    {
        for(int i = 0; i < itemSO.shopItems.Length; i++)
        {
            Item item = itemSO.shopItems[i];
            for (int j = 0; j < item.percent; j++)
                ShopItemList.Add(item);
        }


        for(int i =0; i<ShopItemList.Count; i++)
        {
            int rand = Random.Range(i, ShopItemList.Count);
            Item temp = ShopItemList[i];
            ShopItemList[i] = ShopItemList[rand];
            ShopItemList[rand] = temp;
        }

        for (int i=0; i<ShopCardList.Count; i++)
        {
            ShopCardList[i].SetupDeck(ShopItemList[i]);
            ShopBtn shopBtn = ShopCardList[i].GetComponentInParent<ShopBtn>();
            shopBtn.SetPrice(ShopItemList[i].price);
        }

    }

    void SetShopUseItems()
    {
        for(int i = 0; i < useItemSO.useItem.Length; i++)
        {
            UsingItem item = useItemSO.useItem[i];
            for (int j = 0; j < item.percent; j++)
                ShopUsingItemList.Add(item);
        }


        for(int i =0; i<ShopUsingItemList.Count; i++)
        {
            int rand = Random.Range(i, ShopUsingItemList.Count);
            UsingItem temp = ShopUsingItemList[i];
            ShopUsingItemList[i] = ShopUsingItemList[rand];
            ShopUsingItemList[rand] = temp;
        }

        for (int i=0; i<ShopUseItemList.Count; i++)
        {
            ShopUseItemList[i].SetupShop(ShopUsingItemList[i]);
            ShopBtn shopBtn = ShopUseItemList[i].GetComponentInParent<ShopBtn>();
            shopBtn.SetPrice(ShopUsingItemList[i].price);
        }

    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
            playData.myPlayer.gold += 50;
    }
}
