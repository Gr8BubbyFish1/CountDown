using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatController : MonoBehaviour
{
    
    // input mgmt
    public InputActionAsset InputAction;
    private InputAction flapAction;

    private Rigidbody2D rb;
    [SerializeField] private float flapForce = 3.0f;
    
    private SpriteRenderer sr;
    [SerializeField] private Sprite wingUp;
    [SerializeField] private Sprite wingDown;

    private float wingResetTimer;
    
    void Start()
    {
        InputAction.FindActionMap("FlappyBat").Enable();
        
        flapAction = InputAction.FindAction("Flap");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flapAction.triggered)
        {
            Flap();
        }

        if (wingResetTimer > 0)
        {
            wingResetTimer -= Time.deltaTime;
        }
        else {
            sr.sprite = wingUp;
        }
    }

    void Flap()
    {
        rb.linearVelocityY = flapForce;


        sr.sprite = wingDown;
        if (wingResetTimer > 0) //as in, waiting to release the downwards flap
        {
            StartCoroutine(microflap());
        }
        wingResetTimer = 0.4f;
    }

    IEnumerator microflap()
    {
        sr.sprite = wingUp;
        yield return new WaitForSeconds(0.05f);
        sr.sprite = wingDown;
    }
}
