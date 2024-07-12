using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.3f;

    [SerializeField]
    private GameObject[] bossMissilePrefabs;

    [SerializeField]
    private Transform shootTransform;

    [SerializeField]
    private GameObject BlowUpEffect; // 폭발 효과 프리팹 추가

    [SerializeField]
    private AudioClip blowUpSound; // 폭발 소리 오디오 클립 추가

    private float minY = -5.5f;

    public float shootInterval = 5f;
    private float lastShootTime;

    private float sinFrequency = 1f;
    private float sinMagnitude = 2f;

    private Vector3 startPos;

    [SerializeField] private float bossHp = 800f;

    private bool isDead = false;

    private object lockObject = new object();

    public event Action OnBossDefeated;

    void Start()
    {
        lastShootTime = Time.time;
        startPos = transform.position;

        if (shootTransform == null)
        {
            shootTransform = transform.Find("ShootPoint");
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.ShowBossHpBar(bossHp);
        }
    }

    void Update()
    {
        Move();

        if (Time.time - lastShootTime >= shootInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Move()
    {
        float newX = startPos.x + Mathf.Sin(Time.time * sinFrequency) * sinMagnitude;
        transform.position = new Vector3(newX, transform.position.y - moveSpeed * Time.deltaTime, transform.position.z);

        if (transform.position.y < minY)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SetGameOver();
            }
        }
    }

    private void Shoot()
    {
        if (shootTransform != null && bossMissilePrefabs.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, bossMissilePrefabs.Length);
            GameObject missilePrefab = bossMissilePrefabs[randomIndex];

            if (randomIndex == 0)
            {
                Instantiate(missilePrefab, shootTransform.position, Quaternion.Euler(0, 0, 270));
            }
            else if (randomIndex == 1)
            {
                Vector3 leftPosition = new Vector3(shootTransform.position.x - 1, shootTransform.position.y, shootTransform.position.z);
                Vector3 rightPosition = new Vector3(shootTransform.position.x + 1, shootTransform.position.y, shootTransform.position.z);

                Instantiate(missilePrefab, leftPosition, Quaternion.Euler(0, 0, 270));
                Instantiate(missilePrefab, rightPosition, Quaternion.Euler(0, 0, 270));
            }
            else if (randomIndex == 2)
            {
                Vector3 leftPosition = new Vector3(shootTransform.position.x - 1, shootTransform.position.y, shootTransform.position.z);
                Vector3 rightPosition = new Vector3(shootTransform.position.x + 1, shootTransform.position.y, shootTransform.position.z);

                Instantiate(missilePrefab, leftPosition, Quaternion.Euler(0, 0, 270));
                Instantiate(missilePrefab, rightPosition, Quaternion.Euler(0, 0, 270));

                Vector3 shootPosition = new Vector3(shootTransform.position.x, shootTransform.position.y - 1, shootTransform.position.z);
                Instantiate(missilePrefab, shootPosition, Quaternion.Euler(0, 0, 270));
                Instantiate(missilePrefab, shootPosition, Quaternion.Euler(0, 0, 280));
                Instantiate(missilePrefab, shootPosition, Quaternion.Euler(0, 0, 260));
            }
        }
    }

    public void TakeDamage(float damage)
    {
        lock (lockObject)
        {
            if (isDead) return;

            bossHp -= damage;

            if (GameManager.instance != null && GameManager.instance.bossHpBar != null)
            {
                GameManager.instance.bossHpBar.value = bossHp;
            }

            if (bossHp <= 0)
            {
                isDead = true;
                if (GameManager.instance != null)
                {
                    GameManager.instance.HideBossHpBar();
                }

                OnBossDefeated?.Invoke();

                // 폭발 효과 생성
                Instantiate(BlowUpEffect, transform.position, Quaternion.identity);
                // 폭발 소리 재생
                PlayBlowUpSound();

                Destroy(gameObject);
            }
        }
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Missile missile = other.GetComponent<Missile>();
            if (missile != null)
            {
                TakeDamage(missile.damage);
                Destroy(other.gameObject);
            }
        }
    }
}
