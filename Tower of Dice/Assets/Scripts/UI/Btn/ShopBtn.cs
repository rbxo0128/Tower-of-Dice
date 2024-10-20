using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ShopBtn : MonoBehaviour
{
    [SerializeField] ImageCard stock;
    [SerializeField] UseItem stockItem;
    [SerializeField] TMP_Text shopprice;
    PlayData playData = PlayData.Inst;
    
    public void Buy()
    {
        if (playData.myPlayer.gold < stock.item.price)
            return;
        else
        {
            playData.myPlayer.gold -= stock.item.price;
            playData.myDeck.AddCard(stock.item);
            gameObject.SetActive(false);
            SoundManager.Inst.PlayEffect(1);

            // 카드 덱 추가 or 아이템 추가 이미지 품절 버튼 비활성화
        }
    }

    public void BuyItem()
    {
        if (playData.myPlayer.gold < stockItem.item.price)
            return;
        else
        {
            int count = 0;
            for (int i =0; i<playData.myPlayer.useItems.Count;i++)
            {
                if (playData.myPlayer.useItems[i].percent !=0)
                    count+=1;
            }
            
            if(count < 3)
            {
                SoundManager.Inst.PlayEffect(1);
                playData.myPlayer.gold -= stockItem.item.price;
                ItemManager.Inst.AddItem(stockItem.item.Copy());
                gameObject.SetActive(false);
                
            }
            else
                return;
                  
            // 카드 덱 추가 or 아이템 추가 이미지 품절 버튼 비활성화
        }
    }

    public void GotoStage()
    {
        playData.Save();
        MySceneManager.Instance.ChangeScene("Stage");
    }

    public void SetPrice(int price)
    {
        shopprice.text = price.ToString();
    }
}
