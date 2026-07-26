using System;
using UnityEngine;

public class BagBehavior : MonoBehaviour
{
    public TapperController.Blood bloodType;
    //this... this really doesn't need to be its own script, does it?

    private void Update()
    {
        if (transform.position.x < -10)
            Destroy(gameObject);
    }
}
