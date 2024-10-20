using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowDeckPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public void Show()
    {
        if (SceneManager.GetSceneByName("Stage").isLoaded)
        {
            if(StageTurnManager.Inst.isLoading)
                return;
            else
            {
                StageTurnManager.Inst.isShowDeck = true;
            }
        }
        else
            TurnManager.Inst.isShowDeck = true;
        Sequence sequence = DOTween.Sequence()
            .AppendCallback(() => {
                ScaleOne();
            })
            .AppendInterval(0.9f);
    }

    public void Hide()
    {
        Sequence sequence = DOTween.Sequence()
        .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad))
        .AppendInterval(0.9f);
        if (SceneManager.GetSceneByName("Stage").isLoaded)
            StageTurnManager.Inst.isShowDeck = false;
        else
            TurnManager.Inst.isShowDeck = false;
    }

    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]
    void ScaleOne(){
        if (SceneManager.GetSceneByName("Stage").isLoaded)
            transform.DOScale(new Vector3(0.028f,0.028f,0), 0.3f);
        else
            transform.DOScale(Vector3.one, 0.3f);
    }

    [ContextMenu("ScaleZero")]
    public void ScaleZero() => transform.localScale = Vector3.zero;
}
