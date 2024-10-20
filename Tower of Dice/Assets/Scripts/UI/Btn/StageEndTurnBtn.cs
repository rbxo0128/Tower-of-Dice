using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageEndTurnBtn : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite inactive;
    [SerializeField] Text btnText;
    bool isMine;
    
    
    void Start()
    {
        StageTurnManager.OnTurnStarted += Mine;
    }

    void Update()
    {
        if (StageCardManager.Inst.isOnArea && isMine)
            Setup(true);
        else
            Setup(false);
    }

    void OnDestroy()
    {
        StageTurnManager.OnTurnStarted -= Mine;
    }
    void Mine(bool isMe)
    {
        isMine = isMe ? true : false;
    }

    public void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? active : inactive;
        GetComponent<Button>().interactable = isActive;
        btnText.color = isActive ? new Color32(255, 195, 90, 255) : new Color32(55,55,55,255);
    }
}
