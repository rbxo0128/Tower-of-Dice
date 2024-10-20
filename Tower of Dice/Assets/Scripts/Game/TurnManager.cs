using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst {get;private set;}
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("카드 배분이 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]
    public bool isLoading;
    public bool isShowDeck;
    public bool myTurn;
    public bool isAttacking;

    enum ETurnMode {Random, My, Other}
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    public int draw = 5;

    PlayData playData = PlayData.Inst;

    void GameSetup()
    {   
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0,2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }

        if (playData.myPlayer.level == 1)
            draw = 5;
        else if (playData.myPlayer.level >= 2)
            draw = 6;
    }

    public IEnumerator StartGameCo() // 게임 시작
    {
        GameSetup();
        isLoading = true;
        isAttacking = true;
        yield return delay05;
        StartCoroutine(StartTurnCo());
    }

    public IEnumerator StartTurnCo() // 턴 시작 여기에 적 공격 주사위
    {
        if (myTurn)
        {
            GameManager.Inst.Notification("My Turn");
        }
        
        if (myTurn)
        {
            OnAddCard?.Invoke(false);
            OnAddCard?.Invoke(false);
            CardManager.Inst.SetupEnemy();
            for (int i =0; i< draw; i++)
            {
                yield return delay05;
                SoundManager.Inst.PlayEffect(2);
                OnAddCard?.Invoke(true);
            }
            isAttacking = false;
            isLoading = false;
        }
        yield return delay07;

        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        
        if (!myTurn)
        {
            isAttacking = true;
            isLoading = true;
            CardManager.Inst.removeAlignment();
            StartCoroutine(EndTurnSequence());
        }
        
        else
            StartCoroutine(StartTurnCo());
    }

    IEnumerator EndTurnSequence()
    {
        yield return new WaitForSeconds(1.0f);
        DoAttack();
        yield return new WaitForSeconds(5.7f); // 총 6.7초를 기다리기 위해서
        DoRemove();
        yield return new WaitUntil(() => EntityManager.Inst.isEndTurn);
        DoStartTurn();
    }

    void DoAttack()
    {
        EntityManager.Inst.Attack(EntityManager.Inst.myEntity, EntityManager.Inst.otherEntity);
    }

    void DoRemove()
    {
        EntityManager.Inst.RemoveAll();
        CardManager.Inst.removeAll();
    }

    void DoStartTurn()
    {
        StartCoroutine(StartTurnCo());
    }

    public IEnumerator WaitforSec()
    {
        yield return delay07;
        yield return delay07;
        yield return delay07;
        yield return delay07;
    }

}
