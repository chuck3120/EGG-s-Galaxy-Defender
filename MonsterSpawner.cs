using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Monsters; // 몬스터 프리팹 배열을 저장할 변수 선언

    [SerializeField]
    private GameObject[] Bosses; // 보스 프리팹 배열을 저장할 변수 선언

    private float[] arrPosX = { -2f, -1f, 0f, 1f, 2f }; // 몬스터 생성 위치 X 좌표 배열 선언

    [SerializeField]
    private float spawnInterval; // 몬스터 생성 간격을 저장할 변수 선언

    private bool stopSpawning = false; // 몬스터 생성 중지 여부를 저장할 변수 선언

    private Coroutine monsterRoutine; // 몬스터 생성 코루틴을 저장할 변수 선언

    public float moveSpeed; // 몬스터 이동 속도를 저장할 변수 선언

    private int currentWave = 0; // 현재 웨이브를 저장할 변수
    private int totalWaves = 10; // 총 웨이브 수

    void Start()
    {
        if (Monsters.Length == 0 || Bosses.Length == 0)
        {
            Debug.LogError("Monsters or Bosses is not set.");
            return;
        }

        StartMonsterRoutine();
    }

    void StartMonsterRoutine()
    {
        monsterRoutine = StartCoroutine(MonsterRoutine());
    }

    public void StopMonsterRoutine()
    {
        if (monsterRoutine != null)
        {
            StopCoroutine(monsterRoutine);
        }
    }

    public void StopSpawningMonsters()
    {
        stopSpawning = true;
    }

    IEnumerator MonsterRoutine()
    {
        Debug.Log("MonsterRoutine started.");

        yield return new WaitForSeconds(2f);

        while (!stopSpawning && currentWave < totalWaves)
        {
            currentWave++;
            int monstersCreated = 0;

            while (monstersCreated < 5)
            {
                for (int i = 0; i < arrPosX.Length; i++)
                {
                    if (stopSpawning) yield break;

                    int upgradeIndex = Random.Range(0, 10);
                    bool shouldUpgrade = Random.Range(0, 5) == 0;

                    int monsterIndex = Mathf.Min(GameManager.instance.currentStage - 1, Monsters.Length - 1); // 현재 스테이지에 맞는 몬스터 인덱스 설정

                    if (i == upgradeIndex && shouldUpgrade && monsterIndex < Monsters.Length - 1)
                    {
                        SpawnMonster(arrPosX[i], monsterIndex + 1);
                    }
                    else
                    {
                        SpawnMonster(arrPosX[i], monsterIndex);
                    }

                    monstersCreated++;
                    if (monstersCreated >= 5) break;
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return new WaitForSeconds(2f); // 각 웨이브 사이에 2초 대기
        }

        if (currentWave >= totalWaves)
        {
            GameManager.instance.NextStage();
        }
    }

    void SpawnBoss(int bossIndex)
    {
        if (bossIndex < Bosses.Length)
        {
            Vector3 spawnPos = new Vector3(0, transform.position.y, transform.position.z);
            GameObject bossObject = Instantiate(Bosses[bossIndex], spawnPos, Quaternion.Euler(0f, 0f, 180f)); // 보스를 Z축으로 180도 회전

            Boss boss = bossObject.GetComponent<Boss>();
            if (boss != null)
            {
                boss.OnBossDefeated += OnBossDefeated; // 보스가 처치되었을 때 호출되는 이벤트 등록
            }
        }
    }

    void SpawnMonster(float posX, int index)
    {
        if (stopSpawning) return;

        Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 180f);

        GameObject monsterObject = Instantiate(Monsters[index], spawnPos, rotation);
        if (monsterObject == null)
        {
            Debug.LogError("Failed to instantiate monster.");
            return;
        }

        Monster monster = monsterObject.GetComponent<Monster>();
        if (monster == null)
        {
            Debug.LogError("Failed to get Monster component.");
            return;
        }

        StartCoroutine(MoveMonster(monster.transform));
    }

    IEnumerator MoveMonster(Transform monsterTransform)
    {
        while (monsterTransform != null && monsterTransform.position.y > -7f)
        {
            monsterTransform.position += Vector3.down * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (monsterTransform != null)
        {
            Destroy(monsterTransform.gameObject);
        }
    }

    private void OnBossDefeated() // 보스가 처치되었을 때 호출되는 메소드
    {
        stopSpawning = false;
        GameManager.instance.NextStage(); // 다음 스테이지로 이동
    }

    public void StartNextStage()
    {
        stopSpawning = false;
        currentWave = 0; // 스테이지 시작 시 웨이브 초기화
        if (GameManager.instance.currentStage == 18)
        {
            return;
        }
        else if (GameManager.instance.currentStage == 6 || GameManager.instance.currentStage == 12 || GameManager.instance.currentStage == 17)
        {
            GameManager.instance.UpdateStageText("Boss"); // 보스 스테이지에서 "Boss" 텍스트 표시
            GameManager.instance.ShowWarningText();
            int bossIndex = (GameManager.instance.currentStage / 6) - 1; // 보스 인덱스 설정 (6, 12, 17에서 보스가 나오도록 설정)
            SpawnBoss(bossIndex);
            return;
        }
        moveSpeed = 5f + 2f * (GameManager.instance.currentStage - 1);
        StartMonsterRoutine();
    }
}
