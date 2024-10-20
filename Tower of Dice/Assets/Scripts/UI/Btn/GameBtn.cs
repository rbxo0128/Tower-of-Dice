using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBtn : MonoBehaviour
{
    bool isPause;
    SoundManager soundManager = SoundManager.Inst;
    public void PauseGame()
    {
        if(SceneManager.GetSceneByName("Pause").isLoaded)
            return;
        else
        {
            soundManager.PlayEffect(6);
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        }
            
    }

    public void TitleGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        DestroyImmediate(PlayData.Inst.gameObject);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SettingGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.UnloadSceneAsync("Pause");
    }
}
