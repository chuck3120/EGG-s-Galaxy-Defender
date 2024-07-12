using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsPanel; // 설정 UI 패널

    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // 시작 시 설정 패널 숨기기
        }
    }

    // 보스 모드 버튼 클릭 시 호출되는 메서드
    public void OnBossModeButton()
    {
        SceneManager.LoadScene("BossModeScene"); // 보스 모드 씬으로 이동
    }

    // 노말 모드 버튼 클릭 시 호출되는 메서드
    public void OnNormalModeButton()
    {
        SceneManager.LoadScene("PlayScene"); // 플레이 씬으로 이동
    }

    // 숍 버튼 클릭 시 호출되는 메서드
    public void OnShopButton()
    {
        SceneManager.LoadScene("ShopScene"); // 숍 씬으로 이동
    }

    // 설정 버튼 클릭 시 호출되는 메서드
    public void OnSettingsButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // 설정 패널 표시
        }
    }

    // 설정 패널 닫기 버튼 클릭 시 호출되는 메서드
    public void OnCloseSettingsButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // 설정 패널 숨기기
        }
    }
}
