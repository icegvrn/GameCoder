using UnityEngine;

    public class CustomTimer
    {

    float floatTimer;
    int timer;
    public int Timer { get { return timer; } set { timer = value; } }

    bool timerIsStarted;
    public bool TimerIsStarted { get { return timerIsStarted; } set { timerIsStarted = value;} }


    public void Init()
    {
        Reset();
    }

    public void Update()
    {
       
        if (timerIsStarted)
        {

            floatTimer += Time.deltaTime;
            timer = Mathf.FloorToInt(floatTimer);

        }
    }

     void Reset()
    {
        floatTimer = 0;
        timer = 0;
        timerIsStarted = false;
    }

    public void Start()
    {
        Debug.Log("Timer : je start");
        timerIsStarted = true;
    }

    public void Stop()
    {
        Debug.Log("Timer : je stop");
        Reset();
    }

    public int GetValue()
    {
        return timer;
    }

    public float GetFloatValue()
    {
        return floatTimer;
    }



}

