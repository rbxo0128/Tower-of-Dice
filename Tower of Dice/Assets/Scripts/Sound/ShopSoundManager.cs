using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSoundManager : MonoBehaviour
{
    SoundManager soundManager;
    void Start()
    {
        soundManager = SoundManager.Inst;
    }
    
    public void PlayEffect(int num)
    {
        soundManager.PlayEffect(num);
    }
}