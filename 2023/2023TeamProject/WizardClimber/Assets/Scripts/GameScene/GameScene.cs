using System.Collections;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public BattleFSM m_BattleFSM = new BattleFSM();
    public GameUI m_GameUI = null;
    public HudUI m_HudUI = null;
    public CameraPosition m_CamPos;

    float m_time = 0;
    public int m_playTime = 0;
    void Awake()
    {
        GameManager.Inst.m_GameScene = this;
    }

    void Start()
    {
        m_BattleFSM.Initialize(OnEnter_ReadyState,
                               OnEnter_WaveState,
                               OnEnter_GameState,
                               OnEnter_ResultState);
        m_BattleFSM.SetReadyState();
    }

    void OnEnter_ReadyState()
    {
        if (!GameManager.Inst.isContinue)
        {
            m_time = 0;
            m_playTime = 0;
            m_GameUI.m_player.transform.position = new Vector2(-30, -1.25f);
            m_GameUI.m_rune.transform.position = new Vector2(-30, -1.55f);
            m_GameUI.m_startTree.enabled = false;
            m_GameUI.m_rune.rb.gravityScale = 1;
            m_GameUI.m_rune.rb.velocity = new Vector2(8f, 10f);
            m_GameUI.m_rune.rb.angularVelocity = 200;
            m_GameUI.m_rune.tr.enabled = true;
            StartCoroutine(WaitSec());
        }
        else
        {
            GameManager.Inst.m_SaveInfo.Load();
            m_BattleFSM.SetGameState();
        }
    }
    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(3.7f);
        m_BattleFSM.SetGameState();
    }
    void OnEnter_WaveState()
    {

    }
    void OnEnter_GameState()
    {
        m_CamPos.enabled = true;
        m_GameUI.m_startTree.enabled = true;
    }
    void OnEnter_ResultState()
    {
        StartCoroutine(EndingScene());
    }
    IEnumerator EndingScene()
    {
        m_GameUI.m_rune.StopRune();
        m_GameUI.m_player.transform.position = new Vector3(24.5f, 128.8f, 0);
        m_GameUI.m_rune.transform.position = new Vector3(25.81f, 129.7f, 0);
        m_GameUI.m_rune.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(3);
        m_HudUI.m_ResultUI.SetResultUI();
    }

    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= 1 && m_BattleFSM.IsGameState())
        {
            m_playTime += 1;
            m_time = 0;
            m_HudUI.m_TopUI.m_txtPlayTime.text = $"{m_playTime / 60:00} : {m_playTime % 60:00}";
        }

        if (m_BattleFSM != null)
            m_BattleFSM.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            m_HudUI.m_OptionUI.gameObject.SetActive(true);
        }
        
    }
    private void OnApplicationQuit()
    {
        if(m_BattleFSM.IsGameState())
            GameManager.Inst.m_SaveInfo.Save();
    }
}
