using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private RectTransform front;
    public float startTime;
    private float currentTime;
    
    private void Start()
    {
        currentTime = startTime;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            float newScale = Mathf.Lerp(front.localScale.x,
                Mathf.Clamp01(currentTime / startTime), Time.deltaTime);
            front.localScale = new Vector2(newScale, 1);
        }
        else
        {
            TimesUpAction();
        }
    }

    public void TimesUpAction()
    {
        Debug.Log("Times up!");
    }
}
