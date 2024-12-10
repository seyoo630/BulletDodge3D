using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterAI;

public class CharacterController : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected CapsuleCollider capsuleCollider; // 충돌 박스를 설정하기 위한 변수
    protected bool isGrounded;
    public float moveSpeed = 5f;
    public float attackRange = 5f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운 시간
    protected float lastAttackTime;
    public float attackDamage = 10f; // 공격력
    public int maxTargets = 1; // 최대 공격 가능한 적의 수
   

    public AttackType attackType; // 공격 방식
    public ElementType elementType; // 속성 타입
    public CharacterGrade grade; // 캐릭터 등급

    protected bool canControl = false; // 조작 가능 여부
    protected bool isGettingUp = false; // gettingUp 상태 여부
    protected SelectionManager selectionManager;
    protected GameObject attackRangeIndicator; // 공격 범위 표시 객체

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

        // Animator 컴포넌트를 Awake에서 직접 할당합니다.
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>(); // 충돌 박스를 가져옵니다

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

        // NavMeshAgent의 Base Offset을 조정하여 바닥과의 높이 차이를 맞춥니다.
        agent.baseOffset = 0.3f; // 필요에 따라 값을 조정하세요.

        ApplyGradeBonuses();



    }

    protected virtual void Start()
    {
        Debug.Log("Start called.");
        // 초기 애니메이션 설정은 여기서 계속할 수 있습니다.
    }

    protected virtual void Update()
    {
        Debug.Log("Update called. canControl: " + canControl);
        if (canControl)
        {
            Debug.Log("Player ready for handle.");
            HandleCombat();
        }

        // 애니메이션 트리거
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // 적 감지 및 공격
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
        yield return new WaitForSeconds(1f); // 필요한 대기 시간 설정
        animator.SetTrigger("gettingUp");
        isGettingUp = true;
        OnAnimatorMove();

        yield return new WaitForSeconds(1f); // gettingUp 애니메이션 대기 시간 설정
        isGettingUp = false;
        canControl = true; // 조작 가능하게 설정
        animator.SetTrigger("breathingIdle");
        Debug.Log("Player ready for movement. canControl: " + canControl);
        this.enabled = true; // 스크립트 활성화 확인
    }

    public bool CanControl()
    {
        return canControl;
    }

    protected virtual void HandleCombat()
    {
        // 수동 공격을 제거하고 자동 공격 로직 추가
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

    public virtual void ApplyDamage() // 애니메이션 이벤트에서 호출되는 메서드
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
        // 물리형, 특수형, 마법형에 따른 데미지 계산
        float damageMultiplier = 1.0f;
        switch (attackType)
        {
            case AttackType.Physical:
                if (enemySize == MonsterSize.Medium)
                {
                    damageMultiplier = 1.5f; // 물리형이 중형 몬스터에게 가하는 데미지 증가
                }
                break;
            case AttackType.Special:
                if (enemySize == MonsterSize.Large)
                {
                    damageMultiplier = 1.5f; // 특수형이 대형 몬스터에게 가하는 데미지 증가
                }
                break;
            case AttackType.Magical:
                if (enemySize == MonsterSize.Small)
                {
                    damageMultiplier = 1.5f; // 마법형이 소형 몬스터에게 가하는 데미지 증가
                }
                break;
        }

        // 속성 타입에 따른 데미지 계산 (예시로 물 > 불 > 풀 > 물)
        if ((elementType == ElementType.Water && enemyElementType == ElementType.Fire) ||
            (elementType == ElementType.Fire && enemyElementType == ElementType.Grass) ||
            (elementType == ElementType.Grass && enemyElementType == ElementType.Water))
        {
            damageMultiplier *= 1.5f; // 상성에 따른 배율 예시
        }

        float damage = baseDamage * damageMultiplier - enemyDefense;
        return Mathf.Max(damage, 0); // 데미지는 최소 0 이상
    }

    public virtual GameObject ShowAttackRangeIndicator(GameObject attackRangePrefab)
    {
        if (attackRangeIndicator != null)
        {
            Destroy(attackRangeIndicator);
        }

        attackRangeIndicator = Instantiate(attackRangePrefab, transform.position, Quaternion.identity);
        attackRangeIndicator.transform.SetParent(transform);
        attackRangeIndicator.transform.localScale = new Vector3(attackRange * 0.02f, 0.1f, attackRange * 0.02f); // 반지름 크기로 조정
        return attackRangeIndicator;
    }

    protected virtual void OnAnimatorMove()
    {
        // gettingUp 애니메이션 동안 위치 조정 필요 시 추가
        if (isGettingUp)
        {
            agent.baseOffset = 0.85f; // 애니메이션 동안 NavMeshAgent의 Base Offset을 조정
            // gettingUp 애니메이션이 끝난 후 충돌 박스의 center 값을 수정
            capsuleCollider.center = Vector3.zero;
        }
    }

}