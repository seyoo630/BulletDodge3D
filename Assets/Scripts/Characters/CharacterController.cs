using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterAI;

public class CharacterController : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected CapsuleCollider capsuleCollider; // �浹 �ڽ��� �����ϱ� ���� ����
    protected bool isGrounded;
    public float moveSpeed = 5f;
    public float attackRange = 5f; // ���� ����
    public float attackCooldown = 2f; // ���� ��ٿ� �ð�
    protected float lastAttackTime;
    public float attackDamage = 10f; // ���ݷ�
    public int maxTargets = 1; // �ִ� ���� ������ ���� ��
   

    public AttackType attackType; // ���� ���
    public ElementType elementType; // �Ӽ� Ÿ��
    public CharacterGrade grade; // ĳ���� ���

    protected bool canControl = false; // ���� ���� ����
    protected bool isGettingUp = false; // gettingUp ���� ����
    protected SelectionManager selectionManager;
    protected GameObject attackRangeIndicator; // ���� ���� ǥ�� ��ü

    protected virtual void ApplyGradeBonuses()
    {
        float bonusMultiplier = 1.0f;

        switch (grade)
        {
            case CharacterGrade.Common:
                bonusMultiplier = 1.0f;
                break;
            case CharacterGrade.Rare:
                bonusMultiplier = 1.1f;
                break;
            case CharacterGrade.Epic:
                bonusMultiplier = 1.2f;
                break;
            case CharacterGrade.Legendary:
                bonusMultiplier = 1.3f;
                break;
            case CharacterGrade.Mythical:
                bonusMultiplier = 1.4f;
                break;
            case CharacterGrade.Transcendent:
                bonusMultiplier = 1.5f;
                break;
        }

        moveSpeed *= bonusMultiplier;
        attackRange *= bonusMultiplier;
        attackCooldown /= bonusMultiplier;
        attackDamage *= bonusMultiplier;
    }

    protected virtual void Awake()
    {
        Debug.Log("Awake called.");
        this.enabled = true;

        // Animator ������Ʈ�� Awake���� ���� �Ҵ��մϴ�.
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>(); // �浹 �ڽ��� �����ɴϴ�

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }

        if (capsuleCollider == null)
        {
            Debug.LogError("CapsuleCollider component not found on " + gameObject.name);
        }

        selectionManager = FindObjectOfType<SelectionManager>();

        isGrounded = false;

        // NavMeshAgent�� Base Offset�� �����Ͽ� �ٴڰ��� ���� ���̸� ����ϴ�.
        agent.baseOffset = 0.3f; // �ʿ信 ���� ���� �����ϼ���.

        ApplyGradeBonuses();



    }

    protected virtual void Start()
    {
        Debug.Log("Start called.");
        // �ʱ� �ִϸ��̼� ������ ���⼭ ����� �� �ֽ��ϴ�.
    }

    protected virtual void Update()
    {
        Debug.Log("Update called. canControl: " + canControl);
        if (canControl)
        {
            Debug.Log("Player ready for handle.");
            HandleCombat();
        }

        // �ִϸ��̼� Ʈ����
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // �� ���� �� ����
        if (Time.time > lastAttackTime + attackCooldown)
        {
            DetectAndAttackEnemies();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            if (animator == null)
            {
                Debug.LogError("Animator component is null in OnCollisionEnter");
                return;
            }

            isGrounded = true;
            animator.SetTrigger("fallFlat");
            StartCoroutine(RecoverFromFall());
        }
    }

    protected virtual IEnumerator RecoverFromFall()
    {
        yield return new WaitForSeconds(1f); // �ʿ��� ��� �ð� ����
        animator.SetTrigger("gettingUp");
        isGettingUp = true;
        OnAnimatorMove();

        yield return new WaitForSeconds(1f); // gettingUp �ִϸ��̼� ��� �ð� ����
        isGettingUp = false;
        canControl = true; // ���� �����ϰ� ����
        animator.SetTrigger("breathingIdle");
        Debug.Log("Player ready for movement. canControl: " + canControl);
        this.enabled = true; // ��ũ��Ʈ Ȱ��ȭ Ȯ��
    }

    public bool CanControl()
    {
        return canControl;
    }

    protected virtual void HandleCombat()
    {
        // ���� ������ �����ϰ� �ڵ� ���� ���� �߰�
    }

    protected virtual void DetectAndAttackEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        List<Collider> enemiesInRange = new List<Collider>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemiesInRange.Add(hitCollider);
            }
        }

        if (enemiesInRange.Count > 0)
        {
            animator.SetTrigger("mmaKick");
            lastAttackTime = Time.time;
        }
    }

    public virtual void ApplyDamage() // �ִϸ��̼� �̺�Ʈ���� ȣ��Ǵ� �޼���
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        int hitCount = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                MonsterAI enemy = hitCollider.GetComponent<MonsterAI>();
                if (enemy != null)
                {
                    float finalDamage = CalculateDamage(attackDamage, enemy.defense, enemy.elementType, enemy.sizeType);
                    enemy.TakeDamage(finalDamage);
                    hitCount++;
                    if (hitCount >= maxTargets)
                    {
                        break;
                    }
                }
            }
        }
    }

    protected float CalculateDamage(float baseDamage, float enemyDefense, ElementType enemyElementType, MonsterSize enemySize)
    {
        // ������, Ư����, �������� ���� ������ ���
        float damageMultiplier = 1.0f;
        switch (attackType)
        {
            case AttackType.Physical:
                if (enemySize == MonsterSize.Medium)
                {
                    damageMultiplier = 1.5f; // �������� ���� ���Ϳ��� ���ϴ� ������ ����
                }
                break;
            case AttackType.Special:
                if (enemySize == MonsterSize.Large)
                {
                    damageMultiplier = 1.5f; // Ư������ ���� ���Ϳ��� ���ϴ� ������ ����
                }
                break;
            case AttackType.Magical:
                if (enemySize == MonsterSize.Small)
                {
                    damageMultiplier = 1.5f; // �������� ���� ���Ϳ��� ���ϴ� ������ ����
                }
                break;
        }

        // �Ӽ� Ÿ�Կ� ���� ������ ��� (���÷� �� > �� > Ǯ > ��)
        if ((elementType == ElementType.Water && enemyElementType == ElementType.Fire) ||
            (elementType == ElementType.Fire && enemyElementType == ElementType.Grass) ||
            (elementType == ElementType.Grass && enemyElementType == ElementType.Water))
        {
            damageMultiplier *= 1.5f; // �󼺿� ���� ���� ����
        }

        float damage = baseDamage * damageMultiplier - enemyDefense;
        return Mathf.Max(damage, 0); // �������� �ּ� 0 �̻�
    }

    public virtual GameObject ShowAttackRangeIndicator(GameObject attackRangePrefab)
    {
        if (attackRangeIndicator != null)
        {
            Destroy(attackRangeIndicator);
        }

        attackRangeIndicator = Instantiate(attackRangePrefab, transform.position, Quaternion.identity);
        attackRangeIndicator.transform.SetParent(transform);
        attackRangeIndicator.transform.localScale = new Vector3(attackRange * 0.02f, 0.1f, attackRange * 0.02f); // ������ ũ��� ����
        return attackRangeIndicator;
    }

    protected virtual void OnAnimatorMove()
    {
        // gettingUp �ִϸ��̼� ���� ��ġ ���� �ʿ� �� �߰�
        if (isGettingUp)
        {
            agent.baseOffset = 0.85f; // �ִϸ��̼� ���� NavMeshAgent�� Base Offset�� ����
            // gettingUp �ִϸ��̼��� ���� �� �浹 �ڽ��� center ���� ����
            capsuleCollider.center = Vector3.zero;
        }
    }

}