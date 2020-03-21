using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 2.0f;
    public float turningSpeed = 60;



    float current_groundType;
    private float lookLimit;
    private float distToGround;
    private Collider col;

    //private Camera m_camera;
    
    private LayerMask myLayerMask;
    public float camer_speed = 10.0f;
    float maxSpeed = 12.5f;
    float horizontal;
    float vertical;
    Vector2 rotation = Vector2.zero;
    

    public float jumpForce = 200.0f;

    private Rigidbody rb;
    public Transform playerCameraParent;

    public Transform playerTransform;
    
    private Quaternion newPlayerRotation;
    private Quaternion prevPlayerRotation;
    public float playerRotationSpeed = 2.0f;

    void Start()
    {
        lookLimit = 60.0f;
        Cursor.lockState = CursorLockMode.Locked;
        rb = this.GetComponentInChildren<Rigidbody>();
        
        // 0.001f offset to remove "Look Rotation Viewing Vector Is Zero" debug message, which drops framerate considerably
        rb.AddForce(0.001f, -0.001f, 0.001f, ForceMode.Impulse);
        prevPlayerRotation = transform.rotation;
        distToGround = 1;
        Cursor.visible = false;
    }



    // Update is called once per frame
    void Update()
    {
        
        // Player movement
        horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        rb.AddRelativeForce(horizontal, 0, 0, ForceMode.VelocityChange);
        vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        rb.AddRelativeForce(0, 0, vertical, ForceMode.VelocityChange);

        // Jump mechanics
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }

        //TO DO

        //Tilt player to direction of player's velocity

        float m_angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;

        playerTransform.rotation = Quaternion.Euler(rb.velocity.z, 0.0f, -rb.velocity.x);


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Manage camera view, user input

        // Camera rotation through quaternion
        rotation.y += Input.GetAxis("Mouse X") * camer_speed;
        rotation.x += -Input.GetAxis("Mouse Y") * camer_speed;
        rotation.x = Mathf.Clamp(rotation.x, -lookLimit, lookLimit);
        playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);
    }


    // Check is grounded for jump mechanic
    public bool IsGrounded()
    {        
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

}

