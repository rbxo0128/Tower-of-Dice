using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StageTurnManager : MonoBehaviour
{
    public static StageTurnManager Inst {get;private set;}
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField] [Tooltip("카드 배분이 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]

    public bool isLoading;
    public bool isShowDeck;
    public bool myTurn;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    public int draw;
    int move;
    float delay;

    PlayData playData = PlayData.Inst;
    
    void GameSetup()
    {   
        if (playData.myPlayer.level == 1)
            draw = 3;
        else if (playData.myPlayer.level >= 2)
            draw = 4;
            
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);
    }

    public IEnumerator StartGameCo() // 게임 시작
    {
        GameSetup();
        myTurn = true;
        isLoading = true;
        yield return delay05;
        StartCoroutine(StartTurnCo());
    }

    public IEnumerator StartTurnCo() // 턴 시작 여기에 적 공격 주사위
    {        
        if (myTurn)
        {
            for (int i =0; i< draw; i++)
            {
                yield return delay05;
                SoundManager.Inst.PlayEffect(2);
                OnAddCard?.Invoke(true);
            }
            isLoading = false;
        }
        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        
        if (!myTurn)
        {
            isLoading = true;
            StageCardManager.Inst.removeAlignment();
            Invoke("RollDice", 1.0f);
            Invoke("Move", 2.0f);
            Invoke("DoStartStage", 2.0f);
            playData.myPlayer.SetSeed();
            StartCoroutine(StartTurnCo()); 
        }
        
        else
            StartCoroutine(StartTurnCo());
    }

    void RollDice()
    {
        move = StageEntityManager.Inst.RollDice();
    }

    void Move()
    {
        delay = 1;
        for(int i =0; i< move; i++)
        {
            if ((playData.myPlayer.stage == 21 && !playData.myPlayer.FirstBossClear) || (playData.myPlayer.stage == 41 && !playData.myPlayer.SecondBossClear))
            {
                break;
            }
            Invoke("Moving", delay);
            delay += 0.6f;
        }
    }

    void Moving()
    {
        StageGameManager.Inst.Move();
    }

    public void DoStartStage()
    {
        float time = 2.0f;
        time += delay-1;
        Invoke("StartStage", time);
    }

    void StartStage() // 0: 적 1: 상점 2: 체력 회복 3: 보스 4:체력 감소 5:아이템 획득 6:버프 or 저주
    {
        playData.myPlayer.playerpos = StageGameManager.Inst.getPosition();
        if( playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 0) //적
        {
            playData.myPlayer.isBattle = false;
            MySceneManager.Instance.ChangeScene("Game");
        }
        else if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 1) // 상점
        {
            MySceneManager.Instance.ChangeScene("Shop");
        }
        else if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 3) // 보스
        {
            playData.myPlayer.isBattle = false;
            MySceneManager.Instance.ChangeScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Event", LoadSceneMode.Additive);
        }
        playData.Save();
    }



}
