 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] SpriteRenderer character;
    [SerializeField] SpriteRenderer dicespot1;
    [SerializeField] SpriteRenderer dicespot2;
    [SerializeField] SpriteRenderer dicespot3;
    [SerializeField] SpriteRenderer dicespot4;
    [SerializeField] SpriteRenderer dicespot5;
    [SerializeField] SpriteRenderer dicespot6;
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
    bool isFront;
    public PRS originPRS;
    public bool enemy;

    
    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront; // 여기 임시용
        
        Sprite[] spots = new Sprite[6]; // 수정 해야될듯? 이미지 넣기

        spots[0] = spot1;
        spots[1] = spot2;
        spots[2] = spot3;
        spots[3] = spot4;
        spots[4] = spot5;
        spots[5] = spot6;
        

        if (this.isFront)
        {
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

        else
        {
            card.sprite = cardBack;
            character.sprite = this.item.sprite;
            nameTMP.text = "";
            dicespot1.sprite = null;
            dicespot2.sprite = null;
            dicespot3.sprite = null;
            dicespot4.sprite = null;
            dicespot5.sprite = null;
            dicespot6.sprite = null;
        }
    }

    void OnMouseOver()
    {
        if (isFront && !enemy)
        {
            if (SceneManager.GetSceneByName("Stage").isLoaded)
                StageCardManager.Inst.CardMouseOver(this);
            else
                CardManager.Inst.CardMouseOver(this);
        }
        if(enemy)
            CardManager.Inst.CardEnemyMouseOver(this);
    }

    void OnMouseExit()
    {
        if (isFront&&!enemy)
            {
                if (SceneManager.GetSceneByName("Stage").isLoaded)
                    StageCardManager.Inst.CardMouseExit(this);
                else
                    CardManager.Inst.CardMouseExit(this);
            }
        if(enemy)
            CardManager.Inst.CardEnemyMouseExit(this); 
    }

    void OnMouseDown()
    {
        if (isFront && !enemy)
            {
            if (SceneManager.GetSceneByName("Stage").isLoaded)
                StageCardManager.Inst.CardMouseDown();
            else
                CardManager.Inst.CardMouseDown();
            }
    }

    void OnMouseUp()
    {
        if (isFront && !enemy)
            {
            if (SceneManager.GetSceneByName("Stage").isLoaded)
                StageCardManager.Inst.CardMouseUp();
            else
                CardManager.Inst.CardMouseUp();
            }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos,dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }

        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
