using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI1 : MonoBehaviour
{
    public Slider m_sldBgm;
    public Slider m_sldSfx;
    public Button m_btnCancel;

    void Start()
    {
        m_sldBgm.onValueChanged.AddListener(OnValueChanged_Bgm);
        m_sldSfx.onValueChanged.AddListener(OnValueChanged_Sfx);
        m_btnCancel.onClick.AddListener(OnClicked_Cancel);
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
    void OnClicked_Cancel()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }

    void Initialize()
    {
        m_sldBgm.value = PlayerPrefs.GetFloat("BgmValue", 1);
        m_sldSfx.value = PlayerPrefs.GetFloat("SfxValue", 1);
    }
}
