using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Transform target; // ����ٴ� Ÿ�� (����)
    public Vector3 offset;   // ü�� ���� ������

    private Camera mainCamera;
    private RectTransform rectTransform;
    private Image healthBarFill;
    private Image healthBarBackground;
    private Canvas canvas;   // ĵ���� �߰�

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        healthBarFill = transform.Find("HealthBarFill").GetComponent<Image>();
        healthBarBackground = transform.Find("HealthBarBackground").GetComponent<Image>();
        canvas = GetComponent<Canvas>();

        // ���� ����
        SetAlpha(0.6f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // ü�� �ٰ� Ÿ���� �Ӹ� ���� ��ġ�ϵ��� ����
            transform.position = target.position + offset;

            // Ÿ���� ȸ�� �� Y �� ȸ���� ���󰡰�, X�� Z ȸ���� ����
            Vector3 targetRotation = target.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(90, targetRotation.y, 0); // X�� 90��, Z�� 0���� ����
        }
    }

    public void SetHealth(float healthPercent)
    {
        if (healthBarFill != null)
        {
            healthBarFill.rectTransform.localScale = new Vector3(healthPercent, 1, 1);

            // ü�¿� ���� ���� ����
            if (healthPercent < 0.3f)
            {
                healthBarFill.color = Color.red;
            }
            else if (healthPercent < 0.7f)
            {
                healthBarFill.color = Color.yellow;
            }
            else
            {
                healthBarFill.color = Color.green;
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        if (healthBarFill != null)
        {
            Color fillColor = healthBarFill.color;
            fillColor.a = alpha;
            healthBarFill.color = fillColor;
        }

        if (healthBarBackground != null)
        {
            Color backgroundColor = healthBarBackground.color;
            backgroundColor.a = alpha;
            healthBarBackground.color = backgroundColor;
        }
    }
}
