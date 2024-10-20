using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Damage : MonoBehaviour
{
    [SerializeField] TMP_Text damageTMP;
    Transform tr;

    public void SetupTransform(Transform tr)
    {
        this.tr = tr;
    }

    void Update()
    {
        if (tr != null)
            transform.position = tr.position;
    }

    public void Damaged(int damage) // 나중에 공겨 데미지를 공격 - 방어도가 아니라 방어도도 이미지로 띄운뒤 공격 하는 데미지를 띄울거라 0 데미지도 있을것
    {
        GetComponent<Order>().SetOrder(1000);
        if (damage == -1)
        {
            damageTMP.text = "Miss";
        }
        else
            damageTMP.text = $"-{damage}";

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one * 7f, 0.5f).SetEase(Ease.InOutBack))
            .AppendInterval(0.5f)
            .Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack))
            .OnComplete(() => Destroy(gameObject));
    }

    
}

