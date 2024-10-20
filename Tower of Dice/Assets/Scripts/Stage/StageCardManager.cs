using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class StageCardManager : MonoBehaviour
{
    public static StageCardManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> CenterCard;
    
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] ECardState eCardState;
    [SerializeField] ShowDeckBtn showDeckbtn;
    [SerializeField] ShowDeckBtn RollBtn;
    [SerializeField] SpriteRenderer AttackArea;
    [SerializeField] ScrollRect MapScroll;
    [SerializeField] GameObject MainCamera;
    [SerializeField] Transform Trash;


    public List<Item> itemBuffer;
    public List<Item> itemDeckBuffer;
    Card selectCard;
    public bool isOnArea;
    bool isMyCardDrag;
    bool onMyCardArea;
    bool onSelectCardArea;
    enum ECardState {Nothing, CanMouseOver, CanMouseDrag}
    bool isMapScroll;
    Vector2 scrollOffset;

    PlayData playData = PlayData.Inst;

    int i = 0;
    public Item PopItem(bool isMine)
    {
        if (itemBuffer.Count == 0)
        {
            itemBuffer.AddRange(itemDeckBuffer);
            itemDeckBuffer.Clear();
            SetupItemBuffer();
        }

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetupItemBuffer() // 랜덤으로나오게
    {
        for(int i =0; i<itemBuffer.Count; i++)
        {
            Random.InitState(playData.myPlayer.seed);
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void Start()
    {
        PlayData playData = PlayData.Inst;
        if (playData != null && playData.myDeck != null)
        {
            // myDecks 리스트를 복사하여 Stage 씬에서 사용할 itemBuffer 리스트를 생성합니다.
            
            itemBuffer = new List<Item>(playData.myDeck.myDecks);
        }

        itemDeckBuffer = new List<Item>();

        AttackArea.transform.localScale = Vector3.zero;
        SetupItemBuffer();
        StageTurnManager.OnAddCard += AddCard;
        StageTurnManager.OnTurnStarted += OnTurnStarted;
        MapScroll.vertical = false;
    
        
    }

    void OnDestroy()
    {
        StageTurnManager.OnAddCard -= AddCard;
        StageTurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void OnTurnStarted(bool myTurn)
    {

    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        if(isOnArea && !isMapScroll)
            ShowArea();
        
        else if(!isOnArea && !isMyCardDrag)
            HideArea();

        DetectCardArea();
        SetECardState();
        isOnArea = StageEntityManager.Inst.CheckisOnArea();
        showDeckbtn.DeckSetup(itemBuffer.Count, itemDeckBuffer.Count);
        
        if (StageTurnManager.Inst.isShowDeck && i == 0)
        {
            i = 1;
            SetOriginOrderhide(true);
        }
        else if(!StageTurnManager.Inst.isShowDeck && i == 1)
        {
            i = 0;
            SetOriginOrder(true);
        }
    }


    public void AddCardbyEntity(bool isMine, Item item)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(item,isMine);
        myCards.Add(card);

        SetOriginOrder(isMine);
        cardAlignment(false);
    }

    public void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(isMine),isMine);
        myCards.Add(card);

        SetOriginOrder(true);
        cardAlignment(true);
    }

    public void removeAlignment()
    {
        Vector3 pos = Trash.transform.position;
        Quaternion rot = Quaternion.Euler(0,0,1);
        Vector3 scale = new Vector3(1f,1f,1f);
        PRS remove = new PRS(pos,rot,scale);
        for(int i = 0; i < myCards.Count;i++)
        {
            myCards[i].MoveTransform(remove, true, 0.7f);
        }
    }
    
    public bool TryPutCard()
    {
        Card card = selectCard;
        var spawnPos = Utils.MousePos;
        var targetCards = myCards;

        if (StageEntityManager.Inst.SpawnEntity(true, card.item))
        {
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);

            selectCard = null;

            cardAlignment(true);
            return true;
        }
        else
        {
            targetCards.ForEach(x=> x.GetComponent<Order>().SetMostFrontOrder(false));
            cardAlignment(true);
            return false;
        }
    }

    void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0;i < count; i++)
        {   
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void SetOriginOrderhide(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0;i < count; i++)
        {   
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(-1);
        }
    }

    public void ShowArea()
    {
        if (AttackArea.transform.localScale == new Vector3(2,2.5f,0))
            return;
        AttackArea.transform.DOScale(new Vector3(2,2.5f,0), 0.2f);
    }
    public void HideArea()
    {
        AttackArea.transform.DOScale(Vector3.zero, 0.2f);
    }

    void cardAlignment(bool isMine)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft,myCardRight, myCards.Count, 0.5f, Vector3.one * 1.9f);
       
        var targetCards = myCards;
        
        if (isMine)
        {
            int count = targetCards.Count;
            for(int i = 0; i < count;i++)
            {
                var targetCard = targetCards[i];

                targetCard.originPRS = originCardPRSs[i];
                targetCard.MoveTransform(targetCard.originPRS,true, 0.5f);
            }
        }
        else
        {
            int count = targetCards.Count;
            for(int i = 0; i < count;i++)
            {
                var targetCard = targetCards[i];

                targetCard.originPRS = originCardPRSs[i];
                targetCard.MoveTransform(targetCard.originPRS,false, 0);
            }
        }
    }





    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] {0.5f}; break;
            case 2: objLerps = new float[] {0.27f, 0.73f}; break;
            case 3: objLerps = new float[] {0.1f,0.5f, 0.9f}; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i=0; i<objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }
        
        for (int i = 0; i< objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height,2)-Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS( targetPos, targetRot,scale));
        }
        return results;
    }

    #region MyCard

    public void CardMouseOver(Card card)
    {
        if (eCardState == ECardState.Nothing || isMyCardDrag)
            return;

        selectCard = card;
        cardAlignment(false);
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {   
        if (eCardState != ECardState.CanMouseDrag || isOnArea)
            return;
        
        if(!isOnArea)
            ShowArea();
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
        if(eCardState != ECardState.CanMouseDrag || isOnArea)
            return;
        if(onSelectCardArea)
        {
            TryPutCard();
        }
        else
        {
            StageEntityManager.Inst.RemoveMyEmptyEntity();
            cardAlignment(true);   
        }

    }

    void CardDrag()
    {
        if (!onMyCardArea || isOnArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            if(onSelectCardArea)
                StageEntityManager.Inst.InsertmyEmptyEntity(Utils.MousePos.x);
            else
                StageEntityManager.Inst.RemoveMyEmptyEntity();
                
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        int layer_attack = LayerMask.NameToLayer("AttackArea");
        onSelectCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer_attack);
    }
    

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + 5.2f, -50f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 3.5f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (StageTurnManager.Inst.isLoading || StageTurnManager.Inst.isShowDeck || SceneManager.GetSceneByName("Pause").isLoaded)
            eCardState = ECardState.Nothing;
        else if (!StageTurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseOver;
        else if (StageTurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseDrag;
        
    }

    #endregion
}
