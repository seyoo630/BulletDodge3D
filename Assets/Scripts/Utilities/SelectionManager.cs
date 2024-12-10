using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{
    public GameObject selectionIndicatorPrefab;  // ���õ� ��ü �ֺ��� ��Ÿ�� ���� ����Ʈ ������
    public GameObject attackRangeIndicatorPrefab;  // ���� ���� ǥ�� ������
    private GameObject currentSelectionIndicator;
    private GameObject currentSelectedObject;
    private GameObject currentAttackRangeIndicator; // ���� ���� ���� ǥ�� ��ü
    public float selectionRadius = 10.0f;  // ���� �ݰ� ����

    void Update()
    {
        // ���콺 ��Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        // ���콺 ��Ŭ�� ����
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
                if (hit.collider.gameObject.CompareTag("Ground")) // Ground �±� ����
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
                // ���� ���� ����
                if (currentSelectedObject != null)
                {
                    Destroy(currentSelectionIndicator);
                    if (currentAttackRangeIndicator != null)
                    {
                        Destroy(currentAttackRangeIndicator);
                    }
                }

                // ���ο� ��ü ����
                currentSelectedObject = closestObject;
                Debug.Log("Selected Object: " + closestObject.name);
                currentSelectionIndicator = Instantiate(selectionIndicatorPrefab, closestObject.transform.position, Quaternion.identity);
                currentSelectionIndicator.transform.SetParent(closestObject.transform);

                // ���� ���� ǥ��
                if (closestObject.CompareTag("Player"))
                {
                    CharacterController characterController = closestObject.GetComponent<CharacterController>();
                    if (characterController != null)
                    {
                        currentAttackRangeIndicator = characterController.ShowAttackRangeIndicator(attackRangeIndicatorPrefab);
                        SetAlpha(currentAttackRangeIndicator, 0.5f); // ���� ����
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
