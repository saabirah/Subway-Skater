using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05F;

    //
    private bool isRunning = false;

    //Animation
    private Animator anim;

    //Movement
    private CharacterController controller;
    private float jumpForce = 14.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1;// 0 = Left,1 = Midddle, 2 = right

    //Speed Modifier
    private float originalSpeed = 7.0f;
    private float speed ;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;


    private void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }


    private void Update()
    {
        if(!isRunning)
        {
            return;
        }

        if(Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;

            //change the modifier text
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }


        // Gather the input on which lane we should be
        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(true);
        }*/

        // Gather the input on which lane we should be
        if (MobileImput.Instance.SwipeLeft)
        {
            MoveLane(false);
            Debug.Log("gauche");
        }

        if (MobileImput.Instance.SwipeRight)
        {
            MoveLane(true);
        }



        // Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if(desiredLane ==0)
        {
           targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        // let's calculate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded",isGrounded);

        //Calculate y
        if (isGrounded) //if grounded
        {
            verticalVelocity = -0.1f;


            // if (Input.GetKeyDown(KeyCode.Space))
            if (MobileImput.Instance.SwipeUp)
            {
                //jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileImput.Instance.SwipeDown)
            {
                // slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }

        else
        {
                verticalVelocity -=(gravity*Time.deltaTime);

                //fast falling mechanic
                //if(Input.GetKeyDown(KeyCode.Space))
                if(MobileImput.Instance.SwipeDown)
                {
                    verticalVelocity =-jumpForce;                   
                }
        }
    

        moveVector.y = verticalVelocity;
        //  moveVector.y = -0.1f;
        moveVector.z = speed;


        // move the Pengu
        controller.Move(moveVector * Time.deltaTime);

        // Rotate the Pengu where he is going
        Vector3 dir = controller.velocity;
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }


    }


    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y/2, controller.center.z);
    }


    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);

    }


    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
        /*
        // left
        if(!goingRight)
        {
            desiredLane--;
            if(desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        else
        {
            desiredLane++;
                if(desiredLane == 3) 
            {
                desiredLane = 2;
            }
        }*/
    }


    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
                controller.bounds.center.z),
            Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan,1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
          
    }


    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.IsDead = true;
        GameManager.Instance.OnDeath();
     

    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;
        }
    }



}
