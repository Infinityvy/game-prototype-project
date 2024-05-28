using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    //0 is standstill, 1 is forward, 2 is back, 3 is left, 4 is right
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(GameInputs.keys["Forward"]))
        {
            animator.SetInteger("Direction", 1);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Back"]))
        {
            animator.SetInteger("Direction", 2);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Left"]))
        {
            animator.SetInteger("Direction", 3);
            animator.SetBool("KeyPressed", true);
        }
        if (Input.GetKey(GameInputs.keys["Right"]))
        {
            animator.SetInteger("Direction", 4);
            animator.SetBool("KeyPressed", true);
        }
        if(!Input.anyKey)
        {
            animator.SetBool("KeyPressed", false);
        }
    }
}
