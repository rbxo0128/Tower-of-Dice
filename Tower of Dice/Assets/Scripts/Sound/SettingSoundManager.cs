using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSoundManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.Inst;
        musicSlider.value = soundManager.musicsource.volume*2;
        sfxSlider.value = soundManager.effectsource[0].volume;
    }
    
    public void SoundSetVolume(float volume)
    {
        soundManager.SetMusicVolume(volume);
    }

    public void SoundSetEffectVolume(float volume)
    {
        soundManager.SetEffectVolume(volume);
    }
    public void PlayEffect(int num)
    {
        soundManager.PlayEffect(num);
    }
}
