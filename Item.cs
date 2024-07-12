using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public ItemData data; // 아이템 데이터를 저장
    public int level; // 현재 아이템 레벨
    public Weapon weapon; // 아이템에 연결된 무기
    public Gear gear; // 아이템에 연결된 장비

    private Image icon; // 아이템 아이콘 이미지
    private Text textLevel; // 아이템 레벨 텍스트
    private Text textName;
    public Text textDescription;
    

    private DataManager dataManager;
    public GameObject[] secondaryWeaponPrefabs; // SecondaryWeapon 프리팹 배열
    private GameObject[] activeSecondaryWeapons;

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();

        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDescription = texts[2];
        textName.text = data.itemName;

        activeSecondaryWeapons = new GameObject[secondaryWeaponPrefabs.Length];
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.SpeedUp:
                textDescription.text = string.Format(data.itemDescription, data.moveSpeedIncrease * 100);
                break;
            case ItemData.ItemType.PowerUp:
                textDescription.text = string.Format(data.itemDescription, data.damage[level] * 100);
                break;
            case ItemData.ItemType.MaxHpUp:
                textDescription.text = string.Format(data.itemDescription, data.baseDamage * level);
                break;
            case ItemData.ItemType.MissileSpeedUp:
                textDescription.text = string.Format(data.itemDescription, data.missileSpeedIncrease * 100);
                break;
            default:
                textDescription.text = data.itemDescription;
                break;
        }
    }

    public void OnClick()
    {
        BossModePlayer playerAttributes = FindObjectOfType<BossModePlayer>();
        switch (data.itemType)
        {
            case ItemData.ItemType.SpeedUp:
                playerAttributes.speed += data.moveSpeedIncrease;
                level++;
                break;
            case ItemData.ItemType.PowerUp:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = playerAttributes.baseMissileDamage + (playerAttributes.baseMissileDamage * data.damage[level]);
                    weapon.LevelUp(nextDamage, level + 1); // level + 1을 newCount로 설정
                }
                level++;
                break;
            case ItemData.ItemType.MaxHpUp:
                playerAttributes.maxHealth += (int)(data.baseDamage * level);
                playerAttributes.health = playerAttributes.maxHealth;
                level++;
                break;
            case ItemData.ItemType.MissileSpeedUp:
                playerAttributes.missileSpeed += data.missileSpeedIncrease;
                level++;
                break;
            case ItemData.ItemType.Heal:
                BossManager.instance.health = BossManager.instance.maxHealth;
                break;
            case ItemData.ItemType.SecondaryWeapon:
                int index = data.secondaryWeaponIndex;
                if (index >= 0 && index < secondaryWeaponPrefabs.Length && activeSecondaryWeapons[index] == null)
                {
                    activeSecondaryWeapons[index] = Instantiate(secondaryWeaponPrefabs[index], transform.position, Quaternion.identity);
                    activeSecondaryWeapons[index].GetComponent<SecondaryWeaponController>().player = playerAttributes.transform;
                }
                level++;
                break;
            case ItemData.ItemType.AttributeChange:
                if (playerAttributes != null)
                {
                    BossModePlayer bossModePlayer = FindObjectOfType<BossModePlayer>();
                    if (bossModePlayer != null)
                    {
                        bossModePlayer.SetAttribute(data.attributeType);
                    }
                }
                level++;
                break;
        }

        if (level == data.damage.Length)
        {
            GetComponent<Button>().interactable = false;
        }

        SaveCurrentData();
    }

    private void SaveCurrentData()
    {
        List<ItemData> itemList = dataManager.LoadData();
        itemList.Add(data);
        dataManager.SaveData(itemList);
    }
}
