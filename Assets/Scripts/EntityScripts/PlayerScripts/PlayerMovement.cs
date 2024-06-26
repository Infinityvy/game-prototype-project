using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public:
    [HideInInspector]
    public bool isGrounded = false;


    // private:
    [SerializeField]
    private new Rigidbody rigidbody;

    [SerializeField]
    private LayerMask layerMaskGround;
    [SerializeField]
    private LayerMask layerMaskStep;

    [SerializeField]
    private ParticleSystem dashParticleEmitter;

    private float acceleration = 1f;
    private float maxSpeed = 3.5f;
    private float jumpStrength = 5f;

    private readonly float gravity = 9.81f;
    private Vector3 direction = Vector3.zero;

    private float timeWhenLastGrounded = 0f;
    private float timeWhenLastJumped = 0f;
    private readonly float timeBeforeNextJump = 0.1f;

    private float dashForce = 8.5f;
    private float timeWhenLastDashed = 0f;
    private float timeBeforeNextDash = 1f;
    private float dashDuration = 0.2f;

    private Transform waterSplashPrefab;


    void Start()
    {
        waterSplashPrefab = Resources.Load<Transform>("WaterSplash");
        
        dashParticleEmitter.Stop();

        var mainModule = dashParticleEmitter.main;
        mainModule.duration = dashDuration;
    }

    void Update()
    {
        if(PlayerEntity.instance.isDead) return;

        if(checkIfGrounded())
        {
            if (!isGrounded)
            {
                timeWhenLastGrounded = Time.time;

                if (transform.position.y < 0)
                {
                    AkSoundEngine.PostEvent("player_landing_in_water", gameObject);
                    Instantiate(waterSplashPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                }
                else AkSoundEngine.PostEvent("player_landing_on_raft", gameObject);

                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

        move();
        jump();
        dash();

        rigidbody.velocity += Vector3.down * gravity * Time.deltaTime;
    }

    private void move()
    {
        direction = Vector3.zero;

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
        }
        else if (isGrounded && (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0))
        {
            Vector3 reductionVector = 100.0f * -Time.deltaTime * new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

            rigidbody.AddForce(reductionVector);
        }

        if(rigidbody.velocity.magnitude > 0) stepUp(new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z));
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
            if (transform.position.y < 0) AkSoundEngine.PostEvent("player_jumping_from_water", gameObject);
            else AkSoundEngine.PostEvent("player_jumping_from_raft", gameObject);
        }
    }

    private bool checkIfGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.05f, layerMaskGround))
            return true;

        return false;
    }

    private void dash()
    {
        if (!Input.GetKeyDown(GameInputs.keys["Dash"])) return;
        if (Time.time - timeWhenLastDashed < timeBeforeNextDash) return;

        timeWhenLastDashed = Time.time;

        Vector3 dashDirection = (direction.magnitude == 0 ? PlayerAnimationController.instance.getFacingDirection() : direction);

        float tmpDashForce = dashForce;
        if (!isGrounded) tmpDashForce *= 0.5f;

        rigidbody.AddForce(dashDirection * tmpDashForce, ForceMode.Impulse);

        AkSoundEngine.PostEvent("player_dash", gameObject);

        dashParticleEmitter.Play();

        Invoke(nameof(decelerateDash), dashDuration);
    }

    private void decelerateDash()
    {
        Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        if (horizontalVelocity.magnitude < maxSpeed) return;

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
        rigidbody.velocity = new Vector3(horizontalVelocity.x, rigidbody.velocity.y, horizontalVelocity.z);
    }

    private void stepUp(Vector3 rayVector)
    {
        RaycastHit[] hitsLeft = Physics.RaycastAll(transform.position + Vector3.up * 0.1f + transform.rotation * Vector3.left * 0.5f, rayVector.normalized, rayVector.magnitude * Time.deltaTime + 0.5f, layerMaskStep);
        RaycastHit[] hitsRight = Physics.RaycastAll(transform.position + Vector3.up * 0.1f + transform.rotation * Vector3.right * 0.5f, rayVector.normalized, rayVector.magnitude * Time.deltaTime + 0.5f, layerMaskStep);

        foreach (RaycastHit hit in hitsLeft)
        {
            Vector3 tilePos = hit.transform.position;
            Vector3 playerToTile = tilePos - transform.position;
            transform.position = playerToTile.normalized * 0.6f + new Vector3(transform.position.x, 0.3f, transform.position.z);
            return;
        }

        foreach (RaycastHit hit in hitsRight)
        {
            Vector3 tilePos = hit.transform.position;
            Vector3 playerToTile = tilePos - transform.position;
            transform.position = playerToTile.normalized * 0.6f + new Vector3(transform.position.x, 0.3f, transform.position.z);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rigidbody.velocity);
    }
}
