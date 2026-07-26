using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private RectTransform front;
    public float startTime;
    public float currentTime = -1;
    private bool timeUp;

    private bool startFlag;

    private void Update()
    {
        if (!startFlag)
        {
            //Debug.Log("TimerFireD");
            startFlag = true;
            currentTime = startTime;
        }
        
        if (!StaticManager.transitionPlaying)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                float newScale = Mathf.Lerp(front.localScale.y,
                    Mathf.Clamp01(currentTime / startTime), Time.deltaTime);
                front.localScale = new Vector2(1, newScale);
            }
            else if (!timeUp)
            {
                timeUp = true;
            }
        }
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

    public bool hasTimeRunOut()
    {
        return timeUp;
    }
}
