using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int health;
    public float speed;
    public int currentHealth = 1;
    public int maxHealth;

    private GameObject player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        //finds the position of this enemy relative to the player
        Vector2 playerDirection = new Vector2(
            player.transform.position.x - transform.position.x,
            player.transform.position.y - transform.position.y
        );

        //rotates the enemy towards the player, using the Vector2 made in the previous step
        transform.up = playerDirection;
        //moves the enemy in the direction it is facing
        transform.position += transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentHealth -= 1;
    }
      
}
