using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI m_GameUI;
    public HudUI m_HudUI;

    BattleFSM m_BattleFSM = new BattleFSM();

    void Awake()
    {
        GameManager.Inst.m_GameScene = this;
    }

    void Start()
    {
        m_BattleFSM.Initialize(Callback_Ready,
                               Callback_Wave,
                               Callback_Game,
                               Callback_Result);
    }

    void Callback_Ready()
    {

    }
    void Callback_Wave()
    {

    }
    void Callback_Game()
    {

    }
    void Callback_Result()
    {

    }

    void Update()
    {
        m_BattleFSM.OnUpdate();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
