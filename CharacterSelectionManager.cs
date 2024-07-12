using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public void StartGame()
    {
        // HomeSceneManager에서 설정한 nextScene으로 이동
        SceneManager.LoadScene(HomeSceneManager.nextScene);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Home");
    }
}
