using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionUI : MonoBehaviour
{
    public Slider m_sldBgm;
    public Slider m_sldSfx;
    public Button m_btnResume;
    public Button m_btnExit;

    void Start()
    {
        m_sldBgm.onValueChanged.AddListener(OnValueChanged_Bgm);
        m_sldSfx.onValueChanged.AddListener(OnValueChanged_Sfx);
        m_btnResume.onClick.AddListener(OnClicked_Resume);
        m_btnExit.onClick.AddListener(OnClicked_Exit);
        Initialize();
    }

    void OnValueChanged_Bgm(float value)
    {
        PlayerPrefs.SetFloat("BgmValue", value);
    }
    void OnValueChanged_Sfx(float value)
    {
        PlayerPrefs.SetFloat("SfxValue", value);
    }
    void OnClicked_Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    void OnClicked_Exit()
    {
        if(GameManager.Inst.m_GameScene.m_BattleFSM.IsGameState())
            GameManager.Inst.m_SaveInfo.Save();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClicked_Resume();
    }

    void Initialize()
    {
        m_sldBgm.value = PlayerPrefs.GetFloat("BgmValue", 1);
        m_sldSfx.value = PlayerPrefs.GetFloat("SfxValue", 1);
    }
}
