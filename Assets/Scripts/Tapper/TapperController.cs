using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TapperController : MonoBehaviour
{
    public enum Blood
    {
        A,
        B,
        O,
    }
    
    [SerializeField] private GameObject player;
    [SerializeField] private FeedController feedController;
    
    [SerializeField] private GameObject[] bloodBagPrefab;    
    [SerializeField] private float bagSpeed;

    [SerializeField] private float totalServings;
    // [SerializeField] private GameObject[] lane; // I don't think I ever use this? why is this here?
    
    [SerializeField] private TextMeshProUGUI ScoreText;

    private void Start()
    {
        List<Blood> bloodFeed = new List<Blood>();
        for (int i = 0; i < totalServings; i++)
        {
            bloodFeed.Add((Blood)Random.Range(0,3));
        }
        Debug.Log(bloodFeed.Count);
        feedController.bloodFeed = bloodFeed;
        
        ScoreText.text = $"Served:\n0/{totalServings}";
    }

    private void Update()
    {
        //This is terrible practice, but I have mangled this minigame's code enough to go back now. 
        ScoreText.text = $"Served:\n {totalServings - feedController.bloodFeed.Count}/{totalServings}";
    }

    public void ServeMug(Blood currentBelt, Blood? mug)
    {
        if (mug == null)
        {
            Debug.LogError("You sent a mug full of nothin somehow");
            return;
        }
        Blood nextServe = (Blood)mug;
        //summon a new bag 2 units left of the player then send it alooooong
        GameObject newBag = Instantiate(bloodBagPrefab[(int)nextServe], null, true);
        newBag.transform.parent = this.transform;
        newBag.transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y);
        newBag.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-bagSpeed, 0);

        BagBehavior bag = newBag.GetComponent<BagBehavior>();
        bag.bloodType = nextServe;
    }

    public void LoseLife()
    { 
        StaticManager.removeLife();
        StaticManager.EndMiniGame();
        Debug.Log("Damn man you fucking suck");
    }

    public void GameWon()
    {
        Debug.Log("congarts champ");
        StaticManager.EndMiniGame();
    }
}
