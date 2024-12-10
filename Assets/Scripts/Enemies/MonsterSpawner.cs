using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnInterval = 5f;
    public float spawnHeight = 10f;  // ���� ���� ����

    private float spawnTimer;

    void Start()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0)
        {
            Debug.LogError("Waypoints not set or empty in Waypoints");
            return;
        }

        spawnTimer = spawnInterval;  // ó���� �ٷ� �����ǵ��� Ÿ�̸� �ʱ�ȭ
    }

    void Update()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnMonster();
            spawnTimer = spawnInterval;  // Ÿ�̸� ����
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null)
        {
            Debug.LogError("Monster prefab not set in MonsterSpawner");
            return;
        }

        Vector3 spawnPosition = Waypoints.points[0].position + Vector3.up * spawnHeight;
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        MonsterAI monsterAI = monster.GetComponent<MonsterAI>();
        if (monsterAI != null)
        {
            monsterAI.SetWaypoints(Waypoints.points);  // Waypoints ����
        }
        else
        {
            Debug.LogError("Monster prefab does not have a MonsterAI component");
        }

        // Rigidbody ���� ����
        Rigidbody rb = monster.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = 1f;  // ������ ���� ����
            rb.drag = 5f;  // ������ ���� ���� ����
            rb.angularDrag = 5f;  // ������ �� ���� ����
        }
    }
}
