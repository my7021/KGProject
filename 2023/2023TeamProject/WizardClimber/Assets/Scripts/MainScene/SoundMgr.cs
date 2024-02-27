using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public AudioSource m_bgm1;

    void Start()
    {

    }

    void Update()
    {
        m_bgm1.volume = PlayerPrefs.GetFloat("BgmValue", 1);
    }
}
