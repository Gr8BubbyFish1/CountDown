using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 movement;

    private void Update()
    {
        movement.Set(PlayerManager.Movement.x, PlayerManager.Movement.y);

        rb.linearVelocity = movement * playerSpeed;
    }
}
