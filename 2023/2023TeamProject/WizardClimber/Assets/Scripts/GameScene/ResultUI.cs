using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public Text m_txtPlayTime;
    public Button m_btnRestart;
    public Button m_btnExit;

    void Start()
    {
        m_btnRestart.onClick.AddListener(OnClicked_Restart);
        m_btnExit.onClick.AddListener(OnClicked_Exit);
    }

    void OnClicked_Restart()
    {
        gameObject.SetActive(false);
        GameManager.Inst.m_GameScene.m_BattleFSM.SetReadyState();
    }
    void OnClicked_Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void SetResultUI()
    {
        gameObject.SetActive(true);
        m_txtPlayTime.text = $"{GameManager.Inst.m_GameScene.m_playTime / 60:00} : {GameManager.Inst.m_GameScene.m_playTime % 60:00}";
    }
}
