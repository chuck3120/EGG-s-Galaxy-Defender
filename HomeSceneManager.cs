using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    public static string nextScene; // 다음 씬의 이름을 저장하는 정적 변수

    public void LoadCharacterSceneForShop()
    {
        nextScene = "ShopScene";
        SceneManager.LoadScene("Character");
    }

    public void LoadCharacterSceneForNormalMode()
    {
        nextScene = "NormalModeScene";
        SceneManager.LoadScene("Character");
    }

    public void LoadCharacterSceneForBossMode()
    {
        nextScene = "BossModeScene";
        SceneManager.LoadScene("Character");
    }
}
