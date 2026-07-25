using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    //This code is quite poor. I really should've done one object for the pair of obstacles. But, it works!
    public ObstacleController obstacleController;
    public ObstacleDestroyer pairedObstacle;
    
    private bool isScored = false;
    private bool hitObstacle = false;
    void Update()
    {
        if (!isScored && transform.position.x < -7.8f && obstacleController && hitObstacle == false) //this transform position is the player's position. when the player passes it, they score.
        {
            obstacleController.Score();
            isScored = true;
        }
        
        
        if (transform.position.x < -20)
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        hitObstacle = true;
        if (pairedObstacle)
           pairedObstacle.hitObstacle = true;
        Debug.Log ("You hit the obstacle!");
    }
}
