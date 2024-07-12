using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public List<Sprite> characterSprites; // 캐릭터 이미지 리스트
    public List<string> characterNames; // 캐릭터 이름 리스트
    public Image characterDisplayImage; // 중앙 캐릭터 이미지를 표시할 UI Image
    public Text characterNameText; // 캐릭터 이름을 표시할 UI Text

    // 각 캐릭터 버튼들을 리스트로 관리
    public List<Button> characterButtons;

    void Start()
    {
        // 각 버튼에 클릭 이벤트 추가
        for (int i = 0; i < characterButtons.Count; i++)
        {
            int index = i; // 로컬 변수로 인덱스 저장
            characterButtons[i].onClick.AddListener(() => OnCharacterButtonClick(index));
        }

        // 초기 디스플레이 설정
        UpdateCharacterDisplay(0);
    }

    void OnCharacterButtonClick(int index)
    {
        UpdateCharacterDisplay(index);
    }

    void UpdateCharacterDisplay(int index)
    {
        characterDisplayImage.sprite = characterSprites[index];
        characterNameText.text = characterNames[index];

        // 애니메이션 재생 (Animator 컴포넌트가 있다면 트리거를 설정하여 재생)
        Animator animator = characterDisplayImage.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Play");
        }
    }
}
