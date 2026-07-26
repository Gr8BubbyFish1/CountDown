using System;
using UnityEngine;

public class RequestBehavior : MonoBehaviour
{
    //... what have I done?
    public Tuple<TapperController, Tuple<TapperController.Blood,Tuple<FeedController,TapperController.Blood>>> godawfulhorrendousInstantiatorVariable;
    private TapperController tapperController;
    private TapperController.Blood request;
    
    private FeedController feedController;
    private TapperController.Blood requestLocation;

    private void Start()
    {
        tapperController = godawfulhorrendousInstantiatorVariable.Item1;
        request = godawfulhorrendousInstantiatorVariable.Item2.Item1;
        feedController = godawfulhorrendousInstantiatorVariable.Item2.Item2.Item1;
        requestLocation = godawfulhorrendousInstantiatorVariable.Item2.Item2.Item2;

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        BagBehavior incomingBag = other.GetComponent<BagBehavior>();
        if (request == incomingBag.bloodType)
        {
            // Debug.Log($"a type-{request} bag got some action!");
            feedController.activeRequests[(int)requestLocation] = false;
            Destroy(other.gameObject);
            //play score sfx
            if (feedController.bloodFeed.Count < 1)
            {
                tapperController.GameWon();
            }
        }
        else
        {
            Debug.Log($"Nope, you can't match a {incomingBag.bloodType} bag with a {request} request.");
            tapperController.LoseLife();
        }
            
        Destroy(gameObject);
    }
}
