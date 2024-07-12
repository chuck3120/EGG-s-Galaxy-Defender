using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage;
    private int count;

    public void Init(ItemData data)
    {
        damage = data.baseDamage;
        count = 1; // 초기 값 설정
    }

    public void LevelUp(float newDamage, int newCount)
    {
        damage = newDamage;
        count = newCount;
    }

    // 필요한 경우 추가적인 메서드나 속성을 정의합니다.
}
