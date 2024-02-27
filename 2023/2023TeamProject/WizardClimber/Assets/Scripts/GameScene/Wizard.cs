using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public Transform m_rune;
    Rigidbody2D m_RuneRb;
    public SpriteRenderer m_SpriteRenderer;
    public Animator m_Animator;

    public GameObject m_ParticleTell;
    public Transform m_ParticleParent;
    public AudioSource m_AudTel;

    public Sprite m_WizardIdle;
    public Sprite m_WizardAttack;
    void Awake()
    {
        m_RuneRb = m_rune.GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckRune();
    }

    void SetRune()
    {
        m_RuneRb.gravityScale = 0f;
        m_rune.rotation = Quaternion.identity;
        m_rune.position = new Vector3(transform.position.x, transform.position.y - 0.305f, 9);
    }

    void CheckRune()
    {
        if(m_RuneRb.gravityScale == 1 && m_RuneRb.velocity == Vector2.zero)
        {
            m_SpriteRenderer.sprite = m_WizardIdle;
            transform.position = new Vector3(m_rune.position.x, m_rune.position.y + 1.16f, 0);
            GameObject go = Instantiate(m_ParticleTell, m_ParticleParent);
            go.transform.position = transform.position;
            m_AudTel.Play();
            Destroy(go, 2);
            SetRune();
        }
    }
}
