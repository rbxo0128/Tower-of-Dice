using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class OtherPlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public static OtherPlayerAnimation Inst {get; private set;}

    public EnemySO enemySO;
    PlayData playData = PlayData.Inst;

    int randomIndex;
    int othernum;
    void Awake() {
        Inst = this;
    }

    void Start()
    {
        if (playData.myPlayer.stage < 21)
            randomIndex = Random.Range(0, 2);
        else if(playData.myPlayer.stage < 41)
            randomIndex = Random.Range(2, 4);
        else if(playData.myPlayer.stage < 61)
            randomIndex = Random.Range(4, enemySO.enemy.Length);

        
        if (playData.myPlayer.stage == 21)
        {
            animator.runtimeAnimatorController = enemySO.boss[0].enemyAnimator;
            othernum = -1;
        }
        else if (playData.myPlayer.stage == 41)
        {
            animator.runtimeAnimatorController = enemySO.boss[1].enemyAnimator;
            othernum = -2;
        }
        else if (playData.myPlayer.stage == 61)
        {
            animator.runtimeAnimatorController = enemySO.boss[1].enemyAnimator;
            othernum = -2;
        }
        else
        {     /////// 0빨간단 1늑대 2좀비 3고블린 4버섯 5 공룡
            othernum = randomIndex;
            animator.runtimeAnimatorController = enemySO.enemy[randomIndex].enemyAnimator; 
            
            if (enemySO.enemy[randomIndex].name == "빨간단") // 늑대 적
            {
                Entity a = animator.GetComponentInParent<Entity>();
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
            }

            else if (enemySO.enemy[randomIndex].name == "늑대") // 늑대 적
            {
                animator.transform.position = new Vector3(19, -1.9f, 0);
                Entity a = animator.GetComponentInParent<Entity>();
                a.originPos = new Vector3(19, -1.9f, 0);
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
            }
            else if (enemySO.enemy[randomIndex].name == "좀비") // 늑대 적
            {
                animator.transform.position = new Vector3(18.6f, -2.6f, 0);
                Entity a = animator.GetComponentInParent<Entity>();
                a.originPos = new Vector3(18.6f, -2.6f, 0);
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
            }

            else if (enemySO.enemy[randomIndex].name == "고블린") // 늑대 적
            {
                animator.transform.position = new Vector3(18.8f, -2.8f, 0);
                Entity a = animator.GetComponentInParent<Entity>();
                a.originPos = new Vector3(18.8f, -2.8f, 0);
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
                animator.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (enemySO.enemy[randomIndex].name == "버섯") // 늑대 적
            {
                animator.transform.position = new Vector3(19.4f, -2.8f, 0);
                Entity a = animator.GetComponentInParent<Entity>();
                a.originPos = new Vector3(19.4f, -2.8f, 0);
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
                animator.GetComponent<SpriteRenderer>().flipX = false;
                animator.transform.localScale = new Vector3(1.5f,1.5f,1);
            }

            else if (enemySO.enemy[randomIndex].name == "공룡") // 늑대 적
            {
                animator.transform.position = new Vector3(17.6f, -2.6f, 0);
                Entity a = animator.GetComponentInParent<Entity>();
                a.originPos = new Vector3(17.6f, -2.6f, 0);
                a.maxhealth = enemySO.enemy[randomIndex].maxhealth;
                a.health = enemySO.enemy[randomIndex].maxhealth;
                a.SetEnemyHP();
            }
            
        }
        
        if (playData.myPlayer.stage == 21) // 트롤 보스
        {
            animator.transform.position = new Vector3(18.4f, -0.62f, 0);
            Entity a = animator.GetComponentInParent<Entity>();
            a.originPos = new Vector3(18.4f, -0.62f, 0);
            a.transform.localScale = new Vector3(4,4,4);
            a.maxhealth = enemySO.boss[0].maxhealth;
            a.health = enemySO.boss[0].maxhealth;
            a.SetEnemyHP();
        }

        if (playData.myPlayer.stage == 41) // 닌자 보스
        {
            animator.transform.position = new Vector3(19, -1.85f, 0);
            Entity a = animator.GetComponentInParent<Entity>();
            a.originPos = new Vector3(19, -1.85f, 0);
            a.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            a.maxhealth = enemySO.boss[1].maxhealth;
            a.health = enemySO.boss[1].maxhealth;
            a.SetEnemyHP();
        }
    }
    public void OtherAttackAnimation()
    {
        animator.SetTrigger("IsOtherAttack");
    }

    public void OtherDieAnimation()
    {
        animator.SetTrigger("IsOtherDie");
    }

    public string GetOtherName()
    {
        if(othernum == -1)
            return "트롤";
        if(othernum == -2)
            return "닌자";

        return enemySO.enemy[othernum].name;
    }
}
