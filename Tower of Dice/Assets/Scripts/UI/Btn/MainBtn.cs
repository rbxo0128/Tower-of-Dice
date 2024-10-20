using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBtn : MonoBehaviour
{
    [SerializeField] GamePanel gamePanel;
    [SerializeField] Button loadBtn;
    [SerializeField] TMP_Text loadBtnTMP;

    PlayData playData;
    SoundManager soundManager;

    void Start()
    {
        if (SceneManager.GetSceneByName("Main").isLoaded)
        {
            if (TopBar.Inst != null)
                DestroyImmediate(TopBar.Inst.gameObject);
        }

        playData = PlayData.Inst;
        soundManager = SoundManager.Inst;
    }

    public void ExitGame()
    {
        if (SceneManager.GetSceneByName("Setting").isLoaded)
            return;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StartGame()
    {
        gamePanel.Show();
        soundManager.PlayEffect(6);
        if(playData.myPlayer.stage == 1)
        {
            loadBtn.GetComponent<Button>().interactable = false;
            Color color = loadBtnTMP.color;
            color.a = 0.5f;
            loadBtnTMP.color = color;
        }
        
    }

    public void HideGame()
    {
        gamePanel.Hide();
        soundManager.PlayEffect(6);
    }

    public void NewGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.LoadScene("Job");
    }

    public void LoadGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.LoadScene("Stage");
    }

    public void SettingGame()
    {
        soundManager.PlayEffect(6);
        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
    }

    public void CloseSetting()
    {
        soundManager.PlayEffect(6);
        SceneManager.UnloadSceneAsync("Setting");
    }
}
