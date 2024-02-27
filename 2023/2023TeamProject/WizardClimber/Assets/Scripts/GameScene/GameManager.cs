using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _Inst;
    public static GameManager Inst
    {
        get
        {
            if (_Inst == null)
                _Inst = new GameManager();
            return _Inst;
        }
    }
    public GameScene m_GameScene { get; set; }
    public SaveInfo m_SaveInfo = new SaveInfo();

    public bool isContinue = false;

}
