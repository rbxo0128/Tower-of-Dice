using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// 치트 ,UI, 게임오버. 랭킹
public class GameManager : MonoBehaviour
{
    public static GameManager Inst {get; private set;}
    void Awake() => Inst = this;

    public bool isOnDeck;

    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] ShowDeckPanel deckPanel;

    PlayData playData = PlayData.Inst;

    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            TurnManager.OnAddCard?.Invoke(true);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            TurnManager.OnAddCard?.Invoke(false);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            TurnManager.Inst.EndTurn();
        if (Input.GetKeyDown(KeyCode.Keypad4))
            CardManager.Inst.TryPutCard(false, true);    
    }
   
    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }

    public void DeckShow()
    {
        if (TurnManager.Inst.isAttacking || isOnDeck)
            return;
        isOnDeck = true;
        int deckCount = CardManager.Inst.itemBuffer.Count;
        int trashCount = CardManager.Inst.itemDeckBuffer.Count;
        CheckDeck.Inst.setCount(deckCount,trashCount);
        for(int i = 0; i < deckCount; i++)
        {
            CheckDeck.Inst.AddScroll(CardManager.Inst.itemBuffer[i], true);
        }
        for(int i = 0; i < trashCount; i++)
        {
            CheckDeck.Inst.AddScroll(CardManager.Inst.itemDeckBuffer[i], false);
        }
        deckPanel.Show();
    }
    public void DeckHide()
    {
        CheckDeck.Inst.Clear();
        deckPanel.Hide();
        isOnDeck = false;
    }
}
