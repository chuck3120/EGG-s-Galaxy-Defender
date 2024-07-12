using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // 객체의 이동 속도를 정의하는 변수
    public float moveSpeed = 3f;

    // Update 메서드는 프레임마다 호출됩니다.
    void Update()
    {
        // 객체를 아래로 이동시키기 위해 현재 위치에서 Y축 방향으로 moveSpeed 값을 뺍니다.
        // Time.deltaTime을 곱하여 프레임 독립적으로 움직이도록 합니다.
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // 객체의 Y 위치가 -6보다 작아지면
        if (transform.position.y < -6)
        {
            // 객체의 위치를 Y축 방향으로 15만큼 위로 이동시킵니다.
            transform.position += new Vector3(0, 15f, 0);
        }
    }
}
