using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnInterval = 5f;
    public float spawnHeight = 10f;  // 스폰 높이 설정

    private float spawnTimer;

    void Start()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0)
        {
            Debug.LogError("Waypoints not set or empty in Waypoints");
            return;
        }

        spawnTimer = spawnInterval;  // 처음에 바로 스폰되도록 타이머 초기화
    }

    void Update()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnMonster();
            spawnTimer = spawnInterval;  // 타이머 리셋
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
            monsterAI.SetWaypoints(Waypoints.points);  // Waypoints 설정
        }
        else
        {
            Debug.LogError("Monster prefab does not have a MonsterAI component");
        }

        // Rigidbody 설정 조정
        Rigidbody rb = monster.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = 1f;  // 몬스터의 질량 설정
            rb.drag = 5f;  // 몬스터의 선형 저항 설정
            rb.angularDrag = 5f;  // 몬스터의 각 저항 설정
        }
    }
}
