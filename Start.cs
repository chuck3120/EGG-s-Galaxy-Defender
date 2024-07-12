using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
        // 씬을 전환하는 메서드
    public void LoadScene0()
    {
        SceneManager.LoadScene(0); // 씬0번을 로드
    }
}
