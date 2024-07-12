using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public enum AttributeType
    {
        Fire,
        Water,
        Grass,
        Light,
        Dark
    }

    public AttributeType attribute;
    public float health;
    private bool isDead = false;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            FindObjectOfType<BossManager>().BossDefeated();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Missile"))
        {
            MissileController missile = other.GetComponent<MissileController>();
            if (missile != null)
            {
                ApplyAttributeEffect(missile.attributeType);
                TakeDamage(missile.damage);
                Destroy(other.gameObject);
            }
        }
    }

    private void ApplyAttributeEffect(ItemData.AttributeType missileAttribute)
    {
        switch (missileAttribute)
        {
            case ItemData.AttributeType.Fire:
                if (attribute == AttributeType.Grass)
                {
                    health -= 10; // 예시로 추가 데미지를 주거나
                }
                else if (attribute == AttributeType.Water)
                {
                    health -= 5; // 다른 속성 효과를 줄 수 있습니다.
                }
                break;
            case ItemData.AttributeType.Water:
                if (attribute == AttributeType.Fire)
                {
                    health -= 10;
                }
                else if (attribute == AttributeType.Grass)
                {
                    health -= 5;
                }
                break;
            // 다른 속성 효과들도 추가할 수 있습니다.
        }
    }

    public void ApplyDotDamage(float amount, float duration)
    {
        StartCoroutine(DotDamage(amount, duration));
    }

    private IEnumerator DotDamage(float amount, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void IncreaseSpeed(float amount, float duration)
    {
        StartCoroutine(SpeedBoost(amount, duration));
    }

    private IEnumerator SpeedBoost(float amount, float duration)
    {
        float originalSpeed = GetComponent<BossPool>().moveSpeed;
        GetComponent<BossPool>().moveSpeed += originalSpeed * amount;
        yield return new WaitForSeconds(duration);
        GetComponent<BossPool>().moveSpeed = originalSpeed;
    }

    private void Die()
    {
        FindObjectOfType<BossManager>().BossDefeated();
        Destroy(gameObject);
    }
}

