using System;
using UnityEngine;
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


    private Tuple<float, float> reachableBoundsHolder =  new Tuple<float, float>(5f, -5f);
    void Start()
    {
        spawnDelay = horizontalGap / speed;
        Debug.Log("SpawnDelay set to be " + spawnDelay + " seconds. I sure hope that the units are right");
        
        spawnTimer = spawnDelay;

        GameObject obstacleInitializer = Instantiate(obstacle);
        obstacleHeight = obstacleInitializer.GetComponent<Collider2D>().bounds.size.y;
        Destroy(obstacleInitializer);


        if (speed > 5)
        {
            tightness = 2.5f - Mathf.Sqrt(speed - 5);
            tightness = Mathf.Max(tightness, 0.25f);
        }
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
        
        
        
        // Debug.Log($"Bounds Selected: from {bounds.Item1} to {bounds.Item2}");
        float yCenter = Random.Range(bounds.Item1, bounds.Item2);
        // Debug.Log($"Center Selected: {yCenter}");
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
        Debug.Log($"Current score: {currentScore++}");

        if (currentScore > goalScore)
        {
            Debug.Log($"YOU WIN!!!!");
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
