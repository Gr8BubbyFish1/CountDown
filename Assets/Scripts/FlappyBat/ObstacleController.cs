using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ObstacleController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstacle;
    private float obstacleHeight;
    
    [SerializeField] private float verticalGap;
    [SerializeField] private float horizontalGap;
    [SerializeField] private float speed;

    [SerializeField] private int goalScore = 10;
    private int currentScore;
    
    
    [SerializeField] private float ScreenTop;
    [SerializeField] private float ScreenBottom;

    private float spawnDelay;
    private float spawnTimer;

    private float lastGapCenter = 0.0f;
    private float tightness = 2.5f; //this value constrains how far (vertically) the gap of the next Obstacle will be from that of the last Obstacle. As speed increases, this should shrink.
    
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool gameEnding = false;
    
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private Tuple<float, float> reachableBoundsHolder =  new Tuple<float, float>(5f, -5f);
    void Start()
    {
        DifficultySelect(StaticManager.totalRoundsPlayed);
        spawnDelay = horizontalGap / speed;
        Debug.Log("SpawnDelay set to be " + spawnDelay + " seconds. I sure hope that the units are right");
        
        spawnTimer = spawnDelay + 3;

        GameObject obstacleInitializer = Instantiate(obstacle);
        obstacleHeight = obstacleInitializer.GetComponent<Collider2D>().bounds.size.y;
        Destroy(obstacleInitializer);


        if (speed > 5)
        {
            tightness = 2.5f - Mathf.Sqrt(speed - 5);
            tightness = Mathf.Max(tightness, 0.25f);
        }
        scoreText.text = $"Stakes:\n0/{goalScore}";
        // StaticManager.setTimer(spawnDelay * goalScore + 3 + 2);
    }

    void Update()
    {
        reachableBoundsHolder = reachableBounds();
        
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnNextWave();
            spawnTimer = spawnDelay;
        }
    }

    void spawnNextWave()
    {
        Tuple<float,float> bounds = reachableBounds();
        //clamp the bounds to be within the screen
        bounds = new Tuple<float, float>(Mathf.Min(bounds.Item1, 5), Mathf.Min(bounds.Item2, player.transform.position.y));
        
        
        
        float yCenter = Random.Range(bounds.Item1, bounds.Item2);
        lastGapCenter = yCenter;
        
        Vector3 upperPosition = new Vector2(transform.position.x, 
            yCenter + obstacleHeight/2 + verticalGap/2);
        Vector3 lowerPosition = new Vector2(transform.position.x, 
            yCenter - obstacleHeight/2 - verticalGap/2);
        
        //I hate that you have to see this frankly disgusting obstacle pairing code. It ought to be better, and I am ashamed that it is not.
        GameObject upperObstacle = Instantiate(obstacle, upperPosition, Quaternion.Euler(180,0,0));
        upperObstacle.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed, 0);
        upperObstacle.GetComponent<ObstacleDestroyer>().obstacleController = this;
        
        GameObject lowerObstacle = Instantiate(obstacle, lowerPosition, Quaternion.identity);
        lowerObstacle.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed, 0);
        lowerObstacle.GetComponent<ObstacleDestroyer>().pairedObstacle = upperObstacle.GetComponent<ObstacleDestroyer>();
        
        
    }

    private Tuple<float, float> reachableBounds()
    {
        //clamps the "center" so that the reachableBounds will always be on screen.
        float clampedCenter = Mathf.Clamp(lastGapCenter,  ScreenBottom + tightness, ScreenTop - tightness);
        
        return new Tuple<float, float>(clampedCenter + tightness, clampedCenter - tightness);
        // more math to be done here if we want to be really precise
    }


    public void Score()
    {
        currentScore++;
        scoreText.text = $"Stakes:\n{currentScore}/{goalScore}";
        Debug.Log($"Current score: {currentScore}");

        if (currentScore >= goalScore)
        {
            Debug.Log($"YOU WIN!!!!");
            gameEnding = true;
            audioManager.PlaySFX(audioManager.winSFX);
            StaticManager.EndMiniGame();
        }
    }

    public void Hit()
    {
        if (!gameEnding)
        {
            gameEnding = true;
            Debug.Log("You hit the obstacle!");
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }
    
    private void DifficultySelect(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                goalScore = 10;
                break;
            case 1:
                goalScore = 12;
                horizontalGap = 3.5f;
                verticalGap = 5.5f;
                break;
            case 2:
                goalScore = 15;
                horizontalGap = 3.5f;
                verticalGap = 5.5f;
                speed = 8;
                break;
            case 3:
                goalScore = 18;
                horizontalGap = 3.5f;
                verticalGap = 5f;
                speed = 10;
                break;
            case 4:
                goalScore = 25;
                horizontalGap = 2.0f;
                verticalGap = 4f;
                speed = 15;
                break;
            default:
                goalScore = 25 + (difficulty - 5) * 5;
                horizontalGap = 2.0f;
                verticalGap = 3.5f;
                speed = 20;
                break;
        }
    }


    protected void OnDrawGizmos()
    {
        //show reachableBounds
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector2(player.transform.position.x, (reachableBoundsHolder.Item1 + reachableBoundsHolder.Item2) / 2),
            new Vector2(horizontalGap, Math.Abs(reachableBoundsHolder.Item1) + Math.Abs(reachableBoundsHolder.Item2))
        );
    }
}
