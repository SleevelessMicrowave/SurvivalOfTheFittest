using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherMove : MonoBehaviour
{
    //public static float rabbitSpeed = 1;
    //Changing direction
    private float directionChangeInterval = 1;
    //Angle change
    private float maxHeadingChange = 90;

    //Us to control character
    CharacterController controller;
    float heading;
    Vector3 targetRotation;

    private float radius = 35;

    private Vector3 playerVelocity;

    private bool jumped;

    public static int poacherSpeed = 10;

    // Start is called before the first frame update. Each rabbit has the script so it runs for each one
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        //Set random initial rotation. Which way rabbit is facing
        heading = Random.Range(0, 360);
        //Changes the angle 
        transform.eulerAngles = new Vector3(0, heading, 0);
        //Delays 1 sec
        StartCoroutine(NewHeading());
    }

    // Update is called once per frame
    void Update()
    {
        //Position from the center
        float distance = Vector3.Distance(transform.position, new Vector3(0, 0, 0));

        //Eurlerangles is the angle. Make the movements gradual 
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        //vecto forward value is 0, 0, 1
        var forward = transform.TransformDirection(Vector3.forward);
        //Vector value 0, 1, 0
        Vector3 up = transform.TransformDirection(Vector3.up);

        //If contacted with the floor
        if (controller.isGrounded && !jumped)
        {
            playerVelocity.y += Mathf.Sqrt(.8f * -3f * -9.81f);
            StartCoroutine(DelayJump());
            jumped = true;
        }
        else if (!controller.isGrounded)
        {
            //playerVelocity.y += -9.81f * Time.deltaTime;
            controller.Move(Vector3.down * 3 * Time.deltaTime);
        }

        //Does not update very frame
        playerVelocity.y += -9.81f * Time.deltaTime;
        //moves in x and y direction
        controller.Move((forward + playerVelocity) * poacherSpeed / 10 * Time.deltaTime);
        //Debug.Log("here1");

        //If the rabbbit is farther than 35 from the cetner
        if (distance > radius)
        {
            //Vector from object to center
            Vector3 fromOriginToObject = transform.position - Vector3.zero;
            //Multiply by radius/distance
            fromOriginToObject *= radius / distance;

            transform.position = Vector3.zero + fromOriginToObject;

        }

    }
    //Wait one second and then runs new heading routine
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            //Delays 1 sec
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }
    //
    void NewHeadingRoutine()
    {
        //Caps values?
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        //Chooses random value to change direction
        heading = Random.Range(floor, ceil);
        //New rotation for rabbit
        targetRotation = new Vector3(0, heading, 0);
    }

    IEnumerator DelayJump()
    {

        yield return new WaitForSeconds(.3f);
        jumped = false;
        //Debug.Log("here");

    }
}