using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mapCenter;  // �� �߽���
    public float height = 20f;   // ī�޶� ����
    public Vector3 offset;       // �߰����� ������
    public float orthographicSize = 10f;  // Orthographic ī�޶��� ���� ����

    void Start()
    {
        if (mapCenter != null)
        {
            // ī�޶� ��ġ ����
            transform.position = mapCenter.position + Vector3.up * height + offset;

            // ī�޶� �� �߽��� ���ϵ��� ȸ��
            transform.LookAt(mapCenter.position);

            // Orthographic ī�޶� ����
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = orthographicSize;

            // �ػ󵵿� ���� Viewport Rect ����
            SetCameraViewport(1920f, 1080f);

            AdjustCameraRotation();

        }
        else
        {
            Debug.LogError("Map center not set for CameraController.");
        }
    }

    void AdjustCameraRotation()
    {
        // ī�޶� 180�� ȸ���Ͽ� ���� �Ʒ����� ���� ������ ����
        transform.rotation = Quaternion.Euler(90f, 180f, 0f);
    }

    void SetCameraViewport(float targetWidth, float targetHeight)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = targetWidth / targetHeight;

        if (screenAspect >= targetAspect)
        {
            // Screen is wider than target aspect ratio
            float viewportWidth = targetAspect / screenAspect;
            Camera.main.rect = new Rect((1f - viewportWidth) / 2f, 0f, viewportWidth, 1f);
        }
        else
        {
            // Screen is taller than target aspect ratio
            float viewportHeight = screenAspect / targetAspect;
            Camera.main.rect = new Rect(0f, (1f - viewportHeight) / 2f, 1f, viewportHeight);
        }
    }
}
