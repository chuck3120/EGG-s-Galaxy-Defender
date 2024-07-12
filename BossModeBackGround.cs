using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModeBackGround : MonoBehaviour
{
    // 객체의 이동 속도를 정의하는 변수
    public float moveSpeed = 4f;

    // 배경의 높이를 정의하는 변수 (배경 이미지의 실제 높이와 동일해야 함)
    public float backgroundHeight = 11f;

    // Update 메서드는 프레임마다 호출됩니다.
    void Update()
    {
        // 객체를 아래로 이동시키기 위해 현재 위치에서 Y축 방향으로 moveSpeed 값을 뺍니다.
        // Time.deltaTime을 곱하여 프레임 독립적으로 움직이도록 합니다.
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // y축이 -backgroundHeight보다 작아지면 y를 backgroundHeight로 설정
        if (transform.position.y < -backgroundHeight)
        {
            transform.position += new Vector3(0, 2 * backgroundHeight, 0);
        }
    }
}
