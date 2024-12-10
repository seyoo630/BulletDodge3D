using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mapCenter;  // 맵 중심점
    public float height = 20f;   // 카메라 높이
    public Vector3 offset;       // 추가적인 오프셋
    public float orthographicSize = 10f;  // Orthographic 카메라의 세로 범위

    void Start()
    {
        if (mapCenter != null)
        {
            // 카메라 위치 설정
            transform.position = mapCenter.position + Vector3.up * height + offset;

            // 카메라가 맵 중심을 향하도록 회전
            transform.LookAt(mapCenter.position);

            // Orthographic 카메라 설정
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = orthographicSize;

            // 해상도에 맞춰 Viewport Rect 설정
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
        // 카메라가 180도 회전하여 맵을 아래에서 위로 보도록 설정
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
