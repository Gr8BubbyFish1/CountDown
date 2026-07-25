using UnityEngine;
using UnityEngine.InputSystem;

public class BatController : MonoBehaviour
{
    
    // input mgmt
    public InputActionAsset InputAction;
    private InputAction flapAction;

    private Rigidbody2D rb;
    [SerializeField] private float flapForce = 3.0f;

    void Start()
    {
        InputAction.FindActionMap("FlappyBat").Enable();
        
        flapAction = InputAction.FindAction("Flap");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flapAction.triggered)
        {
            Flap();
        }
    }

    void Flap()
    {
        rb.linearVelocityY = flapForce;
    }
}
