using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    //The min and max x and y values that the player is able to move
    private float playerMinX = -8.7f;
    private float playerMaxX = 8.7f;
    private float playerMinY = -4.8f;
    private float playerMaxY = 4.8f;

    //Variable to store the position of the mouse converted to world space
    private Vector3 mousePosition;

    private Rigidbody2D rb;
    private LineRenderer lr;
    //The direction in which the player is moving
    private Vector2 moveVelocity;

    public int maxHealth;
    public int currentHealth;

    private float invincibilityCounter;
    private float invincibilityLength;
    public float knockbackLength;
    public float knockbackForce;
    private float knockbackCounter;
    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Gets input for player movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        //Draws line from player to cursor
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, transform.position.z));

        //Shows or hides line depending on mouse click
        if (Input.GetKey(KeyCode.Mouse0))
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }

        if(currentHealth <= 0f)
        {
            //make sure to put the 'death sequence' name in here to run the graphics and everything else
        }
    }

    void FixedUpdate()
    {
        //Moves the player
        rb.MovePosition(new Vector2(Mathf.Clamp(rb.position.x + moveVelocity.x, playerMinX, playerMaxX), Mathf.Clamp(rb.position.y + moveVelocity.y, playerMinY, playerMaxY)));

        //Makes the player point towards the mouse cursor
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //finds the position of the player relative to the mouse
        Vector2 mouseDirection = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        //rotates the player to face mouse
        transform.up = mouseDirection;

        if (invincibilityCounter > 0)
        {
            invincibilityCounter--;
        }

        if (knockbackCounter <= 0)
        {
            //all movement of player stays the same
        }
    }

    //Player collision
    //makes it so if the enemy hits the player then the player loses health and jumps back from where it's been hit
    //also makes the player 'invincible' for a little bit after being hit so you don't just lose all your lives in one smash of enemies
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //knockback;

            if(invincibilityCounter == 0)
            {
                currentHealth -= 1;
                invincibilityCounter = 60;
            }
            
        }
        else
        {
            
        }

        //makes it so if the enemy's bullet hits the player they lose health
        if (other.gameObject.tag == "EnemyBullet")
        {
            currentHealth -= 1;
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;
        invincible = true;
    }


    //Player firing - to do
}
