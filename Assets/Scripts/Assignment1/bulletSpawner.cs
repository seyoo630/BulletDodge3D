using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;   // 총알 프리팹
    public GameObject portalPrefab;   // 포탈 프리팹
    public Transform[] spawnPoints;   // 총알이 생성될 위치들
    public Vector3[] spawnDirections; // 각 스폰 포인트에서 총알이 발사될 방향
    public float spawnInterval = 2f;  // 총알 생성 간격
    public float bulletSpawnOffset = 1f; // 포탈에서 총알 생성 위치까지의 거리

    private void Start()
    {
        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBullet()
    {
        // 랜덤한 위치 선택
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Vector3 spawnDirection = spawnDirections[randomIndex];

        // 포탈 생성
        GameObject portal = Instantiate(portalPrefab, spawnPoint.position, spawnPoint.rotation);

        // 포탈 앞쪽에 총알 생성
        Vector3 bulletSpawnPosition = spawnPoint.position + (spawnDirection * bulletSpawnOffset);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(spawnDirection));

    
        bullet.transform.SetParent(portal.transform);

        // 총알 이동 방향 설정
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(spawnDirection);
    }
}
