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
    [SerializeField] private GameObject blood;

    public float maxHealth;
    public float health;
    public float invincibleTime;
    private bool invincible;
    public float speed = 5f;

    private Vector2 target;
    private bool winChase;
    

    void Start()
    {
        PickNewTarget();
        health = maxHealth;
    }

    void Update()
    {
        if (health > 0)
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
        
        if(health <= 0 && !winChase)
        {
            winChase = true;
            player.GetComponent<PlayerManager>().setCanMove(false);
            StaticManager.EndMiniGame();
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
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!invincible && health > 0)
                StartCoroutine(takeDamage());
        }
    }

    private IEnumerator takeDamage()
    {
        invincible = true;
        health--;
        Instantiate(blood, this.transform);
        Color skintone = skin.color;
        float alpha = health / maxHealth;
        skintone.a = alpha;
        skin.color = skintone;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }
}
