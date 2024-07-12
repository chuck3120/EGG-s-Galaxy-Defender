using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect; // RectTransform 컴포넌트 참조
    Item[] items; // Item 배열 참조

    // MonoBehaviour의 Awake 메서드, 오브젝트가 활성화될 때 호출됩니다.
    private void Awake() 
    {
        // RectTransform 컴포넌트 가져오기
        rect = GetComponent<RectTransform>();
        // 자식 오브젝트들에서 Item 컴포넌트들을 가져와 배열에 저장
        items = GetComponentsInChildren<Item>(true);
    }

    // 레벨 업 UI를 보여주는 메서드
    public void Show()
    {
        Next(); // 다음 레벨 업 아이템을 선택
        rect.localScale = Vector3.one; // UI를 활성화
        GameManager.instance.Stop(); // 게임을 일시 정지
    }

    // 레벨 업 UI를 숨기는 메서드
    public void Hide()
    {
        rect.localScale = Vector3.zero; // UI를 비활성화
        GameManager.instance.Resume(); // 게임을 재개
    }

    // 플레이어가 특정 아이템을 선택했을 때 호출되는 메서드
    public void Select(int index)
    {
        items[index].OnClick(); // 선택된 아이템의 OnClick 메서드를 호출
        Hide(); // 선택 후 UI를 숨김
    }

    // 다음 레벨 업 아이템을 선택하는 메서드
    void Next()
    {
        // 1. 모든 아이템을 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그중 랜덤하게 4개만 활성화
        int[] ran = new int[4];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            ran[3] = Random.Range(0, items.Length);

            // 네 개의 랜덤한 인덱스가 서로 다를 때까지 반복
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2] && ran[0] != ran[3] && ran[1] != ran[3] && ran[2] != ran[3])
                break;
        }

        // 선택된 4개의 아이템 활성화
        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // 만렙 아이템의 경우 소비 아이템으로 대체
            if (ranItem.level >= ranItem.data.damage.Length - 1)
            {
                items[4].gameObject.SetActive(true); // 예시로 4번 아이템을 활성화
                // items[Random.Range(4, 7)].gameObject.SetActive(true); // 여러 소비 아이템 중 하나를 랜덤으로 활성화
            }
            else
            {
                ranItem.gameObject.SetActive(true); // 아이템 활성화
            }
        }
    }
}
