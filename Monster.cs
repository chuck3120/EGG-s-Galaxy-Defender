using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 코인 프리팹 참조
    [SerializeField] private GameObject coin;

    // 폭발 효과 프리팹 참조
    [SerializeField] private GameObject BlowUpEffect;

    // 특수 코인 프리팹 참조
    [SerializeField] private GameObject FinisMoveCoinPrefab;

    // 특수 코인 생성 확률 (0에서 1 사이의 값)
    [Range(0f, 1f)]
    [SerializeField] private float FinisMoveCoinSpawnChance = 0.015f;

    // 몬스터가 파괴될 최소 Y값
    private float minY = -7f;

    // 몬스터 체력
    [SerializeField] private float monsterHp = 10f;

    // 몬스터가 사망했는지 여부를 나타내는 변수
    private bool isDead = false;

    // 폭발 소리 오디오 클립
    [SerializeField] private AudioClip blowUpSound;

    // 매 프레임마다 호출되는 메서드
    void Update()
    {
        // 몬스터가 최소 Y값보다 아래로 내려가면 파괴
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }
    }

    // 다른 Collider와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터가 이미 죽었으면 리턴
        if (isDead) return;

        // 충돌한 객체가 미사일인 경우
        if (other.CompareTag("Missile"))
        {
            // 미사일의 데미지를 가져와서 몬스터에게 적용
            Missile missile = other.GetComponent<Missile>();
            if (missile != null)
            {
                TakeDamage(missile.damage);
                // 미사일 파괴
                Destroy(other.gameObject);
            }
        }
    }

    // 몬스터가 데미지를 받을 때 호출되는 메서드
    public void TakeDamage(float damage)
    {
        // 몬스터가 이미 죽었으면 리턴
        if (isDead) return;

        // 체력 감소
        monsterHp -= damage;

        // 체력이 0 이하가 되면 몬스터 파괴
        if (monsterHp <= 0)
        {
            isDead = true;
            PlayBlowUpSound();

            // 랜덤 값을 생성하여 특수 코인 생성 확률 계산
            float randomValue = Random.value;
            if (randomValue <= FinisMoveCoinSpawnChance)
            {
                // 특수 코인 생성
                Instantiate(FinisMoveCoinPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // 일반 코인 생성
                Instantiate(coin, transform.position, Quaternion.identity);
            }

            // 폭발 효과 생성
            Instantiate(BlowUpEffect, transform.position, Quaternion.identity);
            // 몬스터 파괴
            Destroy(gameObject);
        }
    }

    // 폭발 소리를 재생하는 메서드
    private void PlayBlowUpSound()
    {
        if (blowUpSound != null)
        {
            // 새로운 오디오 객체 생성
            GameObject audioObject = new GameObject("ExplosionSound");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = blowUpSound;
            audioSource.Play();
            // 소리 길이 후 오디오 객체 파괴
            Destroy(audioObject, blowUpSound.length);
        }
    }
}
