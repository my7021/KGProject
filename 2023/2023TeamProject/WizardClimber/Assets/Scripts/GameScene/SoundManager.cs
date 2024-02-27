using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource m_bgm;
    public AudioSource m_tel;

    void Start()
    {

    }

    void Update()
    {
        m_bgm.volume = PlayerPrefs.GetFloat("BgmValue", 1);
        m_tel.volume = PlayerPrefs.GetFloat("SfxValue", 1);
    }
}
