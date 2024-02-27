using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFSM
{
    public delegate void DelegateFunc();

    public class State
    {
        public DelegateFunc m_OnEnterFunc = null;
        public virtual void Initialize(DelegateFunc func)
        {
            m_OnEnterFunc = new DelegateFunc(func);
        }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    public class ReadyState : State
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    public class WaveState : State
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    public class GameState : State
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    public class ResultState : State
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }


    private State m_curState = null;
    private State m_newState = null;

    private State m_kReady = new ReadyState();
    private State m_kWave = new WaveState();
    private State m_kGame = new GameState();
    private State m_kResult = new ResultState();

    public void Initialize(DelegateFunc kReady, DelegateFunc kWave, DelegateFunc kGame, DelegateFunc kResult)
    {
        m_kReady.Initialize(kReady);
        m_kWave.Initialize(kWave);
        m_kGame.Initialize(kGame);
        m_kResult.Initialize(kResult);
    }

    public void SetState(State kState)
    {
        m_newState = kState;
    }

    public void OnUpdate()
    {
        if (m_newState != null)
        {
            if (m_curState != null)
                m_curState.OnExit();

            m_curState = m_newState;
            m_newState = null;
            m_curState.OnEnter();
        }

        if (m_curState != null)
            m_curState.OnUpdate();
    }
    public void SetReadyState() { SetState(m_kReady); }
    public void SetWaveState() { SetState(m_kWave); }
    public void SetGameState() { SetState(m_kGame); }
    public void SetResultState() { SetState(m_kResult); }

    public bool IsCurState(State kState)
    {
        if (m_curState == null)
            return false;

        return (m_curState == kState) ? true : false;
    }
    public State GetCurState()
    {
        return m_curState;
    }
    public void SetNoneState()
    {
        m_newState = null;
        m_curState = null;
    }

    public bool IsresultState()
    {
        return (m_curState == m_kResult);
    }

    public bool IsNoneState()
    {
        return (m_curState == null);
    }

    public bool IsGameState()
    {
        return (m_curState == m_kGame);
    }
}

