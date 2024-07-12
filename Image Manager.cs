using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{
    // 초기 크기
    private Vector3 originalScale = new Vector3(0.09f, 0.09f, 0.09f);
    // 위치
    private Vector3 originalPosition = new Vector3(-367f, 594f, 0f);

    // 크기 변화 비율 및 속도
    public float scaleFactor = 0.2f; // 크기 변화 범위 비율
    public float speed = 2.0f; // 크기 변화 속도

    private RectTransform rectTransform;

    void Start()
    {
        // RectTransform 컴포넌트 가져오기
        rectTransform = GetComponent<RectTransform>();
        // 초기 크기 및 위치 설정
        rectTransform.localScale = originalScale;
        rectTransform.localPosition = originalPosition;
    }

    void Update()
    {
        // 시간에 따라 크기가 변하도록 계산
        float scaleOffset = (Mathf.PingPong(Time.time * speed, 1.0f) - 0.5f) * 2.0f * scaleFactor;
        rectTransform.localScale = originalScale + originalScale * scaleOffset;
    }
}
