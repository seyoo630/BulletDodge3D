using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        // 해상도를 1920x1080으로 설정하고 창 모드로 설정
        Screen.SetResolution(1920, 1080, false);
    }
}