using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventBtn : MonoBehaviour
{
    [SerializeField]TMP_Text eventTMP;
    [SerializeField]TMP_Text eventTMP1;
    [SerializeField]UseItemSO useItemSO;
    PlayData playData = PlayData.Inst;

    void Start() // 2:체력회복 4:체력감소 5:아이템 6:버프너프
    {
        if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 2)
        {
            eventTMP.text = "힐";
            
            int healamount = playData.myPlayer.maxhealth/4;
            eventTMP1.text = "체력 회복 +" + healamount.ToString();
            playData.myPlayer.health += healamount;
            if (playData.myPlayer.health > playData.myPlayer.maxhealth)
            {
                playData.myPlayer.health = playData.myPlayer.maxhealth;
                healamount = playData.myPlayer.maxhealth - playData.myPlayer.health;
                print(healamount);
            }
            TopBar.Inst.HealParticle(healamount);
        }

        else if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 4)
        {
            eventTMP.text = "폭탄";
            int healamount = -playData.myPlayer.maxhealth/4;
            eventTMP1.text = "체력 감소 " + healamount.ToString();
            playData.myPlayer.health += healamount;
            if (playData.myPlayer.health < 0)
            {
                playData.myPlayer.health = 0;
            }
            TopBar.Inst.HealParticle(healamount);
        }

        if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 5)
        {
            eventTMP.text = "보물 상자";
            eventTMP1.text = "무작위 아이템 획득";
            int count = 0;

            for (int i =0; i<playData.myPlayer.useItems.Count;i++)
            {
                if (playData.myPlayer.useItems[i].percent !=0)
                    count+=1;
            }

            if(count < 3)
            {
                int random = Random.Range(0,useItemSO.useItem.Length-1);
                ItemManager.Inst.AddItem(useItemSO.useItem[random].Copy());
            }
            
        }

        if (playData.myPlayer.stage_Quad[playData.myPlayer.stage-1] == 6)
        {
            int random = Random.Range(0,3);
            random = 0;
            eventTMP.text = "이벤트";
            if (random == 0)
            {
                eventTMP1.text = "경험치 획득 +10" ;
                playData.myPlayer.exp += 10;
                TopBar.Inst.ExpParticle(10);

                if (playData.myPlayer.exp >= playData.myPlayer.maxexp)
                    playData.myPlayer.LevelUp(playData.myPlayer.exp - playData.myPlayer.maxexp);
            }

            else if (random == 1)
            {
                eventTMP1.text = "버프 획득 (카드 드로우 증가)";
            }
            
        }
    }
    public void StageGame()
    {
        SceneManager.UnloadSceneAsync("Event");
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }

}
