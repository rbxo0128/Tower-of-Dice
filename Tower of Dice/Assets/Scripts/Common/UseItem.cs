using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    public GameObject itemInfo;
    [SerializeField] TMP_Text InfoTMP;
    [SerializeField] SpriteRenderer itemsprite;
    [SerializeField] Image imagesprite;

    public UsingItem item;

    public string itemName;


    public void Setup(UsingItem item)
    {
        this.item = item;

        itemName = item.name;

        itemsprite.sprite = item.itemsprite;

        InfoTMP.text = item.name+"\n"+item.ability;
        GetComponent<Order>().SetOriginOrder(2);
    }

    public void SetupShop(UsingItem item)
    {
        this.item = item;

        itemName = item.name;

        imagesprite.sprite = item.itemsprite;
        //InfoTMP.text = item.name+"\n"+item.ability;
        //GetComponent<Order>().SetOriginOrder(2);
    }

    void OnMouseOver()
    {
        ItemManager.Inst.ItemMouseOver(this);
    }

    void OnMouseExit()
    {
        ItemManager.Inst.ItemMouseExit(this);
    }

    void OnMouseDown()
    {
        ItemManager.Inst.ItemMouseDown();
    }

    void OnMouseUp()
    {
        ItemManager.Inst.ItemMouseUp();
    }
}
