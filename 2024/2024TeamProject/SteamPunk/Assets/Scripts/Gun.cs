using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform m_cameraTransform;
    public GameObject m_prbBullet;
    public Transform m_bulletPos;
    public Transform m_BulletParent;

    float m_time = 0;
    bool isShot = true;

    void Start()
    {
        
    }

    void Shot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && Input.GetMouseButton(0) && isShot)
            {
                if(hit.collider.CompareTag("Target"))
                    GameManager.Inst.m_GameScene.m_HudUI.m_TopUI.m_txtResult.text += "hit";

                GameObject go = Instantiate(m_prbBullet, m_BulletParent);
                go.transform.position = m_bulletPos.position;
                go.GetComponent<Rigidbody>().AddForce(ray.direction * 100, ForceMode.Impulse);
                isShot = false;
            }
        }
    }

    void Update()
    {
        Shot();
        m_time += Time.deltaTime;
        if (m_time >= 0.2f)
        {
            m_time = 0;
            isShot = true;
        }
    }
}
