using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public RuntimeAnimatorController[] playerAnimator;
    private Animator animator;
    public static PlayerAnimation Inst {get; private set;}
    PlayData playData = PlayData.Inst;
    void Awake() {
        Inst = this;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerAnimator[playData.myPlayer.job];

        if (playData.myPlayer.job == 1)
            transform.localScale = new Vector3(1,1,1);
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("IsAttack");
    }

    public void DieAnimation()
    {
        animator.SetTrigger("IsDie");
    }
}
