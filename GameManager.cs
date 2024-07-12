using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // 싱글톤 인스턴스 변수

    [SerializeField]
    private Text scoreText; // 현재 점수를 표시할 텍스트 (Legacy)

    [SerializeField]
    private Text stageText; // 현재 스테이지를 표시할 텍스트 (Legacy)

    [SerializeField]
    private Text warningText; // 경고 메시지를 표시할 텍스트 (Legacy)

    [SerializeField]
    private Text bonusScoreText; // 보너스 점수를 표시할 텍스트 (Legacy)

    public Slider bossHpBar; // 보스 체력 바

    private int coin = 0; // 현재 코인 수

    public bool isGameOver = false; // 게임 오버 상태 여부

    private bool isPaused = false; // 일시정지 상태 여부

    public int[] upgradeScores = { 20, 45, 70 }; // 업그레이드에 필요한 점수 배열

    private int upgradeIndex = 0; // 현재 업그레이드 인덱스

    [SerializeField]
    private GameObject victoryPanel; // 승리 패널

    public int currentStage; // 현재 스테이지

    private AudioSource backgroundMusic; // 배경 음악 AudioSource

    // 현재 코인 수를 설정하고 반환하는 프로퍼티
    public int Score
    {
        set => coin = Mathf.Max(0, value); // 코인을 설정하고 0보다 작아지지 않도록 한다
        get => coin; // 현재 코인 수를 반환
    }

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
        backgroundMusic = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
        InitializeGame(); // 게임 초기화
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때 호출될 메서드 등록
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬이 로드될 때 호출될 메서드 해제
    }

    // 씬이 로드될 때 호출되는 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 후 UI 요소들을 찾고 설정
        if (scoreText == null)
        {
            GameObject scoreTextObject = GameObject.Find("ScoreText"); // ScoreText 오브젝트 찾기
            if (scoreTextObject != null)
            {
                scoreText = scoreTextObject.GetComponent<Text>(); // ScoreText 오브젝트에서 Text 컴포넌트 가져오기
            }
            UpdateCoinText(); // 코인 텍스트 업데이트
        }

        if (stageText == null)
        {
            GameObject stageTextObject = GameObject.Find("StageText"); // StageText 오브젝트 찾기
            if (stageTextObject != null)
            {
                stageText = stageTextObject.GetComponent<Text>(); // StageText 오브젝트에서 Text 컴포넌트 가져오기
            }
        }

        if (warningText == null)
        {
            GameObject warningTextObject = GameObject.Find("WarningText"); // WarningText 오브젝트 찾기
            if (warningTextObject != null)
            {
                warningText = warningTextObject.GetComponent<Text>(); // WarningText 오브젝트에서 Text 컴포넌트 가져오기
            }
        }

        if (bonusScoreText == null)
        {
            GameObject bonusScoreTextObject = GameObject.Find("BonusScoreText"); // BonusScoreText 오브젝트 찾기
            if (bonusScoreTextObject != null)
            {
                bonusScoreText = bonusScoreTextObject.GetComponent<Text>(); // BonusScoreText 오브젝트에서 Text 컴포넌트 가져오기
            }
            bonusScoreText?.gameObject.SetActive(false); // BonusScoreText 오브젝트 숨기기
        }

        if (bossHpBar == null)
        {
            GameObject bossHpBarObject = GameObject.Find("BossHpBar"); // BossHpBar 오브젝트 찾기
            if (bossHpBarObject != null)
            {
                bossHpBar = bossHpBarObject.GetComponent<Slider>(); // BossHpBar 오브젝트에서 Slider 컴포넌트 가져오기
            }
        }

        // GameOver 씬이 로드된 경우 점수를 업데이트
        if (scene.name == "GameOverScene")
        {
            UpdateGameOverScores(); // GameOver 씬 로드 시 점수 업데이트
        }
    }

    public void IncreaseCoin(int amount = 1)
    {
        if (isGameOver) return; // 게임 오버 상태이면 반환

        coin += amount; // 코인 증가
        UpdateCoinText(); // 코인 텍스트 업데이트

        // 업그레이드 조건을 만족하면 플레이어를 업그레이드
        if (upgradeIndex < upgradeScores.Length && coin >= upgradeScores[upgradeIndex])
        {
            UpgradePlayer(); // 플레이어 업그레이드
            upgradeIndex++; // 업그레이드 인덱스 증가
        }
    }

    public void AddBonusScore(int bonus)
    {
        IncreaseCoin(bonus); // 보너스 점수만큼 코인 증가
        StartCoroutine(ShowBonusScoreMessage()); // 보너스 점수 메시지 표시 코루틴 시작
    }

    private IEnumerator ShowBonusScoreMessage()
    {
        if (bonusScoreText != null)
        {
            bonusScoreText.gameObject.SetActive(true); // 보너스 점수 텍스트 표시
            yield return new WaitForSeconds(1f); // 1초 대기
            bonusScoreText.gameObject.SetActive(false); // 보너스 점수 텍스트 숨김
        }
    }

    private void UpdateCoinText()
    {
        if (scoreText != null)
        {
            scoreText.text = coin.ToString(); // 코인 수를 텍스트로 표시
        }
    }

    private void UpgradePlayer()
    {
        Player playerInstance = FindObjectOfType<Player>(); // 플레이어 인스턴스 찾기
        if (playerInstance != null)
        {
            playerInstance.Upgrade(); // 플레이어 업그레이드
        }
    }

    public void SetGameOver()
    {
        if (isGameOver) return; // 이미 게임 오버 상태이면 반환

        isGameOver = true; // 게임 오버 설정
        StopMonsterSpawning(); // 몬스터 스포너 중지
        SaveBestScore(); // 최고 점수 저장
        LoadingSceneManager.LoadScene("GameOverScene"); // GameOver 씬 로드
    }

    private void StopMonsterSpawning()
    {
        MonsterSpawner monsterSpawner = FindObjectOfType<MonsterSpawner>(); // MonsterSpawner 인스턴스 찾기
        if (monsterSpawner != null)
        {
            monsterSpawner.StopMonsterRoutine(); // 몬스터 생성 루틴 중지
            monsterSpawner.StopSpawningMonsters(); // 몬스터 생성 중지
        }
    }

    public void BossDefeated()
    {
        if (currentStage == 17)
        {
            ShowVictory(); // 승리 패널 표시
        }
        else
        {
            FindObjectOfType<MonsterSpawner>().StartNextStage(); // 다음 스테이지 시작
        }
    }

    public void InitializeGame()
    {
        isGameOver = false; // 게임 오버 상태 초기화
        coin = 0; // 코인 초기화
        upgradeIndex = 0; // 업그레이드 인덱스 초기화
        currentStage = 1; // 현재 스테이지 초기화
        UpdateCoinText(); // 코인 텍스트 업데이트

        if (bonusScoreText != null)
        {
            bonusScoreText.gameObject.SetActive(false); // 보너스 점수 텍스트 숨김
        }

        if (bossHpBar != null)
        {
            bossHpBar.gameObject.SetActive(false); // 보스 체력 바 숨김
        }
    }

    private void SaveBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0); // 저장된 최고 점수 불러오기
        if (coin > bestScore) // 현재 코인이 최고 점수보다 높으면
        {
            PlayerPrefs.SetInt("BestScore", coin); // 최고 점수 갱신
        }
    }

    private void UpdateGameOverScores()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0); // 저장된 최고 점수 불러오기

        // 현재 점수와 최고 점수 텍스트 오브젝트 찾기 (TextMeshPro 사용)
        TextMeshProUGUI currentScoreText = GameObject.Find("CurrentScoreText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bestScoreText = GameObject.Find("BestScoreText")?.GetComponent<TextMeshProUGUI>();

        // 현재 점수 텍스트가 null이 아니면 텍스트 업데이트
        if (currentScoreText != null)
        {
            currentScoreText.text = coin.ToString(); // 현재 점수 텍스트 업데이트
        }

        // 최고 점수 텍스트가 null이 아니면 텍스트 업데이트
        if (bestScoreText != null)
        {
            bestScoreText.text = bestScore.ToString(); // 최고 점수 텍스트 업데이트
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // 일시정지 상태 토글
        if (isPaused)
        {
            Time.timeScale = 0f; // 시간 정지
            if (backgroundMusic != null)
            {
                backgroundMusic.Pause(); // 배경 음악 일시정지
            }
        }
        else
        {
            Time.timeScale = 1f; // 시간 재개
            if (backgroundMusic != null)
            {
                backgroundMusic.Play(); // 배경 음악 재생
            }
        }
    }

    public void UpdateStageText(string stage)
    {
        if (stageText != null)
        {
            stageText.text = stage; // 스테이지 텍스트 설정
            StartCoroutine(ShowStageText()); // 스테이지 텍스트 표시 코루틴 시작
        }
    }

    private IEnumerator ShowStageText()
    {
        stageText.gameObject.SetActive(true); // 스테이지 텍스트 표시
        yield return new WaitForSeconds(1f); // 1초 대기
        stageText.gameObject.SetActive(false); // 스테이지 텍스트 숨김
    }

    public IEnumerator UpdateStageTextDelayed(string stage, float delay)
    {
        yield return new WaitForSeconds(delay); // 지연 시간만큼 대기
        UpdateStageText(stage); // 스테이지 텍스트 업데이트
    }

    public void ShowWarningText()
    {
        if (warningText != null)
        {
            StartCoroutine(ShowWarningTextCoroutine()); // 경고 텍스트 표시 코루틴 시작
        }
    }

    private IEnumerator ShowWarningTextCoroutine()
    {
        warningText.gameObject.SetActive(true); // 경고 텍스트 표시
        yield return new WaitForSeconds(1f); // 1초 대기
        warningText.gameObject.SetActive(false); // 경고 텍스트 숨김
    }

    public void ShowBossHpBar(float maxHp)
    {
        if (bossHpBar != null)
        {
            bossHpBar.maxValue = maxHp; // 보스 체력 바 최대값 설정
            bossHpBar.value = maxHp; // 보스 체력 바 현재값 설정
            bossHpBar.gameObject.SetActive(true); // 보스 체력 바 표시
        }
    }

    public void HideBossHpBar()
    {
        if (bossHpBar != null)
        {
            bossHpBar.gameObject.SetActive(false); // 보스 체력 바 숨김
        }
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true); // 승리 패널 표시
        }
        Time.timeScale = 0f; // 게임 시간 정지
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause(); // 배경 음악 일시정지
        }
    }

    public void NextStage()
    {
        currentStage++; // 현재 스테이지 증가
        string stageText = "Stage " + currentStage; // 스테이지 텍스트 설정
        StartCoroutine(UpdateStageTextDelayed(stageText, 1f)); // 스테이지 텍스트 지연 후 업데이트 코루틴 시작
        FindObjectOfType<MonsterSpawner>().StartNextStage(); // MonsterSpawner에서 다음 스테이지 시작 메소드 호출
    }

    // 게임을 일시 정지하는 메서드
    public void Stop()
    {
        if (!isPaused) // 게임이 일시정지 상태가 아니면
        {
            TogglePause(); // 게임 일시 정지
        }
    }

    // 게임을 재개하는 메서드
    public void Resume()
    {
        if (isPaused) // 게임이 일시정지 상태이면
        {
            TogglePause(); // 게임 재개
        }
    }
}
