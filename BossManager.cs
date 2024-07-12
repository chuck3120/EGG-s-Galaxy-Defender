using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 사용하기 위해 필요

public class BossManager : MonoBehaviour
{
    public static BossManager instance = null; // 싱글톤 인스턴스 변수

    [Header("Boss Settings")]
    public GameObject[] bossPrefabs; // 보스 프리팹 배열
    public int totalStages = 10; // 총 스테이지 수
    public float baseHp = 1000f; // 기본 HP
    public float hpIncrement = 400f; // 스테이지당 HP 증가량
    public float baseFireRate = 2f; // 기본 발사 속도
    public float fireRateDecrement = 0.1f; // 스테이지당 발사 속도 감소량

    [Header("UI Settings")]
    public Text stageText; // 스테이지를 표시할 UI 텍스트
    public LevelUp levelUpUI; // 레벨 업 UI 스크립트 참조
    public Text timerText; // 타이머를 표시할 UI 텍스트

    [Header("Player Settings")]
    public float health; // 플레이어의 현재 체력
    public float maxHealth = 100; // 플레이어의 최대 체력

    private int currentBossIndex = 0; // 현재 보스 인덱스
    private GameObject currentBoss; // 현재 보스 인스턴스
    private float stageTimeLimit = 900f; // 각 스테이지의 시간 제한 (15분 = 900초)
    private float currentStageTime; // 현재 스테이지에서 경과한 시간

    void Awake()
    {
        // 싱글톤 패턴을 사용하여 인스턴스를 설정
        if (instance == null)
        {
            instance = this; // 인스턴스가 없으면 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 오브젝트가 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 오브젝트를 파괴
        }
    }

    void Start()
    {
        health = maxHealth; // 시작 시 체력을 최대 체력으로 설정
        SpawnNextBoss();
    }

    void Update()
    {
        if (currentStageTime > 0)
        {
            currentStageTime -= Time.deltaTime; // 경과 시간 감소
            UpdateTimerUI();

            if (currentStageTime <= 0)
            {
                GameOver();
            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentStageTime / 60f);
        int seconds = Mathf.FloorToInt(currentStageTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SpawnNextBoss()
    {
        StartCoroutine(ShowStageAndSpawnBoss());
    }

    IEnumerator ShowStageAndSpawnBoss()
    {
        // 스테이지 텍스트 업데이트 및 표시
        stageText.text = "Stage " + (currentBossIndex + 1);
        stageText.gameObject.SetActive(true);

        // 2초 동안 대기
        yield return new WaitForSeconds(2f);

        // 스테이지 텍스트 숨김
        stageText.gameObject.SetActive(false);

        // 보스 생성 인덱스 설정
        int bossIndex = currentBossIndex < totalStages ? currentBossIndex : Random.Range(0, bossPrefabs.Length);

        // 다음 보스 생성 (회전값을 설정하여 Z축을 180도로 회전)
        currentBoss = Instantiate(bossPrefabs[bossIndex], new Vector3(0, 7, 0), Quaternion.Euler(0, 0, 180));

        // 보스의 HP를 설정
        EnemyBoss boss = currentBoss.GetComponent<EnemyBoss>();
        if (boss != null)
        {
            boss.health = baseHp + (currentBossIndex * hpIncrement);
        }

        // 발사 속도를 설정할 수 있는 스크립트가 있는지 확인하고 설정
        BossFireController fireController = currentBoss.GetComponent<BossFireController>();
        if (fireController != null)
        {
            fireController.fireRate = Mathf.Max(0.1f, baseFireRate - (currentBossIndex * fireRateDecrement));
        }

        // 스테이지 타이머 초기화
        currentStageTime = stageTimeLimit;

        // 스테이지 인덱스 증가
        currentBossIndex++;
    }

    public void BossDefeated()
    {
        if (currentBoss != null)
        {
            Destroy(currentBoss);
            ShowLevelUpUI();
            currentStageTime = 0; // 타이머 정지
        }
    }

    void ShowLevelUpUI()
    {
        if (levelUpUI != null)
        {
            levelUpUI.Show();
        }
    }

    void GameOver()
    {
        // 게임 오버 로직 구현
        Debug.Log("Game Over! Time's up.");
        // 필요한 경우 게임 오버 화면 표시 등 추가 로직을 구현하세요.
    }
}
