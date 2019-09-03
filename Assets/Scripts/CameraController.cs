using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CameraZoom()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Camera.main.orthographicSize -= 0.05f;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, transform.position.z), 0.3f);
        }
    }
}
