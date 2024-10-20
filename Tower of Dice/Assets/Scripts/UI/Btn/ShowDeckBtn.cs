using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ShowDeckBtn : MonoBehaviour
{
    [SerializeField] TMP_Text DeckCount;
    [SerializeField] TMP_Text TrashCount;

    public void DeckSetup(int deckCount, int trashCount)
    {
        DeckCount.text = deckCount.ToString();
        TrashCount.text = trashCount.ToString();
    }

    public void HideBtn()
    {
        this.gameObject.SetActive(false);
    }


    public void ShowBtn()
    {
        this.gameObject.SetActive(true);
    }
}
