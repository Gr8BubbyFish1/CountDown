using System.Collections.Generic;
using UnityEngine;

public class TapperController : MonoBehaviour
{
    public enum Blood
    {
        Plasma,
        RedBlood,
        WhiteBlood,
        Platelets
    }
    
    [SerializeField] private GameObject player;
    [SerializeField] private FeedController feedController;
    
    [SerializeField] private GameObject[] bloodBagPrefab;    
    [SerializeField] private float bagSpeed;

    [SerializeField] private float totalServings;
    // [SerializeField] private GameObject[] lane; // I don't think I ever use this? why is this here?

    private void Start()
    {
        List<Blood> bloodFeed = new List<Blood>();
        for (int i = 0; i < totalServings; i++)
        {
            bloodFeed.Add((Blood)Random.Range(0,4));
        }
        Debug.Log(bloodFeed.Count);
        feedController.bloodFeed = bloodFeed;
        feedController.BuildBloodFeed();
    }

    public void ServeMug(TapperController.Blood currentBelt)
    {
        Blood nextServe = feedController.RemoveBlood();
        //summon a new bag 2 units left of the player then send it alooooong
        GameObject newBag = Instantiate(bloodBagPrefab[(int)nextServe], null, true);
        newBag.transform.parent = this.transform;
        newBag.transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y);
        newBag.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-bagSpeed, 0);

        BagBehavior bag = newBag.GetComponent<BagBehavior>();
        bag.tapperController = this;
        bag.bloodType = nextServe;
        bag.belt = currentBelt;
    }

    public void LoseLife()
    { 
        Debug.Log("Damn man you fucking suck");
    }

    public void GameWon()
    {
        Debug.Log("congarts champ");
    }
}
