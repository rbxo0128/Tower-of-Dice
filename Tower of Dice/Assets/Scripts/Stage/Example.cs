using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System.Linq.Expressions;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class Example : MonoBehaviour
{
    //2.5 0.75
    [SerializeField] SpriteRenderer MyCharacter;
    public ScrollRect MapScroll;
    private Vector3 playerpos;

    void Start()
    {
        playerpos = MyCharacter.transform.position;
    }

    void Update()
    {
        playerpos = MyCharacter.transform.position;
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Sequence sequence = DOTween.Sequence()
            .Append(MyCharacter.transform.DOMove(new Vector3(playerpos.x - 1.5f, playerpos.y + 3f, playerpos.z), 0.8f)).SetEase(Ease.InQuad)
            .Append(MyCharacter.transform.DOMove(new Vector3(playerpos.x - 3, playerpos.y + 2.3f, playerpos.z), 0.8f)).SetEase(Ease.OutQuad)
            .OnComplete(()=>{});
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            MyCharacter.transform.DOJump(new Vector3(playerpos.x - 3, playerpos.y + 2.15f, playerpos.z), 1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            MyCharacter.transform.DOMove(new Vector3(playerpos.x + 3, playerpos.y, playerpos.z), 1.0f);
        }
    }
}
