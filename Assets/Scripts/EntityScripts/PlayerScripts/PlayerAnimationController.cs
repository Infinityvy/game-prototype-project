using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController instance { get; private set; }

    public Animator animator;
    //0 is standstill, 1 is forward, 2 is back, 3 is left, 4 is right

    // start on back since player is facing down by default
    private int facing = 2;

    void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(GameInputs.keys["Forward"]))
        {
            facing = 1;
            animator.SetInteger("Direction", facing);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Back"]))
        {
            facing = 2;
            animator.SetInteger("Direction", facing);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Left"]))
        {
            facing = 3;
            animator.SetInteger("Direction", facing);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Right"]))
        {
            facing = 4;
            animator.SetInteger("Direction", facing);
            animator.SetBool("KeyPressed", true);
        }
        if(!Input.anyKey)
        {
            animator.SetBool("KeyPressed", false);
        }
    }

    public Vector3 getFacingDirection()
    {
        switch (facing)
        {
            case 1:
                return new Vector3(-1, 0, 1).normalized;
            case 2:
                return new Vector3(1, 0, -1).normalized;
            case 3:
                return new Vector3(-1, 0, -1).normalized;
            case 4:
                return new Vector3(1, 0, 1).normalized;
            default:
                throw new System.Exception("Unkown facing direction");
        }
    }
}
