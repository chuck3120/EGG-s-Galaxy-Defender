using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data", order = 51)]
[Serializable]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        SpeedUp,
        PowerUp,
        MaxHpUp,
        MissileSpeedUp,
        Heal,
        SecondaryWeapon,
        AttributeChange
    }

    public enum AttributeType
    {
        None,
        Fire,
        Water,
        Grass,
        Light,
        Dark
    }

    public string itemName; // 아이템 이름
    public ItemType itemType; // 아이템 타입
    public Sprite itemIcon; // 아이템 아이콘
    public string itemDescription; // 아이템 설명
    public float baseDamage; // 기본 데미지
    public float[] damage; // 각 레벨별 데미지 증가량
    public AttributeType attributeType; // 아이템 속성 타입
    public float moveSpeedIncrease; // 이동 속도 증가량
    public float missileSpeedIncrease; // 미사일 발사 속도 증가량
    public int secondaryWeaponIndex; // 세컨더리 웨폰 인덱스
}
