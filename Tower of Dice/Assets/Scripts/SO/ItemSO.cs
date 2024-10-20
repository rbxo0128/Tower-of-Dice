using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class Item
{
    public string name;


    ///////////////////////////////////// 카드

    public string size;
    public int dicespot1;
    public int dicespot2;
    public int dicespot3;
    public int dicespot4;
    public int dicespot5;
    public int dicespot6;
    public int price;
    public Sprite sprite;
    public float percent;

    ////////////////////////////////////아이템
}

 [CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
    public Item[] enemyitems;
    public Item[] addItems;
    public Item[] shopItems;
    public Item[] goblinitems;
    public Item[] mushroomitems;
    public Item[] wolfitems;
    public Item[] zombieitems;
    public Item[] dinoitems;
}