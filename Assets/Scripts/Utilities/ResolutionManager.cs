using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        // �ػ󵵸� 1920x1080���� �����ϰ� â ���� ����
        Screen.SetResolution(1920, 1080, false);
    }
}