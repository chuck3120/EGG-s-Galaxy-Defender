using UnityEngine;

public class Coin : MonoBehaviour
{
    private float destroyY = -8f;
    private bool collected = false;

    [SerializeField] private AudioClip collectSound; // 코인 수집 소리 추가

    private void Start()
    {
        Jump();
    }

    private void Jump()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float randomJumpForce = Random.Range(1f, 6f);
        Vector2 jumpVelocity = Vector2.up * randomJumpForce;
        jumpVelocity.x = Random.Range(-2.3f, 2.3f); // x축 이동 범위를 -2.3과 2.3 사이로 제한
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }

        // x축 범위 제한
        if (transform.position.x < -2.3f)
        {
            transform.position = new Vector3(-2.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 2.3f)
        {
            transform.position = new Vector3(2.3f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            GetComponent<Collider2D>().enabled = false;

            if (GameManager.instance != null)
            {
                if (gameObject.CompareTag("GoldCoin"))
                {
                    GameManager.instance.IncreaseCoin(2);
                }
                else
                {
                    GameManager.instance.IncreaseCoin(1);
                }
            }

            PlayCollectSound(); // 코인 수집 소리 재생

            // 오브젝트를 즉시 파괴합니다.
            Destroy(gameObject);
        }
    }

    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            // 별도의 오디오 소스 오브젝트 생성
            GameObject audioObject = new GameObject("CollectSound");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = collectSound;
            audioSource.Play();

            // 오디오 재생이 끝난 후 오브젝트 파괴
            Destroy(audioObject, collectSound.length);
        }
    }
}
