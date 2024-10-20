using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Stage : MonoBehaviour
{
    public bool isStageLoad;
    public int difficulty;
    // 1: 적 2: 상점 3: 이벤트 4: 보스

    public Material[] materials;

    [SerializeField] GameObject D1;
    [SerializeField] GameObject LD1;
    [SerializeField] GameObject LD2;
    [SerializeField] GameObject LD3;
    [SerializeField] GameObject LD4;
    [SerializeField] GameObject L1;
    [SerializeField] GameObject LU1;
    [SerializeField] GameObject LU2;
    [SerializeField] GameObject LU3;
    [SerializeField] GameObject LU4;
    [SerializeField] GameObject U1;
    [SerializeField] GameObject RU1;
    [SerializeField] GameObject RU2;
    [SerializeField] GameObject RU3;
    [SerializeField] GameObject Ru4;
    [SerializeField] GameObject R1;
    [SerializeField] GameObject RD1;
    [SerializeField] GameObject RD2;
    [SerializeField] GameObject RD3;
    [SerializeField] GameObject RD4;
    [SerializeField] GameObject D1_Quad;
    [SerializeField] GameObject LD1_Quad;
    [SerializeField] GameObject LD2_Quad;
    [SerializeField] GameObject LD3_Quad;
    [SerializeField] GameObject LD4_Quad;
    [SerializeField] GameObject L1_Quad;
    [SerializeField] GameObject LU1_Quad;
    [SerializeField] GameObject LU2_Quad;
    [SerializeField] GameObject LU3_Quad;
    [SerializeField] GameObject LU4_Quad;
    [SerializeField] GameObject U1_Quad;
    [SerializeField] GameObject RU1_Quad;
    [SerializeField] GameObject RU2_Quad;
    [SerializeField] GameObject RU3_Quad;
    [SerializeField] GameObject Ru4_Quad;
    [SerializeField] GameObject R1_Quad;
    [SerializeField] GameObject RD1_Quad;
    [SerializeField] GameObject RD2_Quad;
    [SerializeField] GameObject RD3_Quad;
    [SerializeField] GameObject RD4_Quad;

    [SerializeField] Material enemyMaterial;
    [SerializeField] Material bossMaterial;

    public GameObject[] All;

    void Awake()
    {
        All = new GameObject[40];

        All[0] = D1;
        All[1] = LD1;
        All[2] = LD2;
        All[3] = LD3;
        All[4] = LD4;
        All[5] = L1;
        All[6] = LU1;
        All[7] = LU2;
        All[8] = LU3;
        All[9] = LU4;
        All[10] = U1;
        All[11] = RU1;
        All[12] = RU2;
        All[13] = RU3;
        All[14] = Ru4;
        All[15] = R1;
        All[16] = RD1;
        All[17] = RD2;
        All[18] = RD3;
        All[19] = RD4;
        All[20] = D1_Quad;
        All[21] = LD1_Quad;
        All[22] = LD2_Quad;
        All[23] = LD3_Quad;
        All[24] = LD4_Quad;
        All[25] = L1_Quad;
        All[26] = LU1_Quad;
        All[27] = LU2_Quad;
        All[28] = LU3_Quad;
        All[29] = LU4_Quad;
        All[30] = U1_Quad;
        All[31] = RU1_Quad;
        All[32] = RU2_Quad;
        All[33] = RU3_Quad;
        All[34] = Ru4_Quad;
        All[35] = R1_Quad;
        All[36] = RD1_Quad;
        All[37] = RD2_Quad;
        All[38] = RD3_Quad;
        All[39] = RD4_Quad;
    }
    public void SetupNew(int index) // 0: 적 1: 상점 2: 체력 회복 3: 보스 4:체력 감소 5:아이템 획득 6:버프 or 저주
    {
        int randomIndex1 = Random.Range(1, 5);
        int randomIndex2 = Random.Range(5, 10);
        int randomIndex3 = Random.Range(11, 15);
        int randomIndex4 = Random.Range(15, 20);
        int randomIndex5 = Random.Range(21, 25);
        int randomIndex6 = Random.Range(25, 30);
        int randomIndex7 = Random.Range(31, 35);
        int randomIndex8 = Random.Range(35, 40);
        int randomIndex9 = Random.Range(41, 45);
        int randomIndex10 = Random.Range(45, 50);
        int randomIndex11 = Random.Range(51, 55);
        int randomIndex12 = Random.Range(55, 60);
        List<int> possibleNumbers = new List<int> { 2, 4, 5, 6 };
        for (int i = 0; i < 60; i++)
        {
            int stageIndex;
            if(i%20 == 0)
                stageIndex = 3;
            else if(i %10 ==0)
                stageIndex = 1;
            else
                stageIndex = 0;

            if (i == randomIndex1)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex2)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex3)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex4)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex5)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex6)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex7)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex8)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex9)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex10)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex11)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }
            if (i == randomIndex12)
            {
                int random = Random.Range(0,4);
                stageIndex = possibleNumbers[random];
            }

            PlayData.Inst.myPlayer.stage_Quad.Add(stageIndex);       
        }
    }

    public void SetupLoad(int index)
    { 
        for (int i = 0; i < All.Length; i++)
        {
            Material selectedMaterial = materials[0];
            if (All[i].name.Contains("Quad"))
            {
                if (index == 0)
                    selectedMaterial = materials[PlayData.Inst.myPlayer.stage_Quad[i-20]]; // Quad 부분이 20 부터 이므로 i가 20 부터 시작
                else if (index == 1)
                    selectedMaterial = materials[PlayData.Inst.myPlayer.stage_Quad[i]];
                else if (index == 2)
                    selectedMaterial = materials[PlayData.Inst.myPlayer.stage_Quad[i+20]];
                
                Renderer renderer = All[i].GetComponent<Renderer>();
                renderer.material = selectedMaterial;
            }
        }
    }

    public IEnumerator LerpAndLoadNextStage(int nextStageIndex)
    {
        yield return new WaitForSeconds(1.2f);
        // Material을 연하게 만드는 효과 (페이드 아웃)
        yield return StartCoroutine(FadeMaterials());
        // 페이드 아웃이 완료된 후 다음 스테이지 로드
        SetupLoad(nextStageIndex);
        yield return StartCoroutine(FadeInMaterials());
    }

    private IEnumerator FadeMaterials()
    {
        float duration = 1.2f; // 페이드 효과 시간

        // 모든 오브젝트의 Renderer를 가져와서 각 오브젝트에 대해 개별적으로 Coroutine 실행
        for (int i = 0; i < All.Length; i++)
        {
            if (i== 0 || i == 20)
                continue;
            Renderer renderer = All[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                // 오브젝트마다 개별적으로 페이드 효과 적용
                StartCoroutine(FadeOut(renderer, duration));
            }
        }

        yield return new WaitForSeconds(duration); // 모든 오브젝트가 페이드 아웃되기를 기다림
    }

    // 개별 오브젝트에 대해 어두워지게 만드는 코루틴
    private IEnumerator FadeOut(Renderer renderer, float duration)
    {
        Color initialColor = renderer.material.color;
        Color fadedColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f); // 투명하게

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            renderer.material.color = Color.Lerp(initialColor, fadedColor, elapsed / duration);
            yield return null; // 한 프레임 대기
        }
    }

    // 모든 Material을 다시 밝게 만드는 함수
    private IEnumerator FadeInMaterials()
    {
        float duration = 1.2f; // 밝아지는 데 걸리는 시간

        // 모든 오브젝트의 Renderer를 가져와서 각 오브젝트에 대해 개별적으로 Coroutine 실행
        for (int i = 0; i < All.Length; i++)
        {
            Renderer renderer = All[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                // 오브젝트마다 개별적으로 밝아지게 만들기
                StartCoroutine(FadeIn(renderer, duration));
            }
        }

        yield return new WaitForSeconds(duration); // 모든 오브젝트가 페이드 인되기를 기다림
    }

    // 개별 오브젝트에 대해 밝아지게 만드는 코루틴
    private IEnumerator FadeIn(Renderer renderer, float duration)
    {
        Color fadedColor = renderer.material.color;
        Color initialColor = new Color(fadedColor.r, fadedColor.g, fadedColor.b, 1.0f); // 원래 색상

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            renderer.material.color = Color.Lerp(fadedColor, initialColor, elapsed / duration);
            yield return null; // 한 프레임 대기
        }
    }

    public void SetupAllShow()
    {
        for (int i = 0; i < All.Length; i++)
        {
            SetMaterialAlpha(All[i], 1f);
        }
    }
    public void SetupAllHide()
    {
        for (int i = 0; i < All.Length; i++)
        {
            SetMaterialAlpha(All[i], 0f);
        }
    }

    private void SetMaterialAlpha(GameObject obj, float alpha)
    {
        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                Color color = renderer.material.color;
                color.a = alpha;
                renderer.material.color = color;
            }
        }
    }
}
