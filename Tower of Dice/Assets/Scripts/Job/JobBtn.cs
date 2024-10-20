using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JobBtn : MonoBehaviour
{
    public Button[] buttons;
    private Animator[] animators;
    private int selectedIndex = 0;
    PlayData playData = PlayData.Inst;

    void Start()
    {
        animators = new Animator[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;  // 로컬 변수를 사용하여 캡처
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
            animators[i] = buttons[i].GetComponentInChildren<Animator>();
        }

        selectedIndex = 0;
        animators[selectedIndex].SetTrigger("Select");
    }

    void OnButtonClicked(int index)
    {
        SelectButton(index);
    }

    void SelectButton(int index)
    {
        if (selectedIndex != index)
        {
            // 이전 버튼의 이미지를 Deselect
            animators[selectedIndex].SetTrigger("Deselect");

            // 새로운 버튼의 이미지를 Select
            selectedIndex = index;
            animators[selectedIndex].SetTrigger("Select");
        }
    }

    public void GoStage()
    {
        if (selectedIndex == 2)
            return;
        playData.myPlayer.job = selectedIndex;
        PlayData.Inst.New();
        SceneManager.LoadScene("Stage");
    }
}
