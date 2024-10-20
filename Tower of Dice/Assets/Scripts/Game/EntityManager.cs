using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using System.Threading;
using System;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] GameObject entityPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] List<Entity> myEntities;
    [SerializeField] List<Entity> myDefendEntities;
    [SerializeField] Entity myEmptyEntity;
    [SerializeField] Entity myBossEntity;
    [SerializeField] Entity otherBossEntity;
    [SerializeField] TMP_Text selectedCardsTMP;
    [SerializeField] TMP_Text limitCardsTMP;
    [SerializeField] SelectCardPenel selectCardPenel;
    [SerializeField] LosePenel losePenel;


    public Entity myEntity => myBossEntity;
    public Entity otherEntity => otherBossEntity;

    const int MAX_ENTITY_COUNT = 6;
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT &&  !ExistMyEmptyEntity;
    bool ExistMyEmptyEntity => myEntities.Exists(x=> x == myEmptyEntity);
    bool ExistMyDefendEmptyEntity => myDefendEntities.Exists(x=> x == myEmptyEntity);
    bool ExistMissingEntity => myEntities.Exists(x=> x == null);
    bool ExistMissingDEntity => myDefendEntities.Exists(x=> x == null);

    int MyEmptyEntityIndex => myEntities.FindIndex(x=> x==myEmptyEntity);
    int MyDefendEmptyEntityIndex => myDefendEntities.FindIndex(x=> x==myEmptyEntity);
    int MyMissingIndex => myEntities.FindIndex(x=> x==null);
    int MyMissingDIndex => myDefendEntities.FindIndex(x=> x==null);

    bool CanMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoading;

    bool onMyCardArea;
    bool onAttackArea;
    bool onDefendArea;
    bool buttonClicked;
    public bool isEndTurn;
    public int selectCards;

    Entity selectEntity;

    PlayData playData = PlayData.Inst;


    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
        limitCardsTMP.text = playData.myPlayer.limitCards.ToString();
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void OnTurnStarted(bool myTurn)
    {   
        if (!myTurn)
            StartCoroutine(AICo());   
    }

    void Update()
    {
        CardManager.Inst.isOnArea = myEntities.Count > 0 || myDefendEntities.Count >0;
        CheckSelectedCards();
        DetectCardArea();

        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            otherEntity.SetHP(1);
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            myEntity.SetHP(1);
    }


    IEnumerator AICo()
    {
        yield return true;
        //공격로직
        TurnManager.Inst.EndTurn();
    }

    void EntityAlignment(bool isMine, bool isAttack)
    {
        float targetY = 6.4f;
        var targetAttack = isAttack ? myEntities : myDefendEntities;

        if (isAttack)
        {
            for (int i=0; i< targetAttack.Count; i++)
            {
                float targetX = (targetAttack.Count-1) * -3.4f + i * 6.8f - 14;
                var targetEntity = targetAttack[i];
                targetEntity.originPos = new Vector3(targetX, targetY, 0);
                targetEntity.MoveTransform(targetEntity.originPos, true, 0.3f);
                targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
            }
        }
        else
        {
            for (int i=0; i< targetAttack.Count; i++)
            {
                float targetX = (targetAttack.Count-1) * -3.4f + i * 6.8f + 10;
                var targetEntity = targetAttack[i];
                targetEntity.originPos = new Vector3(targetX, targetY, 0);
                targetEntity.MoveTransform(targetEntity.originPos, true, 0.3f);
                targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
            }
        }

    }

    public void InsertmyEmptyEntity(float xPos, bool isAttack)
    {
        if (IsFullMyEntities)
            return;
        
        if (isAttack && !ExistMyEmptyEntity)
            myEntities.Add(myEmptyEntity);
        else if(!isAttack && !ExistMyDefendEmptyEntity)
            myDefendEntities.Add(myEmptyEntity);

        Vector3 emptyEntityPos = myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        myEmptyEntity.transform.position = emptyEntityPos;

        //(isAttack ? myEntities : myDefendEntities).Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));

    }

    public void RemoveMyEmptyEntity()
    {
        if (!ExistMyEmptyEntity && !ExistMyDefendEmptyEntity)
            return;

        else if(ExistMyEmptyEntity && !ExistMyDefendEmptyEntity)
            myEntities.RemoveAt(MyEmptyEntityIndex);

        else if(ExistMyDefendEmptyEntity && !ExistMyEmptyEntity)
            myDefendEntities.RemoveAt(MyDefendEmptyEntityIndex);

        else
        {
            myEntities.RemoveAt(MyEmptyEntityIndex);
            myDefendEntities.RemoveAt(MyDefendEmptyEntityIndex);
        }
        
        //EntityAlignment(true, true);
    }

    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos, bool isAttack)
    {
        
        if (isMine)
        {
            if(IsFullMyEntities)
                return false;
        }

        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        if (isAttack)
        {   
            if (!ExistMyEmptyEntity)
            {
                DestroyImmediate(entity.gameObject);
                return false;
            }
            myEntities[MyEmptyEntityIndex] = entity;
        }
        else if(!isAttack)
        {
            if (!ExistMyDefendEmptyEntity)
            {
                DestroyImmediate(entity.gameObject);
                return false;
            }
            myDefendEntities[MyDefendEmptyEntityIndex] = entity;
        }

        entity.isMine = isMine;
        entity.Setup(item);
        EntityAlignment(isMine, isAttack);

        return true;

    }
    
    public void EntityMouseOver(Entity entity)
    {
        selectEntity = entity;
    }
    public void EntityMouseDown(Entity entity)
    {
        if (!CanMouseInput || SceneManager.GetSceneByName("Pause").isLoaded || TurnManager.Inst.isShowDeck)
            return;

        selectEntity = entity;
    }

    public void EntityMouseUp()
    {
        if (!CanMouseInput || SceneManager.GetSceneByName("Pause").isLoaded || TurnManager.Inst.isShowDeck)
            return;
        
        if (onAttackArea)
        {   
            TryPutCard(true, true);
        }
        else if (onDefendArea)
            TryPutCard(true, false);

        else if (onMyCardArea)
        {
            CardManager.Inst.AddCardbyEntity(true,selectEntity.items);

            selectEntity.transform.DOKill();
            DestroyImmediate(selectEntity.gameObject);

            if(ExistMissingEntity)
                myEntities.RemoveAt(MyMissingIndex);
            
            if(ExistMissingDEntity)
                myDefendEntities.RemoveAt(MyMissingDIndex);
        }
        else
            RemoveMyEmptyEntity();

        selectEntity = null;

        EntityAlignment(true,true);
        EntityAlignment(true,false);
    }

    public void EntityMouseDrag()
    {
        if (!CanMouseInput || selectEntity == null)
            return;
        
        selectEntity.MoveTransform(new PRS(Utils.MousePos, Utils.QI, Vector3.one * 2.5f), false);
        RemoveMyEmptyEntity();

        if (onAttackArea)
        {
            Inst.InsertmyEmptyEntity(Utils.MousePos.x, true);
        }

        if (onDefendArea)
        {
            Inst.InsertmyEmptyEntity(Utils.MousePos.x, false);
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

    public bool TryPutCard(bool isMine, bool isAttack)
    {     
        var spawnPos = Utils.MousePos;
        Entity entity = selectEntity;
        if (SpawnEntity(isMine, entity.items, spawnPos, isAttack))
        {
            selectEntity.transform.DOKill();
            DestroyImmediate(selectEntity.gameObject);
            
            if(ExistMissingEntity)
                myEntities.RemoveAt(MyMissingIndex);
            
            if(ExistMissingDEntity)
                myDefendEntities.RemoveAt(MyMissingDIndex);

            EntityAlignment(true, true);
            EntityAlignment(true, false);

            return true;
        }
        else
        {
            //다시 돌아가게
            return false;
        }
    }

    public void CheckSelectedCards()
    {
        int selectedCards = 0;
        int count = myEntities.Count;
        int countd = myDefendEntities.Count;
        for(int i=0; i<count;i++)
        {
            if(!(myEntities[i] == myEmptyEntity))
                selectedCards++;
        }
        for(int i=0; i<countd; i++)
        {
            if(!(myDefendEntities[i] == myEmptyEntity))
                selectedCards++;
        }
        
        selectCards = selectedCards;
        selectedCardsTMP.text = selectedCards.ToString();
    }

    public bool CheckTryCard()
    {
        if (selectCards==playData.myPlayer.limitCards)
            return true;
        else
            return false;
    }

    void SpawnDamage(int damage, Transform tr)
    {
        var damageComponent = Instantiate(damagePrefab).GetComponent<Damage>();
        damageComponent.SetupTransform(tr);
        damageComponent.Damaged(damage);
    }

    public void itemDamaged(int damage)
    {
        TurnManager.Inst.isAttacking = true;
        otherEntity.Damaged(damage, 0);
        SpawnDamage(damage, otherEntity.transform);

        if (otherEntity.isDie)
        {   
            CardManager.Inst.SetupSelectCard();
            OtherPlayerAnimation.Inst.OtherDieAnimation();
            StartCoroutine(WinWait());
        }
        TurnManager.Inst.isAttacking = false;

    }



    // 좀더 간결하고 짧게 해야될듯 이해 하기 쉽지않음 나중에 보스 같은게 문제가 생길듯...
    public void Attack(Entity me, Entity enemy)
    {
        int attackSum = 0;
        int enemyattackSum = 0;
        int defendSum = 0;
        int enemydefendSum = 0;
        bool hasfour= false;
        
        isEndTurn = false;
        SoundManager.Inst.PlayEffect(7);
        for(int i=0; i<myEntities.Count; i++)
        {  
            int rand = Random.Range(0, 6);
            int[] spots = new int[] {myEntities[i].items.dicespot1, myEntities[i].items.dicespot2, myEntities[i].items.dicespot3, myEntities[i].items.dicespot4, myEntities[i].items.dicespot5, myEntities[i].items.dicespot6};
            myEntities[i].SetupSpot(myEntities[i].items, rand);
            attackSum += spots[rand];

            if(playData.myPlayer.stage == 41)
            {
                if(spots[rand] == 4)
                {
                    hasfour = true;
                }                   
            }
        }


        if(hasfour)
            attackSum = -1;

        if (ItemManager.Inst.criticalDamage)
        {
            attackSum *= 2;
            ItemManager.Inst.criticalDamage = false;
        }

        for(int i=0; i<myDefendEntities.Count; i++)
        {  
            int rand = Random.Range(0, 6);
            int[] spots = new int[] {myDefendEntities[i].items.dicespot1, myDefendEntities[i].items.dicespot2, myDefendEntities[i].items.dicespot3, myDefendEntities[i].items.dicespot4, myDefendEntities[i].items.dicespot5, myDefendEntities[i].items.dicespot6};
            myDefendEntities[i].SetupSpot(myDefendEntities[i].items, rand);
            defendSum += spots[rand];
            
        }

        (enemyattackSum, enemydefendSum) = CardManager.Inst.RollEnemyDice();

        if(!(attackSum==0)) // 플레이어가 공격이 가능할때
        {
            Sequence sequence = DOTween.Sequence() // 플레이어 공격
                .AppendInterval(1.0f)
                .AppendCallback(() =>
                {
                    me.SetShield(defendSum);
                    enemy.SetShield(enemydefendSum);
                })
                .AppendInterval(0.7f)
                .Append(me.transform.DOMove(new Vector3(enemy.originPos.x-4,me.originPos.y,enemy.originPos.z), 0.5f)).SetEase(Ease.InSine)
                .AppendCallback(() =>
                {
                    PlayerAnimation.Inst.AttackAnimation();
                })
                .AppendInterval(0.15f)
                .AppendCallback(() =>
                {
                    SoundManager.Inst.PlayEffect(4);
                })
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    enemy.Damaged(attackSum, enemydefendSum);
                    SpawnDamage(attackSum, enemy.transform);
                    
                    if (enemy.isDie)
                    {   
                        CardManager.Inst.SetupSelectCard();
                        OtherPlayerAnimation.Inst.OtherDieAnimation();
                        StartCoroutine(WinWait());
                    }
                    else
                    {
                        Sequence sequence2 = DOTween.Sequence()
                        .Append(enemy.transform.DOMove(new Vector3(enemy.originPos.x+2,enemy.originPos.y,enemy.originPos.z), 0.4f))
                        .Append(enemy.transform.DOMove(enemy.originPos, 0.4f))
                        .OnComplete(()=>{});
                    }
                })
                .AppendInterval(1.0f)
                .Append(me.transform.DOMove(me.originPos, 0.5f)).SetEase(Ease.InSine)
                .OnComplete(() => {            
                    if (!enemy.isDie)
                    {                                                         // 상대방 공격
                        Sequence sequence1 = DOTween.Sequence()
                            .Append(enemy.transform.DOMove(new Vector3(me.originPos.x+4,enemy.originPos.y,me.originPos.z), 0.1f))
                            .AppendCallback(() =>
                            {
                                SoundManager.Inst.PlayEffect(4);
                                OtherPlayerAnimation.Inst.OtherAttackAnimation();
                            })
                            .AppendInterval(0.37f)
                            .AppendCallback(() =>
                            {
                                me.Damaged(enemyattackSum, defendSum);
                                SpawnDamage(enemyattackSum, me.transform);
                                if (playData.myPlayer.stage == 21) // 트롤 보스 능력
                                {
                                    print(enemyattackSum-defendSum);
                                    enemy.health += enemyattackSum-defendSum;
                                    if (enemy.health > enemy.maxhealth)
                                        enemy.health = enemy.maxhealth;
                                    enemy.SetEnemyHP();
                                }
                                
                                if(me.isDie)
                                {
                                    PlayerAnimation.Inst.DieAnimation();
                                    StartCoroutine(LoseWait());
                                }
                                else
                                {
                                    Sequence sequence2 = DOTween.Sequence()
                                        .Append(me.transform.DOMove(new Vector3(me.originPos.x-2,me.originPos.y,me.originPos.z), 0.4f))
                                        .Append(me.transform.DOMove(me.originPos, 0.4f))
                                        .OnComplete(()=>{});
                                }
                                
                            })
                            .AppendInterval(1.0f)
                            .Append(enemy.transform.DOMove(enemy.originPos, 0.2f)).SetEase(Ease.InSine)
                            .AppendInterval(1.5f)
                            .OnComplete(()=>{
                                if(!me.isDie)
                                {
                                me.SetShield(0);
                                enemy.SetShield(0);
                                isEndTurn = true;
                                }
                                });
                                
                    }

                });
        }
        else // 플레이어 공격이 0 인경우 상대방만 공격
        {
            Sequence sequence = DOTween.Sequence()
                .AppendInterval(1.1f)
                .AppendCallback(() =>
                {
                    me.SetShield(defendSum);
                    enemy.SetShield(enemydefendSum);
                })
                .AppendInterval(1.5f)
                .OnComplete(() => {
                    Sequence sequence1 = DOTween.Sequence()
                        .Append(enemy.transform.DOMove(new Vector3(me.originPos.x+4,enemy.originPos.y,me.originPos.z), 0.2f))
                        .AppendCallback(() =>
                        {
                            SoundManager.Inst.PlayEffect(4);
                            OtherPlayerAnimation.Inst.OtherAttackAnimation();
                        })
                        .AppendInterval(0.37f)
                        .AppendCallback(() =>
                        {
                            me.Damaged(enemyattackSum, defendSum);
                            SpawnDamage(enemyattackSum, me.transform);
                            if (playData.myPlayer.stage == 21) // 트롤 보스 능력
                                {
                                    enemy.health += enemyattackSum-defendSum;
                                    if (enemy.health > enemy.maxhealth)
                                        enemy.health = enemy.maxhealth;
                                    enemy.SetEnemyHP();
                                }

                            if(me.isDie)
                            {
                                PlayerAnimation.Inst.DieAnimation();
                                StartCoroutine(LoseWait());
                            }
                            else
                            {
                                Sequence sequence2 = DOTween.Sequence()
                                    .Append(me.transform.DOMove(new Vector3(me.originPos.x-2,me.originPos.y,me.originPos.z), 0.4f))
                                    .Append(me.transform.DOMove(me.originPos, 0.4f))
                                    .OnComplete(()=>{});
                            }
                        })
                        .AppendInterval(1.0f)
                        .Append(enemy.transform.DOMove(enemy.originPos, 0.2f)).SetEase(Ease.InSine)
                        .AppendInterval(1.5f)
                        .OnComplete(()=>{
                            if(!me.isDie)
                            {
                            me.SetShield(0);
                            enemy.SetShield(0);
                            isEndTurn = true;
                            }
                            });
                });   
        }
}

    public void RemoveAll()
    {
        int count = myEntities.Count;
        int countd = myDefendEntities.Count;
        for (int i = 0; i< count; i++)
        {
            CardManager.Inst.itemDeckBuffer.Add(myEntities[0].items);
            DestroyImmediate(myEntities[0].gameObject);
            myEntities.RemoveAt(0);
        }
        
        for (int i = 0; i< countd; i++)
        {
            CardManager.Inst.itemDeckBuffer.Add(myDefendEntities[0].items);
            DestroyImmediate(myDefendEntities[0].gameObject);
            myDefendEntities.RemoveAt(0);
        }
    }

    IEnumerator WinWait()
    {
        yield return new WaitForSeconds(2f);

        

        selectCardPenel.Show();
        
        // 버튼 클릭을 기다립니다.
        yield return new WaitUntil(() => buttonClicked);
        selectCardPenel.Next(15, 6);
        buttonClicked = false;

        playData.myPlayer.gold+=15;
        TopBar.Inst.GoldParticle(15);
        
        playData.myPlayer.exp += 6;
        TopBar.Inst.ExpParticle(6);

        if (playData.myPlayer.exp >= playData.myPlayer.maxexp)
            playData.myPlayer.LevelUp(playData.myPlayer.exp - playData.myPlayer.maxexp);

        yield return new WaitUntil(() => buttonClicked);

        if (playData.myPlayer.stage == 21)
            playData.myPlayer.FirstBossClear = true;
        else if (playData.myPlayer.stage == 41)
            playData.myPlayer.SecondBossClear = true;
        else if (playData.myPlayer.stage == 61)
            playData.myPlayer.ThirdBossClear = true;
        
        playData.myPlayer.isBattle = true;
        playData.myPlayer.SetSeed();
        playData.Save();
        yield return new WaitForSeconds(0.6f);
        MySceneManager.Instance.ChangeScene("Stage");
    }

    IEnumerator LoseWait()
    {
        yield return new WaitForSeconds(2f);

        // 버튼 클릭을 기다립니다.
        //yield return new WaitUntil(() => buttonClicked);
        losePenel.Show();
        yield return new WaitUntil(() => buttonClicked);

        MySceneManager.Instance.ChangeScene("Main");
        yield return new WaitForSeconds(1f);
        playData.New();
    }

    public void OnButtonClicked()
    {
        // 이 메소드는 버튼의 onClick 이벤트와 연결되어야 합니다.
        buttonClicked = true;
    }

}
