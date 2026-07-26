using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanRun : MonoBehaviour
{
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [SerializeField] private GameObject player;
    [SerializeField] private float minDisFromPlayer;
    [SerializeField] private float minDisFromSelf;

    public float health;
    public float speed = 5f;

    private Vector2 target;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            target,speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        target = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
        
        float distance = Vector2.Distance(target, player.transform.position);
        if (distance < minDisFromPlayer)
            PickNewTarget();
        
        distance = Vector2.Distance(target, transform.position);
        if(distance < minDisFromSelf)
            PickNewTarget();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            PickNewTarget();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            health -= Time.deltaTime;
        }
    }
}
