using UnityEngine;

public class Gear : MonoBehaviour
{
    private ItemData data;
    private float rate;
    private Transform[] gearPositions;
    private GameObject[] gears;
    private int currentGearCount = 0;
    public int level { get; private set; } = 0; // 현재 레벨

    public void Init(ItemData itemData, Transform[] positions)
    {
        data = itemData;
        rate = data.baseDamage;
        gearPositions = positions;
        gears = new GameObject[gearPositions.Length];
        level = 0; // 레벨 초기화
    }

    public void LevelUp(float nextRate)
    {
        rate = nextRate;

        if (currentGearCount < gearPositions.Length)
        {
            GameObject newGear = new GameObject(data.itemName);
            newGear.transform.position = gearPositions[currentGearCount].position;
            newGear.transform.parent = gearPositions[currentGearCount];
            // 아이템 아이콘이나 프리팹 등을 여기서 설정할 수 있습니다.
            gears[currentGearCount] = newGear;
            currentGearCount++;
        }

        UpdateGearVisibility();
        level++; // 레벨 증가
    }

    private void UpdateGearVisibility()
    {
        for (int i = 0; i < currentGearCount; i++)
        {
            gears[i].SetActive(true);
        }
    }
}
