using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] Card Attack;
    [SerializeField] Entity AttackEnt;
    [SerializeField] Card Defend;
    [SerializeField] Entity DefendEnt;
    
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] ECardState eCardState;
    [SerializeField] ShowDeckBtn showDeckbtn;
    [SerializeField] SpriteRenderer AttackArea;
    [SerializeField] SpriteRenderer DefendArea;

    public List<ImageCard> selectWinCard;


    public List<Item> itemBuffer;
    public List<Item> itemDeckBuffer;
    public List<Item> itemEnemyBuffer;
    public List<Item> itemAddBuffer;
    Card selectCard;
    public bool isOnArea;
    bool isMyCardDrag;
    bool onMyCardArea;
    bool onAttackArea;
    bool onDefendArea;
    enum ECardState {Nothing, CanMouseOver, CanMouseDrag}
    int myPutCount;
    PlayData playData = PlayData.Inst;

    public Item PopItem(bool isMine)
    {
        if(isMine)
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
        else
        {
            if (itemEnemyBuffer.Count == 0)
                SetupEnemyItemBuffer();
            
            Item item = itemEnemyBuffer[0];
            itemEnemyBuffer.RemoveAt(0);
            return item;
        }
    }

    void SetupItemBuffer()
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

    void SetupEnemyItemBuffer()
    {
        string othername = OtherPlayerAnimation.Inst.GetOtherName();
        if(othername == "빨간단")
        {
            for(int i = 0; i < itemSO.enemyitems.Length; i++)
            {
                Item item = itemSO.enemyitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "늑대")
        {
            for(int i = 0; i < itemSO.wolfitems.Length; i++)
            {
                Item item = itemSO.wolfitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "버섯")
        {
            for(int i = 0; i < itemSO.mushroomitems.Length; i++)
            {
                Item item = itemSO.mushroomitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "고블린")
        {
            for(int i = 0; i < itemSO.goblinitems.Length; i++)
            {
                Item item = itemSO.goblinitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "좀비")
        {
            for(int i = 0; i < itemSO.zombieitems.Length; i++)
            {
                Item item = itemSO.zombieitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "공룡")
        {
            for(int i = 0; i < itemSO.dinoitems.Length; i++)
            {
                Item item = itemSO.dinoitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "트롤") // 임시 보스
        {
            for(int i = 0; i < itemSO.dinoitems.Length; i++)
            {
                Item item = itemSO.dinoitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        else if(othername == "닌자")// 임시 보스
        {
            for(int i = 0; i < itemSO.dinoitems.Length; i++)
            {
                Item item = itemSO.dinoitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }
        else // 임시 보스
        {
            for(int i = 0; i < itemSO.zombieitems.Length; i++)
            {
                Item item = itemSO.zombieitems[i];
                for (int j = 0; j < item.percent; j++)
                    itemEnemyBuffer.Add(item);
            }
        }

        


        for(int i =0; i<itemEnemyBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemEnemyBuffer.Count);
            Item temp = itemEnemyBuffer[i];
            itemEnemyBuffer[i] = itemEnemyBuffer[rand];
            itemEnemyBuffer[rand] = temp;
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
        SetupItemBuffer();
        itemDeckBuffer = new List<Item>();
        itemEnemyBuffer = new List<Item>();
        //SetupItemBuffer();
        AttackArea.transform.localScale = Vector3.zero;
        DefendArea.transform.localScale = Vector3.zero;
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
        
    }

    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void OnTurnStarted(bool myTurn)
    {
        if (myTurn)
            myPutCount = 0;
    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        if (isOnArea)
            ShowArea();

        else if(!isOnArea && !isMyCardDrag)
            HideArea();

        showDeckbtn.DeckSetup(itemBuffer.Count, itemDeckBuffer.Count); // 덱 개수 확인 여기에 넣어도 되는지 몰?루 렉 유발하면 뺄 생각 해야될듯
        DetectCardArea();
        SetECardState();
    }

    public void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(isMine),isMine);
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(true);
        cardAlignment(true);
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

    void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0;i < count; i++)
        {   
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
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

    public bool TryPutCard(bool isMine, bool isAttack)
    {
        if (isMine && myPutCount >=1)
            return false;
        
        Card card = selectCard;
        var spawnPos = Utils.MousePos;
        var targetCards = myCards;

        if (EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos, isAttack))
        {
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);

            if(isMine)
            {
                selectCard = null;
                //myPutCount++;
            }

            EntityManager.Inst.CheckSelectedCards();
            cardAlignment(isMine);
            return true;
        }
        else
        {
            targetCards.ForEach(x=> x.GetComponent<Order>().SetMostFrontOrder(false));
            cardAlignment(isMine);
            return false;
        }
    }

    public void removeAll()
    {
        int count = myCards.Count;

        for (int i = 0; i<count; i++)
        {
            itemDeckBuffer.Add(myCards[0].item);
            DestroyImmediate(myCards[0].gameObject);
            myCards.RemoveAt(0);
        }

        int countE = otherCards.Count;

        for (int i = 0; i<countE; i++)
        {
            DestroyImmediate(otherCards[0].gameObject);
            otherCards.RemoveAt(0);
        }
        AttackEnt.SetupNull();
        DefendEnt.SetupNull();
    }

    public void removeAlignment()
    {
        Vector3 pos = new Vector3(-19,-10,0);
        Quaternion rot = Quaternion.Euler(0,0,1);
        Vector3 scale = new Vector3(1.5f,1.5f,1.5f);
        PRS remove = new PRS(pos,rot,scale);
        
        for(int i = 0; i < myCards.Count;i++)
        {
            myCards[i].MoveTransform(remove,true, 0.7f);
        }
    }

    public void SetupEnemy()
    {
        Attack.Setup(otherCards[0].item, false);
        Defend.Setup(otherCards[1].item, false);
    }

    public void SetupSelectCard()
    {
        for(int i = 0; i < itemSO.addItems.Length; i++)
        {
            Item item = itemSO.addItems[i];
            for (int j = 0; j < item.percent; j++)
                itemAddBuffer.Add(item);
        }


        for(int i =0; i<itemAddBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemAddBuffer.Count);
            Item temp = itemAddBuffer[i];
            itemAddBuffer[i] = itemAddBuffer[rand];
            itemAddBuffer[rand] = temp;
        }

        for (int i = 0; i< selectWinCard.Count; i++)
        {
            selectWinCard[i].SetupDeck(itemAddBuffer[i]);
        }
    }

    public void ClickCard1()
    {
        playData.myDeck.AddCard(selectWinCard[0].item);
    }
    public void ClickCard2()
    {
        playData.myDeck.AddCard(selectWinCard[1].item);
    }
    public void ClickCard3()
    {
        playData.myDeck.AddCard(selectWinCard[2].item);
    }

    public (int, int) RollEnemyDice()
    {
        int enemyAttackSum = 0;
        int enemyDefendSum = 0;

        AttackEnt.Setup(Attack.item);
        DefendEnt.Setup(Defend.item);
        
        for(int i=0; i<2; i++)
        {  
            int rand = Random.Range(0, 6);
            int[] spots = new int[] {otherCards[i].item.dicespot1,otherCards[i].item.dicespot2,otherCards[i].item.dicespot3,otherCards[i].item.dicespot4,otherCards[i].item.dicespot5,otherCards[i].item.dicespot6};
            if (i==0)
            {
                AttackEnt.SetupSpot(Attack.item,rand);
                enemyAttackSum += spots[rand];
            }
            else
            {
                DefendEnt.SetupSpot(Defend.item, rand);
                enemyDefendSum += spots[rand];
            }
        }

        return (enemyAttackSum, enemyDefendSum);
    }

    public void ShowArea()
    {
        if (AttackArea.transform.localScale == new Vector3(2,2,2))
            return;
        AttackArea.transform.DOScale(new Vector3(2,2,2), 0.2f);
        DefendArea.transform.DOScale(new Vector3(2,2,2), 0.2f);
    }
    public void HideArea()
    {
        AttackArea.transform.DOScale(Vector3.zero, 0.2f);
        DefendArea.transform.DOScale(Vector3.zero, 0.2f);
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

    public void CardEnemyMouseOver(Card card)
    {
        if (eCardState == ECardState.Nothing || isMyCardDrag)
            return;

        selectCard = card;
        EnlargeEnemyCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;
        EnlargeCard(false, card);
    }

    public void CardEnemyMouseExit(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;
        EnlargeEnemyCard(false, card);
    }

    public void CardMouseDown()
    {   
        if (eCardState != ECardState.CanMouseDrag)
            return;
        if(!isOnArea)
            ShowArea();
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
        if(EntityManager.Inst.CheckTryCard() || eCardState != ECardState.CanMouseDrag)
            return;

        if (onAttackArea)
        {   
            TryPutCard(true, true);
        }
        else if (onDefendArea)
            TryPutCard(true, false);
        else
        {
            EntityManager.Inst.RemoveMyEmptyEntity();
            cardAlignment(true);
        }
    }

    void CardDrag()
    {
        if(EntityManager.Inst.CheckTryCard())
            return;
        
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);

            if (onAttackArea)
            {
                EntityManager.Inst.InsertmyEmptyEntity(Utils.MousePos.x, true); 
            }

            else if (onDefendArea)
            {
                EntityManager.Inst.InsertmyEmptyEntity(Utils.MousePos.x, false);
            }
            else
                EntityManager.Inst.RemoveMyEmptyEntity();
            
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        int layer_attack = LayerMask.NameToLayer("AttackArea");
        onAttackArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer_attack);
        int layer_defend = LayerMask.NameToLayer("DefendArea");
        onDefendArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer_defend);
    }
    

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -4.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 3.5f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void EnlargeEnemyCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = card.gameObject.transform.position;
            card.Setup(card.item, true);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.7f), false);
        }
        else
        {
            card.Setup(card.item, false);
            card.MoveTransform(new PRS(card.gameObject.transform.position, card.gameObject.transform.rotation, Vector3.one * 2.0f), false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading || TurnManager.Inst.isShowDeck || TurnManager.Inst.isAttacking || SceneManager.GetSceneByName("Pause").isLoaded)
            eCardState = ECardState.Nothing;
        else if (!TurnManager.Inst.myTurn || myPutCount == 1 || EntityManager.Inst.IsFullMyEntities)
            eCardState = ECardState.CanMouseOver;
        else if (TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseDrag;
        
    }

    #endregion
}
