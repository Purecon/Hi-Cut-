using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class QuizTimer : MonoBehaviour
{
    [Header("Bar")]
    public Slider timeBar;

    [Header("Time")]
    float currentTime;
    public float quizTime=20f;
    public float timePenalty = 7.5f;
    public bool timerActive = false;

    //Event
    public static event Action QuizTimeUp;

    public void StartTime()
    {
        currentTime = quizTime;
        timerActive = true;
    }

    public void StopTime()
    {
        currentTime = quizTime;
        timerActive = false;
    }

    public void PenaltyTime()
    {
        currentTime -= timePenalty;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            timeBar.value = (currentTime / quizTime);

            if (currentTime <= 0)
            {
                QuizTimeUp?.Invoke();
                timerActive = false;
            }
        }
    }
}
