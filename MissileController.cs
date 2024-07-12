using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float damage;
    public ItemData.AttributeType attributeType;
    private float duration = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            EnemyBoss enemy = other.GetComponent<EnemyBoss>();
            if (enemy != null)
            {
                ApplyAttributeEffect(enemy);
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyAttributeEffect(EnemyBoss enemy)
    {
        switch (attributeType)
        {
            case ItemData.AttributeType.Fire:
                if (enemy.attribute == EnemyBoss.AttributeType.Grass)
                {
                    damage *= 1.5f;
                    enemy.ApplyDotDamage(damage * 0.1f, duration);
                }
                else if (enemy.attribute == EnemyBoss.AttributeType.Water)
                {
                    damage *= 0.5f;
                }
                break;
            case ItemData.AttributeType.Water:
                if (enemy.attribute == EnemyBoss.AttributeType.Fire)
                {
                    damage *= 1.5f;
                    enemy.IncreaseSpeed(0.1f, duration);
                }
                else if (enemy.attribute == EnemyBoss.AttributeType.Grass)
                {
                    damage *= 0.5f;
                }
                break;
            case ItemData.AttributeType.Grass:
                if (enemy.attribute == EnemyBoss.AttributeType.Water)
                {
                    damage *= 1.5f;
                }
                else if (enemy.attribute == EnemyBoss.AttributeType.Fire)
                {
                    damage *= 0.5f;
                }
                break;
            case ItemData.AttributeType.Light:
                if (enemy.attribute == EnemyBoss.AttributeType.Dark)
                {
                    damage *= 1.5f;
                    damage *= 1.05f;
                }
                else if (enemy.attribute == EnemyBoss.AttributeType.Light)
                {
                    damage *= 0.5f;
                }
                break;
            case ItemData.AttributeType.Dark:
                if (enemy.attribute == EnemyBoss.AttributeType.Light)
                {
                    damage *= 1.5f;
                    // 미사일 발사 속도 증가 로직 필요
                }
                else if (enemy.attribute == EnemyBoss.AttributeType.Dark)
                {
                    damage *= 0.5f;
                }
                break;
        }
    }
}
