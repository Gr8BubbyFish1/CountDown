using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FeedController : MonoBehaviour
{
    [SerializeField] TapperController tapperController;
    //... there's a way to import this so I can just use "Blood", right? ... Probably... ehh, time is of the essence. Learning is for later.
    public List<TapperController.Blood> bloodFeed;
    public bool[] activeRequests = new bool[3];

    [SerializeField] private GameObject[] SpeechBubble;
    [SerializeField] private Vector2[] RequestLocations;

    [SerializeField] private float startingDelay;
    [SerializeField] private float speedUpRate = 0.1f;
    
    private void Start()
    {
        StartCoroutine(NewRequest(4f));
    }
    
    
    private void AddRequest(TapperController.Blood lane)
    {
        
        // what a fucking thing.
        // It is six commands in one.
        // Sickening, truly.
        Instantiate(SpeechBubble[(int)bloodFeed[0]], RequestLocations[(int)lane], Quaternion.identity).GetComponent<RequestBehavior>().godawfulhorrendousInstantiatorVariable = new Tuple<TapperController, Tuple<TapperController.Blood,Tuple<FeedController,TapperController.Blood>>>(tapperController, new Tuple<TapperController.Blood, Tuple<FeedController, TapperController.Blood>>(bloodFeed[0], new Tuple<FeedController, TapperController.Blood>(this,lane)));
        
        /* This singular, disgusting line of code is a genuine pilar of mankind's hubris.
           This is my personal tower of Babel.
           
           I hate my creation.
           And I know, that if it were to feel, it would hate me too.
           It is only justified.
         */
        
        
        // Be grateful, young RemoveAt command. Had you been a proper Pop(), you too would join the homunculus.
        // Your void typing has saved you this day.
        bloodFeed.RemoveAt(0);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Vector2 request in RequestLocations)
        {
            Gizmos.DrawWireCube(request, new Vector3(1, 1, 1));
        }
    }

    IEnumerator NewRequest(float delay)
    {
        yield return new WaitForSeconds(delay);
        int nextOpenBelt = Random.Range(0, 3);
        for (int i = 0; i++ < 3; nextOpenBelt = (nextOpenBelt + 1) % 3)
        {
            if (activeRequests[nextOpenBelt] == false)
            {
                AddRequest((TapperController.Blood)nextOpenBelt);
                activeRequests[nextOpenBelt] = true;
                i = 3;
            }
        }
        StartCoroutine(NewRequest(delay * (1f - speedUpRate)));
    }
    
    
}
