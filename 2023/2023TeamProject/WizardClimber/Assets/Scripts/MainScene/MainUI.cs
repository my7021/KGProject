using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button m_btnContinue;
    public Button m_btnStart;
    public Button m_btnOption;
    public Button m_btnExit;

    public OptionUI1 m_optionUI;

    void Start()
    {
        m_btnContinue.onClick.AddListener(OnClicked_Continue);
        m_btnStart.onClick.AddListener(OnClicked_Start);
        m_btnOption.onClick.AddListener(OnClicked_Option);
        m_btnExit.onClick.AddListener(OnClicked_Exit);

        m_btnContinue.gameObject.SetActive(File.Exists("SaveInfo.data"));
    }

    void OnClicked_Continue()
    {
        GameManager.Inst.isContinue = true;
        SceneManager.LoadScene(1);
    }
    void OnClicked_Start()
    {
        GameManager.Inst.isContinue = false;
        SceneManager.LoadScene(1);
    }
    void OnClicked_Option()
    {
        m_optionUI.gameObject.SetActive(true);
    }
    void OnClicked_Exit()
    {
        Application.Quit();
    }
}
