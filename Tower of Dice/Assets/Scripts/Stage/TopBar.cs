using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    // Start is called before the first frame update
    public static TopBar Inst {get; private set;}

    [SerializeField] TMP_Text stageTMP;
    [SerializeField] TMP_Text levelTMP;
    [SerializeField] TMP_Text levelexpTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text healthhealTMP;
    [SerializeField] TMP_Text goldTMP;
    [SerializeField] TMP_Text goldearnTMP;
    
    PlayData playData = PlayData.Inst;

    float displayTime = 0.6f;
    private Color originalColor;

    void Start()
    {
        if (Inst != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        Inst = this;
        healthhealTMP.text = "";
        levelexpTMP.text = "";
        goldearnTMP.text = "";
        originalColor = healthhealTMP.color;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        stageTMP.text = playData.myPlayer.stage.ToString() + "F";
        healthTMP.text = $"체력 : {playData.myPlayer.health} / {playData.myPlayer.maxhealth}";
        levelTMP.text = $"레벨 : {playData.myPlayer.level}    {playData.myPlayer.exp} / {playData.myPlayer.maxexp}";
        goldTMP.text = playData.myPlayer.gold.ToString();
    }

    private Coroutine healCoroutine;

    public void HealParticle(int heal)
    {
        if (heal == 0)
            return;

        if(healCoroutine != null)
            StopCoroutine(healCoroutine);

        healCoroutine = StartCoroutine(Heal(heal));
    }

    IEnumerator Heal(int heal)
    {
        if (heal > 0)
        {
            healthhealTMP.text = "+ " + heal.ToString();
        }
        else if (heal < 0)
        {
            heal = -heal;
            healthhealTMP.text = "- " + heal.ToString();
        }
        healthhealTMP.color = originalColor;
        yield return new WaitForSeconds(displayTime);
        FadeOutText();
    }

    void FadeOutText()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        Color color = healthhealTMP.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;  // 알파 값 점차 줄이기
            healthhealTMP.color = color;
            yield return null;
        }
        healthhealTMP.text = "";  // 텍스트 사라지면 비우기
    }

    public void GoldParticle(int gold)
    {
        if (gold < 1)
            return;
        StartCoroutine(Gold(gold));
    }

    IEnumerator Gold(int gold)
    {
        goldearnTMP.text = "+ " + gold.ToString();
        goldearnTMP.color = originalColor;
        yield return new WaitForSeconds(displayTime);
        FadeOutGoldText();
    }

    void FadeOutGoldText()
    {
        StartCoroutine(FadeOutGoldCoroutine());
    }

    IEnumerator FadeOutGoldCoroutine()
    {
        Color color = goldearnTMP.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;  // 알파 값 점차 줄이기
            goldearnTMP.color = color;
            yield return null;
        }
        goldearnTMP.text = "";  // 텍스트 사라지면 비우기
    }

    public void ExpParticle(int exp)
    {
        if (exp < 1)
            return;
        StartCoroutine(Exp(exp));
    }

    IEnumerator Exp(int exp)
    {
        levelexpTMP.text = "+ " + exp.ToString();
        levelexpTMP.color = originalColor;
        yield return new WaitForSeconds(displayTime);
        FadeOutExpText();
    }

    void FadeOutExpText()
    {
        StartCoroutine(FadeOutExpCoroutine());
    }

    IEnumerator FadeOutExpCoroutine()
    {
        Color color = levelexpTMP.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;  // 알파 값 점차 줄이기
            levelexpTMP.color = color;
            yield return null;
        }
        levelexpTMP.text = "";  // 텍스트 사라지면 비우기
    }
}
