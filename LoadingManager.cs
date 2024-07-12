using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene; // 로드할 다음 씬의 이름을 저장하는 정적 변수

    [SerializeField]
    private Slider loadingSlider; // 로딩 슬라이더

    [SerializeField]
    private Image progressBar; // 로딩 진행을 표시하는 이미지

    [SerializeField]
    private Text tipText; // 팁 텍스트

    [SerializeField]
    private string[] tips; // 일반 팁 배열

    [SerializeField]
    private string rareTip = "You'll never be able to defeat the final boss!"; // 매우 낮은 확률로 나타나는 팁

    private void Start()
    {
        StartCoroutine(LoadSceneAsync(nextScene)); // 비동기적으로 다음 씬 로드 시작
        ShowRandomTip(); // 랜덤 팁 표시
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName; // 로드할 씬의 이름을 정적 변수에 저장
        SceneManager.LoadScene("LoadingScene"); // 로딩 씬을 로드
    }

    private void ShowRandomTip()
    {
        float rareTipProbability = 0.01f; // 희귀 팁이 나타날 확률 (1%)

        if (Random.value < rareTipProbability)
        {
            tipText.text = "Tip: " + rareTip; // 희귀 팁 설정
        }
        else
        {
            if (tips.Length > 0)
            {
                int randomIndex = Random.Range(0, tips.Length);
                tipText.text = "Tip: " + tips[randomIndex]; // 일반 팁 중 랜덤하게 설정
            }
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null; // 한 프레임 대기

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // 씬 자동 활성화 비활성화

        float timer = 0.0f;
        float displayTime = 2.0f; // 로딩 화면에 머무르는 시간
        while (!asyncOperation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            // 로딩 슬라이더와 프로그레스 바를 업데이트
            float progress = Mathf.Clamp01(timer / displayTime);
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }

            // 로딩이 90% 이상이 되었을 때와 타이머가 displayTime을 초과했을 때
            if (asyncOperation.progress >= 0.9f && timer >= displayTime)
            {
                asyncOperation.allowSceneActivation = true; // 씬 활성화 허용
                yield break;
            }
        }
    }
}
