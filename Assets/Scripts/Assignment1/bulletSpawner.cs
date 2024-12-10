using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;   // �Ѿ� ������
    public GameObject portalPrefab;   // ��Ż ������
    public Transform[] spawnPoints;   // �Ѿ��� ������ ��ġ��
    public Vector3[] spawnDirections; // �� ���� ����Ʈ���� �Ѿ��� �߻�� ����
    public float spawnInterval = 2f;  // �Ѿ� ���� ����
    public float bulletSpawnOffset = 1f; // ��Ż���� �Ѿ� ���� ��ġ������ �Ÿ�

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
        // ������ ��ġ ����
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Vector3 spawnDirection = spawnDirections[randomIndex];

        // ��Ż ����
        GameObject portal = Instantiate(portalPrefab, spawnPoint.position, spawnPoint.rotation);

        // ��Ż ���ʿ� �Ѿ� ����
        Vector3 bulletSpawnPosition = spawnPoint.position + (spawnDirection * bulletSpawnOffset);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(spawnDirection));

    
        bullet.transform.SetParent(portal.transform);

        // �Ѿ� �̵� ���� ����
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(spawnDirection);
    }
}
