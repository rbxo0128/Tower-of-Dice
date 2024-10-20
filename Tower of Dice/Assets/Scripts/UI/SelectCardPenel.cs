using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardPenel : MonoBehaviour
{
    public static SelectCardPenel Inst {get; private set;}
    void Awake() => Inst = this;
    [SerializeField] Image card1;
    [SerializeField] Image card2;
    [SerializeField] Image card3;
    [SerializeField] GameObject secondPenel;
    [SerializeField] TMP_Text goldTMP;
    [SerializeField] TMP_Text expTMP;

    void Start()
    {
        transform.localScale = Vector3.zero;
        secondPenel.transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        transform.localScale = Vector3.one;
    }

    public void Next(int gold, int exp)
    {
        card1.transform.localScale = Vector3.zero;
        card2.transform.localScale = Vector3.zero;
        card3.transform.localScale = Vector3.zero;
        goldTMP.text = gold.ToString();
        expTMP.text = $"Exp +{exp}";
        secondPenel.transform.localScale = Vector3.one;
    }


}
