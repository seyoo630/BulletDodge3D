using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  
    private float startTime; // 게임 시작 시간

    private void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
    }

    private void Update()
    {
        float t = Time.time - startTime; // 경과 시간 계산
        string minutes = ((int)t / 60).ToString(); 
        string seconds = (t % 60).ToString("f2"); 

        timerText.text = "Time: " + minutes + ":" + seconds; 
    }
}
