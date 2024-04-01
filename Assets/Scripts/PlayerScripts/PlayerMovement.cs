using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public:
    public new Rigidbody rigidbody;
    public float acceleration = 1f;
    public float maxSpeed = 2.5f;
    public float jumpStrength = 1f;

    public bool isGrounded = false;

    public LayerMask layerMaskGround;

    // private:
    private readonly float gravity = 9.81f;
    private Vector3 direction = Vector3.zero;

    private float timeWhenLastGrounded = 0f;
    private float timeWhenLastJumped = 0f;
    private readonly float timeBeforeNextJump = 0.1f;

    void Start()
    {

    }

    void Update()
    {
        if(checkIfGrounded())
        {
            if (!isGrounded)
                timeWhenLastGrounded = Time.time;

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        move();
        jump();        

        rigidbody.velocity += Vector3.down * gravity * Time.deltaTime;
    }

    private void move()
    {
        if (Input.GetKey(GameInputs.keys["Forward"]))
        {
            direction += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(GameInputs.keys["Back"]))
        {
            direction += new Vector3(1, 0, -1);
        }
        if (Input.GetKey(GameInputs.keys["Left"]))
        {
            direction += new Vector3(-1, 0, -1);
        }
        if (Input.GetKey(GameInputs.keys["Right"]))
        {
            direction += new Vector3(1, 0, 1);
        }

        Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        if (direction != Vector3.zero
            && horizontalVelocity.magnitude < maxSpeed)
        {
            direction = direction.normalized;
            rigidbody.AddForce(800 * acceleration * Time.deltaTime * direction);

            direction = Vector3.zero;
        }
        else if (isGrounded && (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0))
        {
            Vector3 reductionVector = 100.0f * -Time.deltaTime * new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

            rigidbody.AddForce(reductionVector);
        }
    }

    private void jump()
    {
        if (Input.GetKey(GameInputs.keys["Jump"])
            && isGrounded
            && (Time.time - timeWhenLastJumped) > timeBeforeNextJump
            && (Time.time - timeWhenLastGrounded) > 0.01f)
        {
            timeWhenLastJumped = Time.time;
            rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private bool checkIfGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.05f, layerMaskGround))
            return true;

        return false;
    }

    private bool equalSignOrZero(float a, float b)
    {
        if (a <= 0 && b <= 0) return true;
        if (a >= 0 && b >= 0) return true;
        return false;
    }
}
