using System;
using System.Collections;
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
    [SerializeField] private Vector2[] BeltLocations;

    private AudioSource playerSFX;
    [SerializeField] private AudioClip fillingSFX;
    bool playedFillingSFX = false;
    [SerializeField] private AudioClip fullSFX;
    bool playedFullSFX = false;

    
    private TapperController.Blood playerPosition = TapperController.Blood.O;
    private TapperController.Blood? currentMug;
    
    
    private SpriteRenderer sr;
    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite stillSprite;

    void Start()
    {
        InputAction.FindActionMap("Tapper").Enable();
        
        moveAction = InputAction.FindAction("MoveBartender");
        fillAction = InputAction.FindAction("FillOrServe");
        
        fillTimer = fillDuration;
        
        playerSFX = GetComponent<AudioSource>();
        playerSFX.clip = fillingSFX;
        
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.WasPressedThisFrame()) {
            ChangeLane(moveAction.ReadValue<float>());
        }

        if (fillAction.IsPressed())
        {
            sr.sprite = stillSprite;
            if (fillTimer > 0)
            {
                fillTimer -= Time.deltaTime;
                if(!playedFillingSFX)
                {
                    playerSFX.Play();
                    playedFillingSFX = true;
                }
                sr.flipX = true; //while filling, face the spout
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
                    currentMug = playerPosition;
                }
                
                sr.flipX = false; //face the customer
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
                tapperController.ServeMug(playerPosition, currentMug);
                currentMug = null;
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
        gameObject.transform.position = BeltLocations[(int)playerPosition];

        StartCoroutine(step());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector2 belt in BeltLocations)
        {
            Gizmos.DrawWireCube(belt, new Vector3(1, 1, 1));
        }
    }
    
    IEnumerator step()
    {
        sr.sprite = moveSprite;
        yield return new WaitForSeconds(0.1f);
        sr.sprite = stillSprite;
    }
}
