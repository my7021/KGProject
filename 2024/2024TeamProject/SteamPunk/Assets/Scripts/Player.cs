using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform m_cameraTransform;
    public CharacterController m_characterController;

    public float m_moveSpeed;
    public float m_jumpSpeed;
    public float m_gravity;
    public float m_yVelocity;

    void Start()
    {
        
    }

    void Update()
    {
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(axisH, 0, axisV);
        moveDirection = m_cameraTransform.TransformDirection(moveDirection) * m_moveSpeed;
        if (m_characterController.isGrounded)
        {
            m_yVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_yVelocity = m_jumpSpeed;
            }
        }
        m_yVelocity += m_gravity * Time.deltaTime;
        moveDirection.y = m_yVelocity;
        m_characterController.Move(moveDirection * Time.deltaTime);
    }
}
