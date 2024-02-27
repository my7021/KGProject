using UnityEngine;

public class Rune : MonoBehaviour
{
    public GameObject m_arrow;
    public Wizard m_Wizard;

    Vector2 point1, point2;
    float angle;
    float dis;
    Vector3 scale;
    Vector3 oriScale;
    Vector2 dir;

    public Rigidbody2D rb;
    public TrailRenderer tr;
    public CameraPosition m_CP;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void LookAtMouse()
    {
        angle = (Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg) - 180;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void Move()
    {
        rb.gravityScale = 1;
        rb.AddForce(transform.up * dis * 1.5f, ForceMode2D.Impulse);
        rb.angularVelocity -= 10 * dis;
    }
    void Update()
    {
        if(rb.velocity == Vector2.zero && !(Time.timeScale == 0) && GameManager.Inst.m_GameScene.m_BattleFSM.IsGameState())
            RuneShoot();
        cheat();
    }

    void RuneShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.gravityScale = 0;
            m_arrow.SetActive(true);
            point1 = transform.position;
        }
        if (Input.GetMouseButton(0) && point1 != Vector2.zero)
        {
            point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LookAtMouse();
            dis = Mathf.Clamp(Vector2.Distance(point1, point2), 1f, 12f);
            scale = new Vector3(3, dis / 2, 1);
            m_arrow.transform.localScale = scale;
        }
        if (Input.GetMouseButtonUp(0) && point1 != Vector2.zero)
        {
            rb.gravityScale = 1;
            point1 = Vector2.zero;
            m_arrow.SetActive(false); 
            m_Wizard.m_SpriteRenderer.sprite = m_Wizard.m_WizardAttack;
            Move();
        }
    }
    public void StopRune()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }

    void cheat()
    {
        if(Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F) && Input.GetMouseButtonDown(1)) 
        {
            StopRune();
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            rb.gravityScale = 1;
            m_CP.m_player = gameObject.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("End"))
        {
            GameManager.Inst.m_GameScene.m_BattleFSM.SetResultState();
        }
    }
}
