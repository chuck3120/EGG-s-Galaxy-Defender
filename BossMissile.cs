using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // 화면 아래로 벗어나면 파괴
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어와 충돌하면 플레이어를 파괴
            Destroy(collision.gameObject);
        }
    }
}
