using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GUIController : MonoBehaviour
{
   
    Text text;
    float thetime;
    public float speed = 1; 
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
     
        thetime += Time.deltaTime * speed;
        string minutes = Mathf.Floor((thetime % 3600) / 60).ToString("00");
        string seconds = (thetime % 60).ToString("00");
       text.text = minutes + ":" + seconds; 
    }

    public void StartGame()
    {
       
    }
}
