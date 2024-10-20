using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePenel : MonoBehaviour
{
    public static LosePenel Inst {get; private set;}
    void Awake() => Inst = this;

    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        transform.localScale = Vector3.one;
    }
}
