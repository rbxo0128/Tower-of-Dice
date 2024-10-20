using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageCard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image card;
    [SerializeField] Image character;
    [SerializeField] Image dicespot1;
    [SerializeField] Image dicespot2;
    [SerializeField] Image dicespot3;
    [SerializeField] Image dicespot4;
    [SerializeField] Image dicespot5;
    [SerializeField] Image dicespot6;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite spot1;
    [SerializeField] Sprite spot2;
    [SerializeField] Sprite spot3;
    [SerializeField] Sprite spot4;
    [SerializeField] Sprite spot5;
    [SerializeField] Sprite spot6;

    public Item item;

    void Start()
    {
        /*card.sprite = cardBack;
        character.sprite = cardBack;
        dicespot1.sprite = cardBack;
        dicespot2.sprite = cardBack;
        dicespot3.sprite = cardBack;
        dicespot4.sprite = cardBack;
        dicespot5.sprite = cardBack;
        dicespot6.sprite = cardBack;

        nameTMP.text ="";*/

    }

    // Update is called once per frame
    public void SetupDeck(Item item, bool isDeck)
    {
        this.item = item;
        
        Sprite[] spots = new Sprite[6]; // 수정 해야될듯? 이미지 넣기

        spots[0] = spot1;
        spots[1] = spot2;
        spots[2] = spot3;
        spots[3] = spot4;
        spots[4] = spot5;
        spots[5] = spot6;
        
        card.sprite = cardFront;
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        dicespot1.sprite = spots[this.item.dicespot1 - 1];
        dicespot2.sprite = spots[this.item.dicespot2 - 1];
        dicespot3.sprite = spots[this.item.dicespot3 - 1];
        dicespot4.sprite = spots[this.item.dicespot4 - 1];
        dicespot5.sprite = spots[this.item.dicespot5 - 1];
        dicespot6.sprite = spots[this.item.dicespot6 - 1];

        if (isDeck)
            transform.SetParent(CheckDeck.Inst.ContentContainer);
        if (!isDeck)
            transform.SetParent(CheckDeck.Inst.ContentContainer1);
    }

    public void SetupDeck(Item item)
    {
        this.item = item;
        
        Sprite[] spots = new Sprite[6]; // 수정 해야될듯? 이미지 넣기

        spots[0] = spot1;
        spots[1] = spot2;
        spots[2] = spot3;
        spots[3] = spot4;
        spots[4] = spot5;
        spots[5] = spot6;
        
        card.sprite = cardFront;
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        dicespot1.sprite = spots[this.item.dicespot1 - 1];
        dicespot2.sprite = spots[this.item.dicespot2 - 1];
        dicespot3.sprite = spots[this.item.dicespot3 - 1];
        dicespot4.sprite = spots[this.item.dicespot4 - 1];
        dicespot5.sprite = spots[this.item.dicespot5 - 1];
        dicespot6.sprite = spots[this.item.dicespot6 - 1];
    }
}

