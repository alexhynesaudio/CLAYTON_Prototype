using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : MonoBehaviour
{

    //Vector3 Offset;
    Rigidbody parentRb;
    private Animator m_animation;
    PlayerController controller;
    private bool m_isgrounded_current;
    private bool m_isgrounded_previous;
    private Quaternion m_desiredRotation;
    private Quaternion m_actualRotation;

    Vector3 m_desiredRotation_euler;
    Vector3 m_currentRotation_euler;

    private float m_turnSpeed;


    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponentInParent<PlayerController>();
        //Offset = new Vector3(10.0f, 0.0f, 0.0f);

        // Instantiating reference for rb in parent
        parentRb = this.GetComponentInParent<Rigidbody>();

        // 0.001f offset to remove "Look Rotation Viewing Vector Is Zero" debug message, which drops framerate considerably
        transform.localRotation = Quaternion.LookRotation(new Vector3((parentRb.velocity.x + 0.001f), 0.0f, parentRb.velocity.z), Vector3.up);

        m_animation = this.GetComponent<Animator>();
        //m_isgrounded_previous = m_isgrounded_current = controller.IsGrounded();

        m_turnSpeed = 55.0f;
    }

    // Update is called once per frame
    void Update()
    {


        if (parentRb.velocity.magnitude > .5f)
        {
            // TODO Smooth rotation for larger angled rotation shifts > 45;
            m_actualRotation = transform.localRotation;
            m_desiredRotation = Quaternion.LookRotation(new Vector3(parentRb.velocity.x, 0.0f, parentRb.velocity.z), Vector3.up);
            m_desiredRotation_euler = m_desiredRotation.eulerAngles;
            m_currentRotation_euler = m_actualRotation.eulerAngles;

            transform.localRotation =
            Quaternion.Lerp(
                Quaternion.Euler(new Vector3(0.0f, m_desiredRotation_euler.y, 0.0f)),
                Quaternion.Euler(new Vector3(0.0f, m_currentRotation_euler.y, 0.0f)),
                Time.deltaTime * m_turnSpeed);



        }

        // Debug
        Debug.DrawRay(transform.position, (transform.forward), Color.red, 1.0f);

        // Is grounded frame compare, add bounce

        
        // Animation for jumping/landing
        m_isgrounded_current = controller.IsGrounded();
        if (m_isgrounded_previous != m_isgrounded_current)
        {
            m_animation.SetBool("Is_Bouncing", true);
        }
        else
        {
            m_animation.SetBool("Is_Bouncing", false);
        }
        m_isgrounded_previous = controller.IsGrounded();

    }
}
