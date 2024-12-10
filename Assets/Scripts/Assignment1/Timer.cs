using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  
    private float startTime; // ���� ���� �ð�

    private void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
    }

    private void Update()
    {
        float t = Time.time - startTime; // ��� �ð� ���
        string minutes = ((int)t / 60).ToString(); 
        string seconds = (t % 60).ToString("f2"); 

        timerText.text = "Time: " + minutes + ":" + seconds; 
    }
}
