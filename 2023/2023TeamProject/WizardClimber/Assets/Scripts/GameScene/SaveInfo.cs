using System.IO;
using UnityEngine;

public class SaveInfo
{
    float PlayerPosX;
    float PlayerPosY;
    float RunePosX;
    float RunePosY;
    float RuneVelX;
    float RuneVelY;
    float RuneAngVel;
    int PlayTime;

    public void Save()
    {
        PlayerPosX = GameManager.Inst.m_GameScene.m_GameUI.m_player.transform.position.x;
        PlayerPosY = GameManager.Inst.m_GameScene.m_GameUI.m_player.transform.position.y;
        RunePosX = GameManager.Inst.m_GameScene.m_GameUI.m_rune.transform.position.x;
        RunePosY = GameManager.Inst.m_GameScene.m_GameUI.m_rune.transform.position.y;
        RuneVelX = GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.velocity.x;
        RuneVelY = GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.velocity.y;
        RuneAngVel = GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.angularVelocity;
        PlayTime = GameManager.Inst.m_GameScene.m_playTime;
        FileStream fs = new FileStream("SaveInfo.data", FileMode.Create, FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(PlayerPosX);
        bw.Write(PlayerPosY);
        bw.Write(RunePosX);
        bw.Write(RunePosY);
        bw.Write(RuneVelX);
        bw.Write(RuneVelY);
        bw.Write(RuneAngVel);
        bw.Write(PlayTime);
        bw.Close();
        fs.Close();
    }
    public void Load()
    {
        FileStream fs = new FileStream("SaveInfo.data", FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        PlayerPosX = br.ReadSingle();
        PlayerPosY = br.ReadSingle();
        RunePosX = br.ReadSingle();
        RunePosY = br.ReadSingle();
        RuneVelX = br.ReadSingle();
        RuneVelY = br.ReadSingle();
        RuneAngVel = br.ReadSingle();
        PlayTime = br.ReadInt32();
        br.Close();
        fs.Close();

        GameManager.Inst.m_GameScene.m_GameUI.m_player.transform.position = new Vector3(PlayerPosX, PlayerPosY, 0);
        GameManager.Inst.m_GameScene.m_GameUI.m_rune.transform.position = new Vector3(RunePosX, RunePosY, 0);
        GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.velocity = new Vector2(RuneVelX, RuneVelY);
        GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.angularVelocity = RuneAngVel;
        GameManager.Inst.m_GameScene.m_GameUI.m_rune.rb.gravityScale = 1;
        GameManager.Inst.m_GameScene.m_GameUI.m_rune.tr.enabled = true;
        GameManager.Inst.m_GameScene.m_playTime = PlayTime;
    }
}
