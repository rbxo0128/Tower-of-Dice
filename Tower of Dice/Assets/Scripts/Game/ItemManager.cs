using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Inst {get; private set;}
    
    UsingItem emptyItem;
    public UseItemSO useItemSO;

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    UseItem selectItem;

    [SerializeField] GameObject itemPrefab;
    public static Action<bool> OnAddCard;
    
    [SerializeField] Transform[] itemSpawnPoint;
    public Transform container;
    public List<UsingItem> UsingItems;
    public bool criticalDamage;

    int EmptyIndex;

    PlayData playData = PlayData.Inst;

    void Start()
    {
        playData = PlayData.Inst;
        emptyItem = useItemSO.useItem[useItemSO.useItem.Length-1].Copy();
        for (int i = 0; i<playData.myPlayer.useItems.Count; i++)
        {
            if (playData.myPlayer.useItems[i].percent == 0)
                {
                    
                }
            else
                AddItem(playData.myPlayer.useItems[i].Copy(), playData.myPlayer.useItems[i].Copy().place);
        }

        EmptyIndex = UsingItems.FindIndex(x=> x.percent==0);
    }
    void Update()
    {
        
    }
    
    public void AddItem(UsingItem usingItem, int place)
    {
        var itemObject = Instantiate(itemPrefab, itemSpawnPoint[place].position, Utils.QI);
        var item = itemObject.GetComponent<UseItem>();
        UsingItems[place] = usingItem;

        item.Setup(usingItem);
        item.transform.SetParent(container);
        EmptyIndex = UsingItems.FindIndex(x=> x.percent==0);
    }

    public void AddItem(UsingItem usingItem)
    {
        var itemObject = Instantiate(itemPrefab, itemSpawnPoint[EmptyIndex].position, Utils.QI);
        var item = itemObject.GetComponent<UseItem>();
        playData.myPlayer.useItems[EmptyIndex] = usingItem;
        playData.myPlayer.useItems[EmptyIndex].place = EmptyIndex;
        UsingItems[EmptyIndex] = usingItem;

        item.Setup(usingItem);
        item.transform.SetParent(container);
        EmptyIndex = UsingItems.FindIndex(x=> x.percent==0);
    }

    public void ItemUse(UseItem item)
    {
        if (SceneManager.GetSceneByName("Game").isLoaded)
            if (TurnManager.Inst.isLoading || TurnManager.Inst.isAttacking)
                return;
                
        if (SceneManager.GetSceneByName("Stage").isLoaded)
            if (StageTurnManager.Inst.isLoading)
                return;

        if (item.itemName == "체력 포션")
        {
            int healamount = 10;
            playData.myPlayer.health += healamount;
            if (playData.myPlayer.health > playData.myPlayer.maxhealth)
            {
                healamount = playData.myPlayer.health - playData.myPlayer.maxhealth;
                playData.myPlayer.health = playData.myPlayer.maxhealth;
            }
            TopBar.Inst.HealParticle(healamount);
            if(SceneManager.GetSceneByName("Game").isLoaded)
                EntityManager.Inst.myEntity.SetHP();
        }
    
        if (item.itemName == "드로우 포션")
        {
            if (SceneManager.GetSceneByName("Shop").isLoaded)
                return;

            else if(SceneManager.GetSceneByName("Stage").isLoaded)
            {
                StageCardManager.Inst.AddCard(true);
                StageCardManager.Inst.AddCard(true);
            }
            else if(SceneManager.GetSceneByName("Game").isLoaded)
            {
                CardManager.Inst.AddCard(true);
                CardManager.Inst.AddCard(true);
            }
        }

        if (item.itemName == "딜 포션")
        {
            if (SceneManager.GetSceneByName("Shop").isLoaded || SceneManager.GetSceneByName("Stage").isLoaded)
                return;

            else if(SceneManager.GetSceneByName("Game").isLoaded)
            {
                EntityManager.Inst.itemDamaged(10);
            }
        }


        if (item.itemName == "금 주머니")
        {
            if (SceneManager.GetSceneByName("Shop").isLoaded)
                return;
            playData.myPlayer.gold += 100;
        }

        if (item.itemName == "치명타 포션")
        {
            if (SceneManager.GetSceneByName("Shop").isLoaded || SceneManager.GetSceneByName("Stage").isLoaded || criticalDamage)
                return;
            
            criticalDamage = true;
        }

        int place = item.item.place;
        UsingItems[item.item.place] = emptyItem;

        playData.myPlayer.useItems[item.item.place] = emptyItem;

        Destroy(item.gameObject);

        EmptyIndex = UsingItems.FindIndex(x=> x.percent==0);
    }

    public void ShowItemInfo(UseItem item, bool isShow)
    {
        if(isShow)
            item.itemInfo.SetActive(true);
        else
            item.itemInfo.SetActive(false);
    }
    public void ItemMouseOver(UseItem item)
    {
        selectItem = item;
        ShowItemInfo(item, true);
    }
    
    public void ItemMouseExit(UseItem item)
    {
        ShowItemInfo(item, false);
    }

    public void ItemMouseDown()
    {
        ItemUse(selectItem);
    }

    public void ItemMouseUp()
    {
        
    }
}
