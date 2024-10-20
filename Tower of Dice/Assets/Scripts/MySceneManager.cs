using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance {
        get {
            return instance;
        }
    }

    public CanvasGroup Fade_img;
    float fadeDuration = 2;
    
    public GameObject Loading;
    public TMP_Text Loading_text;

    
    private static MySceneManager instance;
 
    void Start () {
        if (instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
 
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoadaed;
    }

    private void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoadaed;
    }

    private void OnSceneLoadaed(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(()=>{
            Loading.SetActive(false);
        })
        .OnComplete(()=>{
            Fade_img.blocksRaycasts = false;
        });
    }

    public void ChangeScene(string sceneName)
    {
        Fade_img.DOFade(1, 1.0f)
        .OnStart(()=>{
            Fade_img.blocksRaycasts = true;
        })
        .OnComplete(()=>{
            StartCoroutine("LoadScene", sceneName);
        });
    } 

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while(!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if(percentage >= 90){
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if(percentage == 100){
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else{
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                yield return new WaitForSeconds(0.01f);
                if(percentage >= 90) past_time = 0;
            }
            Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기

            
        }
    }
}