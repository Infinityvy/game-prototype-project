using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(GameInputs.keys["Forward"]))
        {
            animator.Play("WalkBack");
        }
        if (Input.GetKey(GameInputs.keys["Back"]))
        {
            animator.Play("WalkFront");
        }
        if (Input.GetKey(GameInputs.keys["Left"]))
        {
            animator.SetBool("Mirror", false);
            animator.Play("WalkSideLeft");
        }
        if (Input.GetKey(GameInputs.keys["Right"]))
        {
            animator.SetBool("Mirror", true);
            animator.Play("WalkSideRight");
        }
        if(!Input.GetKey(GameInputs.keys["Forward"])&&!Input.GetKey(GameInputs.keys["Back"])&&!Input.GetKey(GameInputs.keys["Left"])&&!Input.GetKey(GameInputs.keys["Right"]))
        {
            animator.Play("StandStill");
        }

    }
}
