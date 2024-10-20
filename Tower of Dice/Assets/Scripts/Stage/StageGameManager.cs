using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

// 치트 ,UI, 게임오버. 랭킹
public class StageGameManager : MonoBehaviour
{
    public static StageGameManager Inst {get; private set;}
    void Awake() => Inst = this;

    public bool isOnDeck;
    public Vector3 playerpos;
    int a;

    public GameObject prefab; // 추가할 이미지를 가진 프리팹
    public RectTransform content; // 스크롤뷰의 콘텐츠 영역

    public int numberOfImages; // 추가할 이미지의 개수

    [SerializeField] ShowDeckPanel deckPanel;
    [SerializeField] public SpriteRenderer player;

    PlayData playData = PlayData.Inst;

    void Start() // 게임 시작 시 데이터가 있으면 이어오기
    {
        AddImagesToScrollView();

        if (!(playData.myPlayer.stage == 1))
        {
            SetupPlayer();
            if (!playData.myPlayer.isBattle)
                StageTurnManager.Inst.DoStartStage();
            else
                StartGame();
        }

        else
        {
            playData.myStage.SetupNew(0);
            StartGame();
        }

        if(playData.myPlayer.stage <= 21 && !playData.myPlayer.FirstBossClear)
        {
            playData.myStage.SetupLoad(0);
        }
        else if(playData.myPlayer.stage == 21 && playData.myPlayer.FirstBossClear)
        {
            playData.myStage.SetupLoad(0);
            StartCoroutine(playData.myStage.LerpAndLoadNextStage(1));
        }
        else if(playData.myPlayer.stage <= 41 && !playData.myPlayer.SecondBossClear)
        {
            playData.myStage.SetupLoad(1);
        }
        else if(playData.myPlayer.stage == 41&& playData.myPlayer.SecondBossClear)
        {
            playData.myStage.SetupLoad(1);
            StartCoroutine(playData.myStage.LerpAndLoadNextStage(2));
        }
        else if(playData.myPlayer.stage <= 61)
        {
            playData.myStage.SetupLoad(2);
        }
        
        if (playData.myPlayer.job == 1)
            player.transform.position = new Vector3(-0.8f, -8.8f, 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        InputCheatKey();
#if UNITY_EDITOR
        //InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            Move();
        if (Input.GetKeyDown(KeyCode.Keypad1))
            playData.myPlayer.FirstBossClear = true;
        if (Input.GetKeyDown(KeyCode.Keypad2))
            playData.myPlayer.SecondBossClear = true;
        if (Input.GetKeyDown(KeyCode.Keypad3)|| Input.GetKeyDown(KeyCode.Alpha3))
            playData.myPlayer.gold += 50;
    }
    public Vector3 getPosition()
    {
        return player.transform.position;
    }

    void SetupPlayer() // 플레이어 위치 동기화
    {
        player.transform.position = playData.myPlayer.playerpos;
        if (playData.myPlayer.stage%20 > 6 && playData.myPlayer.stage%20 < 17)
            player.flipX =true;
        else
            player.flipX = false;
    }
    public void StartGame()
    {
        StartCoroutine(StageTurnManager.Inst.StartGameCo());
    }

    void AddImagesToScrollView()
    {
        // 이미지 개수만큼 반복하여 프리팹을 생성하고 스크롤뷰에 추가
        for (int i = 0; i < numberOfImages; i++)
        {
            // 프리팹을 복제하여 이미지 GameObject 생성
            GameObject imageGO = Instantiate(prefab, content);
            Stage stageComponent = imageGO.GetComponent<Stage>();

            // 이미지 GameObject의 RectTransform 컴포넌트 가져오기
            RectTransform imageRectTransform = imageGO.GetComponent<RectTransform>();

            // 이미지의 위치 설정 (스크롤뷰의 콘텐츠 영역 내에서 이동)
            imageRectTransform.anchoredPosition = new Vector3(-1.510986f, -32f);

            playData.myStage = stageComponent;
        }
    }

    public void DeckShow()
    {
        if (isOnDeck || StageTurnManager.Inst.isLoading)
            return;
        isOnDeck = true;
        int deckCount = StageCardManager.Inst.itemBuffer.Count;
        int trashCount = StageCardManager.Inst.itemDeckBuffer.Count;
        CheckDeck.Inst.setCount(deckCount,trashCount);
        StageEntityManager.Inst.ShowAll(false);

        for(int i = 0; i < deckCount; i++)
        {
            CheckDeck.Inst.AddScroll(StageCardManager.Inst.itemBuffer[i], true);
        }
        for(int i = 0; i < trashCount; i++)
        {
            CheckDeck.Inst.AddScroll(StageCardManager.Inst.itemDeckBuffer[i], false);
        }

        deckPanel.Show();
    }
    
    public void DeckHide()
    {
        StageEntityManager.Inst.ShowAll(true);
        CheckDeck.Inst.Clear();
        deckPanel.Hide();
        isOnDeck = false;
    }

    
    Vector3 positionOffset = Vector3.zero;
    Vector3 positionOffset2 = Vector3.zero;
    public void Move()
    {
        SoundManager.Inst.PlayEffect(0);
        playerpos = player.transform.position;
        
        int a = 0;

        if (playData.myPlayer.stage < 20)
            a = playData.myPlayer.stage+1;

        else if(playData.myPlayer.stage < 40)
            a = playData.myPlayer.stage - 19;

        else if(playData.myPlayer.stage < 60)
            a = playData.myPlayer.stage - 39;

        if (playData.myPlayer.job == 1)
        {
            positionOffset = new Vector3(-0.8f, -0.3f, 0.0f);
            positionOffset2 = new Vector3(0f, -0.3f, 0.0f);
        }

        if (a == 1)
        {
            player.transform.DOJump(new Vector3(0, -8.5f, 1) + positionOffset, 1, 1, 0.4f);
        }
        else if (a == 2)
        {
            if ((playData.myPlayer.stage == 21 && !playData.myPlayer.FirstBossClear) || (playData.myPlayer.stage == 41 && !playData.myPlayer.SecondBossClear))
                return;
            player.transform.DOJump(new Vector3(-3.83f, -7.23f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 3)
        {
            player.transform.DOJump(new Vector3(-6.71f, -5.08f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 4)
        {
            player.transform.DOJump(new Vector3(-9.68f, -2.84f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 5)
        {
            player.transform.DOJump(new Vector3(-12.74f, -0.47f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 6)
        {
            player.transform.DOJump(new Vector3(-15.71f, 1.72f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 7)
        {
            player.flipX = true;
            player.transform.DOJump(new Vector3(-11.92f, 2.95f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 8)
        {
            player.transform.DOJump(new Vector3(-9f, 4.96f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 9)
        {
            player.transform.DOJump(new Vector3(-6f, 7f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 10)
        {
            player.transform.DOJump(new Vector3(-2.88f, 9f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 11)
        {
            player.transform.DOJump(new Vector3(0f, 11f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 12)
        {
            player.transform.DOJump(new Vector3(2.87f, 8.83f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 13)
        {
            player.transform.DOJump(new Vector3(5.84f, 7.46f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 14)
        {
            player.transform.DOJump(new Vector3(8.9f, 5.81f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 15)
        {
            player.transform.DOJump(new Vector3(11.91f, 4.3f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 16)
        {
            player.transform.DOJump(new Vector3(16f, 1.74f, 1)+ positionOffset2, 1, 1, 0.4f);
        }
        else if (a == 17)
        {
            player.flipX = false;
            player.transform.DOJump(new Vector3(12.73f, -0.86f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 18)
        {
            player.transform.DOJump(new Vector3(9.63f, -3f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        else if (a == 19)
        {
            player.transform.DOJump(new Vector3(6.66f, -4.88f, 1)+ positionOffset,1,1,0.4f);
        }
        else if (a == 20)
        {
            player.transform.DOJump(new Vector3(3.6f, -6.89f, 1)+ positionOffset, 1, 1, 0.4f);
        }
        if(playData.myPlayer.job == 0)
        {
            if (playData.myPlayer.health < playData.myPlayer.maxhealth)
            {
                playData.myPlayer.health += 1;
                TopBar.Inst.HealParticle(1);
            }
        }
        playData.myPlayer.stage += 1;
    }
}
