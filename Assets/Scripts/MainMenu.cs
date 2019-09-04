using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SampleScene;

    public string HowToPlay;

    public string HighScores;

    public string Settings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBtn()
    {
        SceneManager.LoadScene(SampleScene);
    }

    public void HelpBtn()
    {
        SceneManager.LoadScene(HowToPlay);
    }

    public void ScoreBtn()
    {
        SceneManager.LoadScene(HighScores);
    }

    public void SettingBtn()
    {
        SceneManager.LoadScene(Settings);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
