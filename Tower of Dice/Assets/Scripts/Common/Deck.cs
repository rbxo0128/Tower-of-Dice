using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public ItemSO itemSO;
    public List<Item> myDecks;
    
    
    public void SetupDeck()
    {
        myDecks.Clear();
        itemSO = PlayData.Inst.Getitem();
        for(int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            for (int j = 0; j < item.percent; j++)
                myDecks.Add(item);
        }


        for(int i =0; i<myDecks.Count; i++)
        {
            int rand = Random.Range(i, myDecks.Count);
            Item temp = myDecks[i];
            myDecks[i] = myDecks[rand];
            myDecks[rand] = temp;
        }
    }

    public void AddCard(Item item)
    {
        myDecks.Add(item);
    }

}
