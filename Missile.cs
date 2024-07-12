using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; // 미사일 이동 속도
    public float damage; // 미사일 데미지

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; // 미사일 이동

        // 미사일이 y값 5.5에 도달하면 파괴
        if (transform.position.y >= 5.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Destroy(gameObject); // 미사일 파괴
            }
        }
        else if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Destroy(gameObject); // 미사일 파괴
            }
        }
    }
}
