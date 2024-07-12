using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireController : MonoBehaviour
{
    public float fireRate = 2f; // 발사 속도
    public List<GameObject> missilePrefabs; // 미사일 프리팹 리스트
    public Transform firePoint; // 미사일 발사 위치

    void Start()
    {
        StartCoroutine(FireMissiles());
    }

    IEnumerator FireMissiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            if (missilePrefabs.Count > 0 && firePoint != null)
            {
                int randomIndex = Random.Range(0, missilePrefabs.Count);
                GameObject selectedMissile = missilePrefabs[randomIndex];
                Instantiate(selectedMissile, firePoint.position, firePoint.rotation);
            }
        }
    }
}
