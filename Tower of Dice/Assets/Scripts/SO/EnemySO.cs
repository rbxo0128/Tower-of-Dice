using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 [System.Serializable]
public class Enemy
{
    public string name;

    public int maxhealth;
    public RuntimeAnimatorController enemyAnimator;

    ///////////////////////////////////// 카드

    ////////////////////////////////////아이템
}

 [CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Object/EnemySO")]
public class EnemySO : ScriptableObject
{
    public Enemy[] enemy;
    public Enemy[] boss;

}
