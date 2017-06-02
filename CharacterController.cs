using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVelocity = 12;
        public float rotationalVelocity = 100;
        public float jumpVelocity = 25;
        public float distanceToGrounded = 0.51f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downwardAcceleration = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    //ensuring the classes can be seen and modified by the inspector
    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rigidBody;
    float forwardInput, turnInput, jumpInput;


    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distanceToGrounded, moveSetting.ground);
    }
 
	void Start ()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("The character needs a rigidbody.");

        forwardInput = turnInput = jumpInput = 0;
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);//interpolated
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);//interpolatd
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);//non-interpolated
    }

	void Update ()
    {
        GetInput();
        Turn();
	}

    void FixedUpdate()
    {
        Run();
        Jump();

        rigidBody.velocity = transform.TransformDirection(velocity);
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            //move
            velocity.z = moveSetting.forwardVelocity * forwardInput;
        }
        else
            //zero velocity
            velocity.z = 0;
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotationalVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            //jump
            velocity.y = moveSetting.jumpVelocity;
        }
        else if (jumpInput == 0 && Grounded()){
            //zero out velocity
            velocity.y = 0;
        }
        else
        {
            //decrease velocity.y
            velocity.y -= physSetting.downwardAcceleration;
        }
    }
}
