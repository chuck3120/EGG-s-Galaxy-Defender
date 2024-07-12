using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPool : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 0.5f;
    public float minX = -1f;
    public float maxX = 1f;
    public float minY = -5.5f;
    public GameObject[] missilePrefabs; // 미사일 프리펩 배열
    public float missileFireRate = 2f; // 미사일 발사 간격
    public float xMoveSpeed = 2f; // x축 이동 속도
    private bool isPaused = false;
    private bool isPausedCoroutineRunning = false;
    private bool movingRight = true;
    private bool reachedYThreshold = false;
    private BossManager bossManager;
    

    void Start()
    {
        bossManager = FindObjectOfType<BossManager>();
        // 처음에는 미사일 발사 코루틴을 시작하지 않음
    }

    void Update()
    {
        if (!isPaused)
        {
            // 보스가 y축 2.5에 도달하지 않았으면 아래로 이동
            if (!reachedYThreshold)
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                
                // 보스가 y축 위치 2.5에 도달하면 이동 멈추고 x축으로만 이동
                if (transform.position.y <= 2.5f)
                {
                    reachedYThreshold = true;
                    StartCoroutine(FireMissiles());
                }
            }

            // 보스를 x축에서 왔다 갔다 움직이도록 설정
            if (reachedYThreshold)
            {
                if (movingRight)
                {
                    transform.position += Vector3.right * xMoveSpeed * Time.deltaTime;
                    if (transform.position.x >= maxX)
                    {
                        movingRight = false;
                    }
                }
                else
                {
                    transform.position += Vector3.left * xMoveSpeed * Time.deltaTime;
                    if (transform.position.x <= minX)
                    {
                        movingRight = true;
                    }
                }
            }
        }

        // 보스의 y축 위치가 minY 이하로 내려가면 게임 오버 처리
        if (transform.position.y <= minY)
        {
            bossManager.BossDefeated();
        }
    }

    IEnumerator FireMissiles()
    {
        while (true)
        {
            if (missilePrefabs.Length > 0)
            {
                // 미사일 프리펩을 무작위로 선택하여 발사
                int index = Random.Range(0, missilePrefabs.Length);
                if (index == 1)
                {
                    // 엘레멘트 1이면 두 개의 미사일을 x축 -1과 1 위치에 발사
                    Vector3 shootPositionLeft = new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z);
                    Vector3 shootPositionRight = new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z);
                    Instantiate(missilePrefabs[index], shootPositionLeft, Quaternion.Euler(0, 0, 270));
                    Instantiate(missilePrefabs[index], shootPositionRight, Quaternion.Euler(0, 0, 270));
                }
                else if (index == 2)
                {
                    // 엘레멘트 2이면 세 개의 미사일을 서로 다른 회전 각도로 발사
                    Vector3 shootPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                    Instantiate(missilePrefabs[index], shootPosition, Quaternion.Euler(0, 0, 270));
                    Instantiate(missilePrefabs[index], shootPosition, Quaternion.Euler(0, 0, 280));
                    Instantiate(missilePrefabs[index], shootPosition, Quaternion.Euler(0, 0, 260));
                }
                else
                {
                    // 다른 엘레멘트는 기본 위치에 발사
                    Vector3 shootPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                    Instantiate(missilePrefabs[index], shootPosition, Quaternion.Euler(0, 0, 270));
                }
            }
            yield return new WaitForSeconds(missileFireRate);
        }
    }
}
