using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    //Settings variable declaration
    public float inputDelay = 0.1f;
    public float forwardVelocity = 12;
    public float rotationalVelocity = 100;

    Quaternion targetRotation;
    Rigidbody rigidBody;
    float forwardInput, turnInput;


    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }
 
	void Start ()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("The character needs a rigidbody.");

        forwardInput = turnInput = 0;
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

	void Update ()
    {
        GetInput();
        Turn();
	}

    void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            //move
            rigidBody.velocity = transform.forward * forwardInput * forwardVelocity;
        }
        else
            //zero velocity
            rigidBody.velocity = Vector3.zero;
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(rotationalVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }
}
