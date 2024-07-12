using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] Missiles;
    [SerializeField] private GameObject specialMissile;
    private int missileIndex = 0;

    [SerializeField] private Transform shootTransForm;
    [SerializeField] private float shootInterval = 0.07f;
    public float lastShotTime = 0f;

    private bool isSpecialMissileActive = false;
    public float specialMissileDuration = 2f;
    private float specialMissileTimer = 0f;

    void Start()
    {
        InitializePlayer(); // 플레이어 초기화
    }

    void Update()
    {
        HandleMovement(); // 플레이어 이동 처리
        if (GameManager.instance != null && !GameManager.instance.isGameOver)
        {
            if (isSpecialMissileActive)
            {
                UpdateSpecialMissile(); // 특수 미사일 상태 업데이트
            }
            else
            {
                HandleShooting(); // 일반 미사일 발사 처리
            }
        }
    }

    // 플레이어 이동 처리
    void HandleMovement()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float toX = Mathf.Clamp(mousePos.x, -2f, 2f); // 플레이어 이동 범위 제한
        transform.position = new Vector3(toX, transform.position.y, transform.position.z);
    }

    // 일반 미사일 발사 처리
    void HandleShooting()
    {
        if (Time.time - lastShotTime > shootInterval)
        {
            Instantiate(Missiles[missileIndex], shootTransForm.position, Quaternion.Euler(0, 0, 90)); // 미사일 생성
            lastShotTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    // 특수 미사일 상태 업데이트
    private void UpdateSpecialMissile()
    {
        specialMissileTimer += Time.deltaTime; // 타이머 증가
        if (specialMissileTimer >= specialMissileDuration)
        {
            isSpecialMissileActive = false; // 특수 미사일 비활성화
            specialMissileTimer = 0f; // 타이머 초기화
        }
        else
        {
            SpecialMissile(); // 특수 미사일 발사
        }
    }

    // 특수 미사일 발사 처리
    private void SpecialMissile()
    {
        if (Time.time - lastShotTime > shootInterval)
        {
            Instantiate(specialMissile, shootTransForm.position, Quaternion.Euler(0, 0, 90)); // 특수 미사일 생성
            lastShotTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적 또는 보스와 충돌했을 때
        if (other.CompareTag("monster") || other.CompareTag("Boss") || other.CompareTag("BossMissile"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SetGameOver(); // 게임 오버 처리
            }
            Destroy(gameObject); // 플레이어 파괴
        }
        // 특수 코인과 충돌했을 때
        else if (other.CompareTag("FinisMoveCoin"))
        {
            ActivateSpecialMissile(); // 특수 미사일 활성화
            Destroy(other.gameObject); // 특수 코인 파괴
        }
        // 보스 코인과 충돌했을 때
        else if (other.CompareTag("BossCoin"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.IncreaseCoin(20); // 보스 코인 점수 증가
            }
            Destroy(other.gameObject); // 보스 코인 파괴
        }
    }

    // 특수 미사일 활성화
    private void ActivateSpecialMissile()
    {
        isSpecialMissileActive = true; // 특수 미사일 활성화
        specialMissileTimer = 0f; // 타이머 초기화
    }

    // 플레이어 업그레이드
    public void Upgrade()
    {
        if (!isSpecialMissileActive) // 특수 미사일 활성화 중이 아닐 때만 업그레이드
        {
            missileIndex += 1; // 미사일 인덱스 증가
            if (missileIndex >= Missiles.Length) // 배열 범위를 넘지 않도록 설정
            {
                missileIndex = Missiles.Length - 1; // 마지막 미사일 인덱스로 설정
            }
        }
    }

    // 플레이어 초기화
    private void InitializePlayer()
    {
        missileIndex = 0; // 미사일 인덱스 초기화
        lastShotTime = 0f; // 마지막 발사 시간 초기화
        isSpecialMissileActive = false; // 특수 미사일 비활성화
        specialMissileTimer = 0f; // 타이머 초기화
    }

    // 게임 오버 처리
    private void GameOver()
    {
        SceneManager.LoadScene("GameOverScene"); // 게임 오버 씬 로드
    }
}
