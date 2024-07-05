using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player_mouvement : NetworkBehaviour
{
    private CharacterController controller;
    public float PlayerSpeed = 2f;

    private Vector3 velocity;
    private bool IsJumpPress;
    private float Gravity = -9.81f;
    private float JumpForce = 5f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            IsJumpPress = true;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        if(controller.isGrounded)
        {
            velocity = new Vector3(0,-1,0);
        }
        Vector3 move = new Vector3 (Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"))*Runner.DeltaTime*PlayerSpeed;

        velocity.y += Gravity * Runner.DeltaTime;

        if(IsJumpPress && controller.isGrounded)
        {
            velocity.y += JumpForce;
        }
        controller.Move(move + velocity*Runner.DeltaTime);
        if(move!=Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        IsJumpPress = false;
    }
}
