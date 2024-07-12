using System.Collections;
using UnityEngine;

public class SecondaryWeaponController : MonoBehaviour
{
    public Transform player; // 플레이어를 따라다니도록 참조
    public GameObject missilePrefab; // 미사일 프리팹
    public float followDistance = 2.0f; // 플레이어와의 거리
    public float missileCooldown = 10.0f; // 미사일 발사 간격

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>().transform; // 플레이어 찾기
        }
        StartCoroutine(FireMissile());
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = player.position + player.forward * followDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2.0f); // 부드럽게 따라다니기
    }

    private IEnumerator FireMissile()
    {
        while (true)
        {
            yield return new WaitForSeconds(missileCooldown);
            Instantiate(missilePrefab, transform.position, transform.rotation); // 미사일 발사
        }
    }
}
