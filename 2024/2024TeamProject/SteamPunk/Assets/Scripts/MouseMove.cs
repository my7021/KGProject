using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public float m_sensitivity = 500f;
    public float m_rotationX;
    public float m_rotationY;

    void Start()
    {
        
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        m_rotationY += mouseX * m_sensitivity * Time.deltaTime;
        m_rotationX += mouseY * m_sensitivity * Time.deltaTime;

        if (m_rotationX > 35f)
            m_rotationX = 35f;
        if (m_rotationX < -30f)
            m_rotationX = -30f;

        transform.eulerAngles = new Vector3(-m_rotationX, m_rotationY, 0);
    }
}
