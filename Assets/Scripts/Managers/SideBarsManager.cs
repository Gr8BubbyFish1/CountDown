using System;
using TMPro;
using UnityEngine;

public class SideBarsManager : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI round;
    [SerializeField] private TextMeshProUGUI lives;
    
    private void Awake()
    {
        StaticManager.sideBarsManager = this;
    }

    private void Start()
    {
        UpdateSideBars();
    }

    public void UpdateSideBars()
    {
        round.text = "Round\n" + StaticManager.roundNumber + " / 5";
        lives.text = "Lives\n" + StaticManager.lives;
    }

    public void setMaxTime(float time)
    {
        //Debug.Log("TimerFireC");
        timer.startTime = time;
    }

    public float getCurrentTime()
    {
        return timer.getCurrentTime();
    }
    
    public bool hasTimeRunOut()
    {
        return timer.hasTimeRunOut();
    }
}
