using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public void ReplayGame() 
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.InitializeGame(); // GameManager 인스턴스 초기화
        }
        SceneManager.LoadScene("Home"); // "PlayScene" 대신 "Home" 씬으로 이동
    }
}
