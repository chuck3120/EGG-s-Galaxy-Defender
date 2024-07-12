using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossModePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] Missiles;
    [SerializeField] private Transform shootTransForm;
    [SerializeField] private float shootInterval = 0.07f;
    [SerializeField] private Transform[] gearPositions; // 보조무기의 위치 배열

    public Gear gear; // 보조무기 스크립트 참조
    private int missileIndex = 0;
    private float lastShotTime = 0f;

    private ItemData.AttributeType currentAttribute;

    // 추가된 속성
    public float speed;
    public int maxHealth;
    public int health;
    public float missileSpeed;
    public float baseMissileDamage; // 미사일의 기본 데미지

    void Start()
    {
        InitializePlayer(); // 플레이어 초기화
    }

    void Update()
    {
        HandleMovement(); // 플레이어 이동 처리
        if (GameManager.instance != null && !GameManager.instance.isGameOver)
        {
            HandleShooting(); // 미사일 발사 처리
        }
    }

    void HandleMovement()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float toX = Mathf.Clamp(mousePos.x, -2f, 2f); // 플레이어 이동 범위 제한
        transform.position = new Vector3(toX, transform.position.y, transform.position.z);
    }

    void HandleShooting()
    {
        if (Time.time - lastShotTime > shootInterval)
        {
            GameObject missileObject = Instantiate(Missiles[missileIndex], shootTransForm.position, Quaternion.Euler(0, 0, 90));
            MissileController missileController = missileObject.GetComponent<MissileController>();
            missileController.attributeType = currentAttribute;
            missileController.damage = baseMissileDamage; // 기본 데미지 설정
            lastShotTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 여기에 충돌 처리 로직 추가
    }

    public void Upgrade()
    {
        missileIndex += 1; // 미사일 인덱스 증가
        if (missileIndex >= Missiles.Length) // 배열 범위를 넘지 않도록 설정
        {
            missileIndex = Missiles.Length - 1; // 마지막 미사일 인덱스로 설정
        }
    }

    private void InitializePlayer()
    {
        missileIndex = 0; // 미사일 인덱스 초기화
        lastShotTime = 0f; // 마지막 발사 시간 초기화

        // 초기값 설정
        health = maxHealth;
        speed = moveSpeed;
        missileSpeed = shootInterval;
        baseMissileDamage = 10f; // 초기 미사일 기본 데미지 설정 (예: 10)
    }

    public void ActivateGear(ItemData gearData)
    {
        if (gear == null)
        {
            gear = gameObject.AddComponent<Gear>();
            gear.Init(gearData, gearPositions);
        }
        else
        {
            gear.LevelUp(gearData.baseDamage + gearData.damage[gear.level]);
        }
    }

    public void SetAttribute(ItemData.AttributeType attribute)
    {
        currentAttribute = attribute;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOverScene"); // 게임 오버 씬 로드
    }
}
