using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Transform target; // 따라다닐 타겟 (몬스터)
    public Vector3 offset;   // 체력 바의 오프셋

    private Camera mainCamera;
    private RectTransform rectTransform;
    private Image healthBarFill;
    private Image healthBarBackground;
    private Canvas canvas;   // 캔버스 추가

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        healthBarFill = transform.Find("HealthBarFill").GetComponent<Image>();
        healthBarBackground = transform.Find("HealthBarBackground").GetComponent<Image>();
        canvas = GetComponent<Canvas>();

        // 투명도 설정
        SetAlpha(0.6f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 체력 바가 타겟의 머리 위에 위치하도록 설정
            transform.position = target.position + offset;

            // 타겟의 회전 중 Y 축 회전만 따라가고, X와 Z 회전은 고정
            Vector3 targetRotation = target.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(90, targetRotation.y, 0); // X는 90도, Z는 0도로 고정
        }
    }

    public void SetHealth(float healthPercent)
    {
        if (healthBarFill != null)
        {
            healthBarFill.rectTransform.localScale = new Vector3(healthPercent, 1, 1);

            // 체력에 따라 색상 변경
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
