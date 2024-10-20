using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    public GameObject effectsources;
    public AudioSource[] effectsource;
    public static SoundManager Inst { get; private set; }
    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }

        effectsource = effectsources.GetComponents<AudioSource>();
        SetMusicVolume(0.3f);
        SetEffectVolume(0.3f);

    }


    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume*0.5f;
    }

    public void SetEffectVolume(float volume)
    {
        for(int i =0; i<effectsource.Length; i++)
        {
            effectsource[i].volume = volume;
        }
        
    }

    public void PlayEffect(int num) // 0:card 1:coin 2:fromdeck 3:swing 4:swing 5:win 6:menu 7:dice 
    {
        effectsource[num].Play();
    }

    

    
}
