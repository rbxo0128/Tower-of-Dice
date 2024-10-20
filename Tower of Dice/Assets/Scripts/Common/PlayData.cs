using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class PlayData : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] public Deck myDeck;
    [SerializeField] public Player myPlayer;
    [SerializeField] public Stage myStage;

    public GameObject prefab;
    public int numberOfImages = 10;
    
    string filedeckname = "decksave";
    string fileplayername = "playersave";
    string path;

    public static PlayData Inst { get; private set; }

    // 게임 시작 시 GameManager 인스턴스를 설정합니다.
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

        path = Application.persistentDataPath+"/";
        Load();
        
    }

    public void New()
    {
        // 플레이어
        myPlayer.SetupNew();
        // 덱
        myDeck.SetupDeck();
        // 아이템

        // 스테이지
        
        Save();
    }

    public void Load()
    {
        if(!File.Exists(path+filedeckname)||!File.Exists(path+fileplayername))
            New();
            
        string data_deck = File.ReadAllText(path+filedeckname);
        JsonUtility.FromJsonOverwrite(data_deck, myDeck);

        string data_player = File.ReadAllText(path+fileplayername);
        JsonUtility.FromJsonOverwrite(data_player, myPlayer);

    }

    public void Save()
    {
        string data_deck = JsonUtility.ToJson(myDeck);
        File.WriteAllText(path+filedeckname,data_deck);

        string data_player = JsonUtility.ToJson(myPlayer);
        File.WriteAllText(path+fileplayername,data_player);

    }


    public ItemSO Getitem()
    {
        return itemSO;
    }
}
