using UnityEngine;
using UnityEngine.InputSystem;

public class TapperPlayerController : MonoBehaviour
{
    // input mgmt
    public InputActionAsset InputAction;
    private InputAction moveAction;
    private InputAction fillAction;

    [SerializeField] private TapperController tapperController;
    
    
    [SerializeField] private float fillDuration;
    private float fillTimer;
    
    [SerializeField] private GameObject[] ActiveBelts;

    private AudioSource playerSFX;
    [SerializeField] private AudioClip fillingSFX;
    bool playedFillingSFX = false;
    [SerializeField] private AudioClip fullSFX;
    bool playedFullSFX = false;

    
    private TapperController.Blood playerPosition = TapperController.Blood.Platelets;

    void Start()
    {
        InputAction.FindActionMap("Tapper").Enable();
        
        moveAction = InputAction.FindAction("Move");
        fillAction = InputAction.FindAction("FillOrServe");
        
        fillTimer = fillDuration;
        
        playerSFX = GetComponent<AudioSource>();
        playerSFX.clip = fillingSFX;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.WasPressedThisFrame()) {
            ChangeLane(moveAction.ReadValue<float>());
        }

        if (fillAction.IsPressed())
        {
            //SPRITE - set filling
            if (fillTimer > 0)
            {
                fillTimer -= Time.deltaTime;
                if(!playedFillingSFX)
                {
                    playerSFX.Play();
                    playedFillingSFX = true;
                }
            }
            else
            {
                if (playedFillingSFX)
                {
                    playerSFX.Stop();
                    playedFillingSFX = false;
                }
                if (!playedFullSFX)
                {
                    playedFullSFX = true;
                    playerSFX.PlayOneShot(fullSFX);
                }
            }
        }
        else
        {
            if (playedFillingSFX)
                playerSFX.Stop();
            playedFillingSFX = false;
            playedFullSFX = false;
            if (fillTimer < 0)
            {
                tapperController.ServeMug(playerPosition);
            }
            fillTimer = fillDuration;
        }
    }
    
    public void ChangeLane(float change)
    {
        //kinda sloppy, but it makes it so the guy wraps around. The "+ ActiveBelts.Length" is done so the code is never negative.
        playerPosition = (TapperController.Blood)(
            ((int)playerPosition + (int)change + ActiveBelts.Length) % ActiveBelts.Length 
            );
        //convert it to a vector so the character moves where he supposed to
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, ActiveBelts[(int)playerPosition].transform.position.y);
    }
}
