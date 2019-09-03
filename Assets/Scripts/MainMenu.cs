using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SampleScene;

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
        //SceneManager.LoadScene();
    }

    public void ScoreBtn()
    {
        //SceneManager.LoadScene();
    }

    public void SettingBtn()
    {
        //SceneManager.LoadScene();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
