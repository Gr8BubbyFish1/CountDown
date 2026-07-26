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

    private bool endTapper;

    [SerializeField] private TextMeshProUGUI ScoreText;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        endTapper = false;
        DifficultySelect(StaticManager.totalRoundsPlayed);
        
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

        if (StaticManager.hasTimeRunOut() && !endTapper)    
            LoseLife(); 
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
        if (!endTapper)
        {
            endTapper = true;
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
            Debug.Log("Damn man you fucking suck");
        }
    }

    public void GameWon()
    {
        if (!endTapper)
        {
            Debug.Log("congarts champ");
            endTapper = true;
            audioManager.PlaySFX(audioManager.winSFX);
            StaticManager.EndMiniGame();
        }
    }

    private void DifficultySelect(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                totalServings = 8;
                break;
            case 1:
                totalServings = 10;
                break;
            case 2:
                totalServings = 14;
                feedController.startingDelay = 2;
                break;
            case 3:
                totalServings = 18;
                feedController.startingDelay = 2;
                break;
            case 4:
                totalServings = 20;
                feedController.startingDelay = 1.5f;
                break;
            default:
                totalServings = 20 + (difficulty - 5) * 5;
                feedController.startingDelay = 1f;
                break;
        }
    }
}
