using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpEffect : MonoBehaviour
{
    // 효과가 파괴되기 전까지의 지속 시간을 정의하는 변수입니다.
    [SerializeField]
    private float effectDuration = 0.5f;

    // Start 메서드는 스크립트가 처음 실행될 때 호출됩니다.
    void Start()
    {
        // 지정된 지속 시간 후에 게임 오브젝트를 파괴합니다.
        Destroy(gameObject, effectDuration);

        // 로테이션의 z축을 180도 돌립니다.
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
