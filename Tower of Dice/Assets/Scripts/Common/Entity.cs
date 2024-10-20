using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer character;
    [SerializeField] SpriteRenderer dicespot1;
    [SerializeField] SpriteRenderer dicespot2;
    [SerializeField] SpriteRenderer dicespot3;
    [SerializeField] SpriteRenderer dicespot4;
    [SerializeField] SpriteRenderer dicespot5;
    [SerializeField] SpriteRenderer dicespot6;
    [SerializeField] SpriteRenderer selectSpot;
    [SerializeField] Sprite spot1;
    [SerializeField] Sprite spot2;
    [SerializeField] Sprite spot3;
    [SerializeField] Sprite spot4;
    [SerializeField] Sprite spot5;
    [SerializeField] Sprite spot6;
    [SerializeField] Sprite blank;
    [SerializeField] Slider HealthBar;
    [SerializeField] Image Shield;

    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text shieldTMP;


    public bool isMine;
    public bool isBossOrEmpty;
    public bool isDie;
    public bool attackable;
    public Vector3 originPos;
    public PRS originPRS;

    public Item items;

    public int maxhealth;
    public int health;

    PlayData playData = PlayData.Inst;

    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
        if (HealthBar)
        {
            if(isMine)
            {
                health = playData.myPlayer.health;
                maxhealth = playData.myPlayer.maxhealth;
            }

            if(isBossOrEmpty)
            {

            }
            HealthBar.maxValue = maxhealth;
            HealthBar.value = health;

            healthTMP.text = $"{HealthBar.value} / {HealthBar.maxValue}";
            Shield.DOFade(0, 0);
            shieldTMP.DOFade(0, 0);
        }
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void OnTurnStarted(bool myTurn)
    {
        if (isBossOrEmpty)
            return;
    }

    public void Setup(Item item)
    {
        this.item = item;
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;

        Sprite[] spots = new Sprite[6]; // 수정 해야될듯? 이미지 넣기

        spots[0] = spot1;
        spots[1] = spot2;
        spots[2] = spot3;
        spots[3] = spot4;
        spots[4] = spot5;
        spots[5] = spot6;
        
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        dicespot1.sprite = spots[this.item.dicespot1 - 1];
        dicespot2.sprite = spots[this.item.dicespot2 - 1];
        dicespot3.sprite = spots[this.item.dicespot3 - 1];
        dicespot4.sprite = spots[this.item.dicespot4 - 1];
        dicespot5.sprite = spots[this.item.dicespot5 - 1];
        dicespot6.sprite = spots[this.item.dicespot6 - 1];

        items = item;
    }

    public void SetupSpot(Item item, int rand)
    {
        this.item = item;
        entity.sprite = null;
        character.sprite = null;
        nameTMP.text = "";

        Sprite[] spots = new Sprite[6]; // 수정 해야될듯? 이미지 넣기
        spots[0] = dicespot1.sprite;
        spots[1] = dicespot2.sprite;
        spots[2] = dicespot3.sprite;
        spots[3] = dicespot4.sprite;
        spots[4] = dicespot5.sprite;
        spots[5] = dicespot6.sprite;
        
        dicespot1.sprite = null;
        dicespot2.sprite = null;
        dicespot3.sprite = null;
        dicespot4.sprite = null;
        dicespot5.sprite = null;
        dicespot6.sprite = null;

        StartCoroutine(RolltheDice(spots, rand));

        items = item;
    }
    public void SetupNull()
    {
        selectSpot.sprite = null;
    }

    IEnumerator RolltheDice(Sprite[] spots, int rand)
    {
        for (int i=0; i<15; i++)
        {
            int randomSide = Random.Range(0,5);
            selectSpot.sprite = spots[randomSide];

            yield return new WaitForSeconds(0.05f);
        }

        selectSpot.sprite = spots[rand];
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }

    void OnMouseDown()
    {
        if (isMine)
            {
                if(SceneManager.GetSceneByName("Stage").isLoaded)
                    StageEntityManager.Inst.EntityMouseDown(this);
                else
                    EntityManager.Inst.EntityMouseDown(this);
            }
    }
    void OnMouseUp()
    {
        if (isMine)
            {
                if(SceneManager.GetSceneByName("Stage").isLoaded)
                    StageEntityManager.Inst.EntityMouseUp();
                else
                    EntityManager.Inst.EntityMouseUp();
            }
    }
    void OnMouseDrag()
    {
        if (isMine)
            {
                if(SceneManager.GetSceneByName("Stage").isLoaded)
                    StageEntityManager.Inst.EntityMouseDrag();
                else
                    EntityManager.Inst.EntityMouseDrag();
            }
    }


    public void SetShield(int shield)
    {
        if (shield !=0)
       {
            Shield.DOFade(1.0f, 0.5f).SetEase(Ease.OutSine);
            shieldTMP.DOFade(1.0f, 0.5f).SetEase(Ease.OutSine);
            shieldTMP.text = shield.ToString();
        }
        else
        {
            Shield.DOFade(0, 0.5f).SetEase(Ease.OutSine);
            shieldTMP.DOFade(0, 0.5f).SetEase(Ease.OutSine);
        }
    }

    public void Damaged(int damage, int shield)
    {
        if (damage == -1)
            damage = 0;
            
        if (damage > shield)
        {
            health = health + shield - damage;
            shield = 0;
        }
        else
            shield -= damage;
        SetShield(shield);
        
        HealthBar.value = health;
        healthTMP.text = $"{HealthBar.value} / {HealthBar.maxValue}";

        if (health<=0)
        {
            health = 0;
            isDie = true;
            if (isMine)
                playData.myPlayer.health = health;
        }

        if (isMine)
            playData.myPlayer.health = health;
    }

    public void SetHP(int hp)
    {        
        health = hp;
        HealthBar.value = health;
        healthTMP.text = $"{HealthBar.value} / {HealthBar.maxValue}";

        if (isMine)
            playData.myPlayer.health = health;
    }

    public void SetHP()
    {        
        HealthBar.value = playData.myPlayer.health;
        healthTMP.text = $"{HealthBar.value} / {HealthBar.maxValue}";
    }

    public void SetEnemyHP() // 임시 나중에 지울거임 치트용
    {        
        HealthBar.maxValue = maxhealth;
        HealthBar.value = health;
        healthTMP.text = $"{HealthBar.value} / {HealthBar.maxValue}";
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
