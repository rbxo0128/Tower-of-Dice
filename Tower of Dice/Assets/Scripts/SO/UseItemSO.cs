using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 [System.Serializable]
public class UsingItem
{
    public string name;
    public string ability;
    public Sprite itemsprite;
    public int price;
    public int percent;
    public int place;

    public UsingItem Copy()
    {
        return new UsingItem
        {
            name = this.name,
            ability = this.ability,
            itemsprite = this.itemsprite,
            price = this.price,
            percent = this.percent,
            place = this.place
        };
    }

    ///////////////////////////////////// 카드

    ////////////////////////////////////아이템
}

 [CreateAssetMenu(fileName = "UseItemSO", menuName = "Scriptable Object/UseItemSO")]
public class UseItemSO : ScriptableObject
{
    public UsingItem[] useItem;
}
