using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string bulletType;
    public float speed;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Moves bullet forward at its speed
        transform.position += transform.up * speed;
    }

    //A function that can be called after instantiating a bullet to easily set its type, speed, rotation and colour
    public void instantiateBullet (string type, float bSpeed, Quaternion rotation, Color color)
    {
        bulletType = type;
        speed = bSpeed;
        transform.rotation = rotation;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    //Deletes the bullet if it goes outside the playspace trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayspaceTrigger") Destroy(gameObject);
    }
}
