using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static Vector2 Movement;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private bool loseChase;
    private bool canMove = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

    }

    private void Update()
    {
        if(canMove)
            Movement = moveAction.ReadValue<Vector2>();
        
        if (StaticManager.hasTimeRunOut() && !loseChase)
        {
            loseChase = true;
            canMove = false;
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }

    public void setCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}