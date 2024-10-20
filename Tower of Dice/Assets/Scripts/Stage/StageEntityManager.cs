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

public class StageEntityManager : MonoBehaviour
{
    public static StageEntityManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] GameObject entityPrefab;
    [SerializeField] public List<Entity> myEntities;
    [SerializeField] Entity myEmptyEntity;
    [SerializeField] Transform Center;


    const int MAX_ENTITY_COUNT = 6;
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT &&  !ExistMyEmptyEntity;
    bool ExistMyEmptyEntity => myEntities.Exists(x=> x == myEmptyEntity);
    bool ExistMissingEntity => myEntities.Exists(x=> x == null);

    int MyEmptyEntityIndex => myEntities.FindIndex(x=> x==myEmptyEntity);
    int MyMissingIndex => myEntities.FindIndex(x=> x==null);

    bool CanMouseInput => StageTurnManager.Inst.myTurn && !StageTurnManager.Inst.isLoading;

    bool onMyCardArea;
    bool onAttackArea;

    public int limitCards;
    public int selectCards;

    Entity selectEntity;

    WaitForSeconds delay1 = new WaitForSeconds(0.5f);

    void Start()
    {
        
    }

    void OnDestroy()
    {
        
    }

    void Update()
    {
        DetectCardArea();
    }

    public bool CheckisOnArea()
    {
        selectCards = 0;
        for (int i=0; i< myEntities.Count; i++)
        {
            if(!(myEntities[i] == myEmptyEntity))
                selectCards++;
        }
        
        if (selectCards > 0)
            return true;
        else
            return false;
    }

    void EntityAlignment()
    {
        if(myEntities.Count == 1)
        {
            myEntities[0].transform.DOMove(Center.position, 0.4f);
            myEntities[0].GetComponent<Order>()?.SetOriginOrder(1);
        }
    }

    public int RollDice()
    {
        SoundManager.Inst.PlayEffect(7);
        int rand = Random.Range(0, 6);
        int[] spots = new int[] {myEntities[0].items.dicespot1, myEntities[0].items.dicespot2, myEntities[0].items.dicespot3, myEntities[0].items.dicespot4, myEntities[0].items.dicespot5, myEntities[0].items.dicespot6};
        myEntities[0].SetupSpot(myEntities[0].items, rand);

        return spots[rand];
    }

    public void InsertmyEmptyEntity(float xPos)
    {
        if (IsFullMyEntities)
            return;
        
        if (!ExistMyEmptyEntity)
        {
            myEntities.Add(myEmptyEntity);
        }
        
        Vector3 emptyEntityPos = myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        myEmptyEntity.transform.position = emptyEntityPos;

        //(isAttack ? myEntities : myDefendEntities).Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));

    }

    public void ShowAll(bool isShow)
    {
        try{
            myEntities[0].gameObject.SetActive(isShow);
        }
        catch{
            
        }
    }
    public void RemoveMyEmptyEntity()
    {
        if (!ExistMyEmptyEntity)
            return;

        else if(ExistMyEmptyEntity)
            myEntities.RemoveAt(MyEmptyEntityIndex);

        
        //EntityAlignment(true, true);
    }

    public bool SpawnEntity(bool isMine,Item item)
    {
        if(IsFullMyEntities)
            return false;

        var entityObject = Instantiate(entityPrefab, Utils.MousePos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        if (!ExistMyEmptyEntity)
        {
            DestroyImmediate(entity.gameObject);
            return false;
        }
        myEntities[MyEmptyEntityIndex] = entity;
        
        entity.isMine = isMine;
        entity.Setup(item);
        EntityAlignment();

        return true;

    }
    
    public void EntityMouseOver(Entity entity)
    {
        selectEntity = entity;
    }
    public void EntityMouseDown(Entity entity)
    {
        if (!CanMouseInput || SceneManager.GetSceneByName("Pause").isLoaded || StageTurnManager.Inst.isShowDeck)
            return;

        selectEntity = entity;
    }

    public void EntityMouseUp()
    {
        if (!CanMouseInput || SceneManager.GetSceneByName("Pause").isLoaded || StageTurnManager.Inst.isShowDeck)
            return;
        
        if (onAttackArea)
        {   
            TryPutCard(true, true);
        }

        else if (onMyCardArea)
        {
            StageCardManager.Inst.AddCardbyEntity(true,selectEntity.items);

            selectEntity.transform.DOKill();
            DestroyImmediate(selectEntity.gameObject);

            if(ExistMissingEntity)
                myEntities.RemoveAt(MyMissingIndex);
        }
        else
            RemoveMyEmptyEntity();

        selectEntity = null;

        EntityAlignment();
    }

    public void EntityMouseDrag()
    {
        if (!CanMouseInput || selectEntity == null)
            return;
        
        selectEntity.MoveTransform(new PRS(Utils.MousePos, Utils.QI, Vector3.one * 2.5f), false);
        RemoveMyEmptyEntity();

        if (onAttackArea)
        {
            InsertmyEmptyEntity(Utils.MousePos.x);
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        int layer_attack = LayerMask.NameToLayer("AttackArea");
        onAttackArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer_attack);
    }

    public bool TryPutCard(bool isMine, bool isAttack)
    {     
        var spawnPos = Utils.MousePos;
        Entity entity = selectEntity;
        if (SpawnEntity(isMine, entity.items))
        {
            selectEntity.transform.DOKill();
            DestroyImmediate(selectEntity.gameObject);
            
            if(ExistMissingEntity)
                myEntities.RemoveAt(MyMissingIndex);

            EntityAlignment();

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckTryCard()
    {
        if (selectCards==limitCards)
            return true;
        else
            return false;
    }

        
}
