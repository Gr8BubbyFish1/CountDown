using UnityEngine;

public class importantTitleController : MonoBehaviour
{
    
    void Update()
    {
        this.gameObject.transform.Rotate(0f, 0f, Time.deltaTime);
    }
}
