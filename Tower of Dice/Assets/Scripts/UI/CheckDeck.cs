using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CheckDeck : MonoBehaviour
{
    public static CheckDeck Inst {get; private set;}
    void Awake() => Inst = this;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<ImageCard> myDeck;
    [SerializeField] List<ImageCard> myTrashDeck;
    [SerializeField] TMP_Text deckCount;
    [SerializeField] TMP_Text trashCount;
    public Transform ContentContainer;
    public Transform ContentContainer1;


    public void setCount(int deckcount, int trashcount)
    {
        deckCount.text = deckcount.ToString();
        trashCount.text = trashcount.ToString();
    }
    public void AddScroll(Item item, bool isDeck)
    {
        var cardObject = Instantiate(cardPrefab, Utils.MousePos, Utils.QI);
        var card = cardObject.GetComponent<ImageCard>();
        if (isDeck)
        {
            myDeck.Add(card);
            card.SetupDeck(item, true);
        }
        else
        {
            myTrashDeck.Add(card);
            card.SetupDeck(item, false);
        }

        card.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
    }
    
    public void Clear()
    {
        int count = myDeck.Count;
        int countd = myTrashDeck.Count;
        for(int i=0; i< count;i++)
        {
            DestroyImmediate(myDeck[0].gameObject);
            myDeck.RemoveAt(0);
        }
        for(int i=0; i< countd;i++)
        {
            DestroyImmediate(myTrashDeck[0].gameObject);
            myTrashDeck.RemoveAt(0);
        }
    }

}
