using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private RectTransform front;
    public float startTime;
    private float currentTime;
    private bool timeUp;
    
    private void Start()
    {
        currentTime = startTime;
    }

    private void Update()
    {
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
