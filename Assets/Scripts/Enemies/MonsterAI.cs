using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public GameObject healthBarPrefab; // ü�� �� ������
    private HealthBarController healthBarController;
    private float maxHealth = 100f;
    private float currentHealth;
    public float defense = 10f; // ����

    public ElementType elementType; // �Ӽ� Ÿ��
    public MonsterSize sizeType; // ũ�� Ÿ��

    public GameObject goldPrefab; // ��� ������
    public GameObject gemPrefab; // �� ������
    public int goldDrop = 10; // ����� ��� ��
    public int minGemDrop = 1; // ����� �ּ� �� ��
    public int maxGemDrop = 3; // ����� �ִ� �� ��
    public float gemDropChance = 0.2f; // �� ��� Ȯ�� (20%)

    private Renderer monsterRenderer; // ������ ������
    private Color originalColor; // ������ ���� ����

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

        // NavMeshAgent ���� ����
        agent.radius = 0.5f;
        agent.height = 2f;
        agent.stoppingDistance = 0.5f;
        agent.avoidancePriority = 50;
        agent.angularSpeed = 500f;
        agent.acceleration = 50f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // �浹 ȸ�� ����

        agent.enabled = false;  // NavMeshAgent�� �ʱ⿡�� ��Ȱ��ȭ�մϴ�.

        currentHealth = maxHealth;
        originalColor = monsterRenderer.material.color;

        // ü�� �� ����
        if (healthBarController == null && healthBarPrefab != null)
        {
            GameObject healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform); // �θ� ���ͷ� ����
            healthBarController = healthBar.GetComponent<HealthBarController>();
            if (healthBarController != null)
            {
                healthBarController.target = this.transform; // Ÿ���� ���� ������ Transform���� ����
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
        // 1�� �Ŀ� NavMeshAgent Ȱ��ȭ
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
        float damage = Mathf.Max(0, amount - defense); // ������ ������ ������ ���
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
        // ��� ���
        GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        ItemAnimation goldItem = gold.GetComponent<ItemAnimation>();
        if (goldItem != null)
        {
            goldItem.Initialize(ItemType.Gold, goldDrop);
        }

        // �� ��� Ȯ��
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
        // ������ �ִ� ���������� ����
        monsterRenderer.material.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.2f); // 0.2�� ���
        // ���� �������� ����
        monsterRenderer.material.color = originalColor;
    }
}

