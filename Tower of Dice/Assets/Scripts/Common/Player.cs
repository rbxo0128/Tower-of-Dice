using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Data data;


    public int job; // 0 : 전사 1: 기사 2: 미정
    public int level;
    public int exp;
    public int maxexp;
    public int maxhealth;
    public int health;
    public int gold;
    public int stage;
    public int seed;
    public int limitCards;
    public bool isBattle;
    public bool FirstBossClear;
    public bool SecondBossClear;
    public bool ThirdBossClear;
    public bool drawEvent;
    public bool limitEvent;

    public List<int> stage_Quad;

    public Vector3 playerpos;

    public Sprite sprite; // 플레이어 직업이나 필요한 이미지는 여기서 불러오게
    public List<UsingItem> useItems;

    UsingItem emptyItem;

    public void SetupNew()
    {
        level = 1;
        exp = 0;
        maxexp = level*5;
        maxhealth = 40;
        if (job == 1)
            maxhealth = 60; // 데이터 저장해서 하고 싶은데 어렵네...
        health = maxhealth;
        gold = 100;
        stage = 1;
        isBattle = true;
        stage_Quad.Clear();
        Setitems();
        FirstBossClear = false;
        SecondBossClear = false;
        ThirdBossClear = false;
        limitCards = 2;
        seed = Random.Range(1,10000);
    }

    public void SetSeed()
    {
        Random.InitState(seed);
        seed = Random.Range(1,10000);
    }

    public void Setitems()
    {
        useItems.Clear();
        useItems.Add(emptyItem);
        useItems.Add(emptyItem);
        useItems.Add(emptyItem);
    }

    public void LevelUp(int exprest)
    {
        maxhealth += 5;
        health += 5;
        level += 1;
        maxexp = level*5;
        exp = exprest;
        TopBar.Inst.HealParticle(5);
        if (level == 3)
            limitCards += 1;
    }
}
