using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanRun : MonoBehaviour
{
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [SerializeField] private GameObject player;
    [SerializeField] private float minDisFromPlayer;
    [SerializeField] private float minDisFromSelf;
    [SerializeField] private SpriteRenderer skin;

    public float maxHealth;
    public float health;
    public float invincibleTime;
    private bool invincible;
    public float speed = 5f;

    private Vector2 target;
    

    void Start()
    {
        PickNewTarget();
        health = maxHealth;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            target,speed * Time.deltaTime);
        
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        

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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!invincible)
                StartCoroutine(takeDamage());
        }
    }

    private IEnumerator takeDamage()
    {
        invincible = true;
        health--;
        Color skintone = skin.color;
        float alpha = (health / maxHealth) * 100;
        skintone.a = alpha;
        skin.color = skintone;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;

    }
}
