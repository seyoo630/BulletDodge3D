using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{
    public GameObject selectionIndicatorPrefab;  // 선택된 개체 주변에 나타날 원형 이펙트 프리팹
    public GameObject attackRangeIndicatorPrefab;  // 공격 범위 표시 프리팹
    private GameObject currentSelectionIndicator;
    private GameObject currentSelectedObject;
    private GameObject currentAttackRangeIndicator; // 현재 공격 범위 표시 객체
    public float selectionRadius = 10.0f;  // 선택 반경 설정

    void Update()
    {
        // 마우스 좌클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        // 마우스 우클릭 감지
        if (Input.GetMouseButtonDown(1))
        {
            MoveSelectedObject();
        }
    }

    void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray, selectionRadius);

        if (hits.Length > 0)
        {
            GameObject closestObject = null;
            float closestDistance = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Ground")) // Ground 태그 무시
                {
                    continue;
                }

                float distance = Vector3.Distance(ray.origin, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = hit.collider.gameObject;
                }
            }

            if (closestObject != null)
            {
                // 기존 선택 해제
                if (currentSelectedObject != null)
                {
                    Destroy(currentSelectionIndicator);
                    if (currentAttackRangeIndicator != null)
                    {
                        Destroy(currentAttackRangeIndicator);
                    }
                }

                // 새로운 개체 선택
                currentSelectedObject = closestObject;
                Debug.Log("Selected Object: " + closestObject.name);
                currentSelectionIndicator = Instantiate(selectionIndicatorPrefab, closestObject.transform.position, Quaternion.identity);
                currentSelectionIndicator.transform.SetParent(closestObject.transform);

                // 공격 범위 표시
                if (closestObject.CompareTag("Player"))
                {
                    CharacterController characterController = closestObject.GetComponent<CharacterController>();
                    if (characterController != null)
                    {
                        currentAttackRangeIndicator = characterController.ShowAttackRangeIndicator(attackRangeIndicatorPrefab);
                        SetAlpha(currentAttackRangeIndicator, 0.5f); // 투명도 설정
                    }
                }
            }
        }
    }

    void MoveSelectedObject()
    {
        if (currentSelectedObject != null && currentSelectedObject.CompareTag("Player"))
        {
            Debug.Log("Move command issued for: " + currentSelectedObject.name);
            NavMeshAgent agent = currentSelectedObject.GetComponent<NavMeshAgent>();
            CharacterController characterController = currentSelectedObject.GetComponent<CharacterController>();
            if (agent != null && characterController != null && characterController.CanControl())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Setting destination to: " + hit.point);
                    agent.SetDestination(hit.point);
                }
            }
        }
    }

    public GameObject GetSelectedObject()
    {
        return currentSelectedObject;
    }

    void SetAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }
}
