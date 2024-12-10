using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public GameObject healthBarPrefab; // 체력 바 프리팹
    private HealthBarController healthBarController;
    private float maxHealth = 100f;
    private float currentHealth;
    public float defense = 10f; // 방어력

    public ElementType elementType; // 속성 타입
    public MonsterSize sizeType; // 크기 타입

    public GameObject goldPrefab; // 골드 프리팹
    public GameObject gemPrefab; // 젬 프리팹
    public int goldDrop = 10; // 드랍할 골드 수
    public int minGemDrop = 1; // 드랍할 최소 젬 수
    public int maxGemDrop = 3; // 드랍할 최대 젬 수
    public float gemDropChance = 0.2f; // 젬 드랍 확률 (20%)

    private Renderer monsterRenderer; // 몬스터의 렌더러
    private Color originalColor; // 몬스터의 원래 색상

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterRenderer = GetComponentInChildren<Renderer>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            return;
        }

        if (monsterRenderer == null)
        {
            Debug.LogError("Renderer component not found on " + gameObject.name);
            return;
        }

        // NavMeshAgent 설정 조정
        agent.radius = 0.5f;
        agent.height = 2f;
        agent.stoppingDistance = 0.5f;
        agent.avoidancePriority = 50;
        agent.angularSpeed = 500f;
        agent.acceleration = 50f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // 충돌 회피 설정

        agent.enabled = false;  // NavMeshAgent를 초기에는 비활성화합니다.

        currentHealth = maxHealth;
        originalColor = monsterRenderer.material.color;

        // 체력 바 생성
        if (healthBarController == null && healthBarPrefab != null)
        {
            GameObject healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform); // 부모를 몬스터로 설정
            healthBarController = healthBar.GetComponent<HealthBarController>();
            if (healthBarController != null)
            {
                healthBarController.target = this.transform; // 타겟을 현재 몬스터의 Transform으로 설정
                healthBarController.offset = new Vector3(0, 20, 0);
            }
            else
            {
                Debug.LogError("HealthBarController component not found on " + healthBarPrefab.name);
                return;
            }
        }
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
        // 1초 후에 NavMeshAgent 활성화
        Invoke("EnableNavMeshAgent", 1f);
    }

    void EnableNavMeshAgent()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not set or empty for " + gameObject.name);
            return;
        }

        agent.enabled = true;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        if (!agent.enabled || waypoints == null || waypoints.Length == 0) return;

        // Check if the agent is close to the current waypoint
        if (agent.remainingDistance < 15f)
        {
            // Increment the waypoint index
            currentWaypointIndex++;

            // If the current waypoint index is equal to the number of waypoints, reset to 0
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

            // Set the next waypoint as the destination
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public void TakeDamage(float amount)
    {
        float damage = Mathf.Max(0, amount - defense); // 방어력을 적용한 데미지 계산
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (healthBarController != null)
        {
            healthBarController.SetHealth(currentHealth / maxHealth);
        }
        if (currentHealth <= 0)
        {
            DropItems();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashDamage());
        }
    }

    void DropItems()
    {
        // 골드 드랍
        GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        ItemAnimation goldItem = gold.GetComponent<ItemAnimation>();
        if (goldItem != null)
        {
            goldItem.Initialize(ItemType.Gold, goldDrop);
        }

        // 젬 드랍 확률
        if (Random.value < gemDropChance)
        {
            int gemAmount = Random.Range(minGemDrop, maxGemDrop + 1);
            GameObject gem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            ItemAnimation gemItem = gem.GetComponent<ItemAnimation>();
            if (gemItem != null)
            {
                gemItem.Initialize(ItemType.Gem, gemAmount);
            }
        }
    }

    IEnumerator FlashDamage()
    {
        // 투명도가 있는 빨간색으로 변경
        monsterRenderer.material.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.2f); // 0.2초 대기
        // 원래 색상으로 복구
        monsterRenderer.material.color = originalColor;
    }
}

