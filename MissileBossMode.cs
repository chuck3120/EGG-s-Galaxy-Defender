using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBossMode : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // 미사일을 위쪽으로 이동시키지만 기본 회전이 270도이므로 시각적으로는 아래로 이동합니다
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

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
