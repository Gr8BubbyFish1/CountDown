using System;
using UnityEngine;

public class BagBehavior : MonoBehaviour
{
    public TapperController tapperController;
    
    public TapperController.Blood bloodType;
    public TapperController.Blood belt;
    [SerializeField] private float scoringZone;

    // nothin to see here just standard debug statements
    // private void Start()
    // {
    //     Debug.Log($"wheeee I'm a lovely little {bloodType} bag going for a lovely walk down {belt} street");
    // }

    private void Update()
    {
        if (transform.position.x < scoringZone)
        {
            if (bloodType == belt)
            {
                //play score sfx
            }
            else
            {
                tapperController.LoseLife();
            }
            
            Destroy(gameObject);
        }
    }
}
