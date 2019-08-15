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

    //Health variables - self explanatory
    public int maxHealth;
    public int currentHealth;
    public Camera cam;

    //Invinciblity after being hit by enemy
    private float invincibilityCounter;
    public bool invincible;
    //Used for knockback force and for how long
    public float enemyForce;
    public float enemyknockTime;
    public float playerForce;
    public float playerknockTime;

    //tells if the player can move or not
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        currentHealth = maxHealth;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        canMove = true;
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

        MouseClickLine();

        if (currentHealth <= 0f)
        {
            //make sure to put the 'death sequence' name in here to run the graphics and everything else
        }
    }
    //Shows or hides line depending on mouse click
    private void MouseClickLine()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
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
    }

    //Player collision
    //makes it so if the enemy hits the player then the player loses health and jumps back from where it's been hit
    //also makes the player 'invincible' for a little bit after being hit so you don't just lose all your lives in one smash of enemies
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Knocks the enemy back if they touch the player
            //Checks for rigidbody of enemy
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            //If there is a rigidibody
            if (enemy != null)
            {
                //Makes it so we can use the force of the object as you can't do that in dynamic mode
                enemy.isKinematic = false;
                //Finds the difference between the player and the enemies positions
                Vector2 enemyDifference = enemy.transform.position - transform.position;
                //Finds the average of the 'difference' and multiplies it with the public force
                //Normalized make the vector a vector of 1
                enemyDifference = enemyDifference.normalized * enemyForce;
                //Adds the amount of force to the enemy for it to use to bounce off
                enemy.AddForce(enemyDifference, ForceMode2D.Impulse);
                //Starts the Coroutine that will make the enemy stop moving backwards - otherwise they'd just keep going off the screen
                StartCoroutine(KnockbackEnemyCo(enemy));

            }

            //Makes it so the player goes invincible for a little bit if hit by enemy
            if (invincibilityCounter == 0)
            {
                currentHealth -= 1;
                invincibilityCounter = 60;
            }

        }

        //makes it so if the enemy's bullet hits the player they lose health
        if (other.gameObject.tag == "EnemyBullet")
        {
            currentHealth -= 1;
        }

        if (currentHealth <= 0)
        {
            DeathSequence();
        }
    }

    //Player won't keep moving backwards
    //Turns Kinematic mode of player back on so it doesn't screw with the rest of our program
    private IEnumerator KnockbackEnemyCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(enemyknockTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
        }
    }

    private void DeathSequence()
    {
        //stops the player from moving
        canMove = false;
        //deletes the player - dies and disappears
        Destroy(gameObject);
    }
    //cam.orthographicSize = Transform.position.x + transform.position.y;

    //GetComponent<Camera>().orthographicSize -= 1;
    //camera position zooms onto player
    //theCamControl.DeathZoom();
    //player vibrates for a couple seconds
    //player explodes - delete object



    //Player firing - to do
    //private void Firing()
    //{
    //if(MouseClickLine)
    //{
    //firebullet - need bullet script from CJ
    //}
    //}





    //Below is the code needed to get the player to bounce off of the enemy, however this has to be put into the enemy code, not the players

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //   if (other.gameObject.tag == "Enemy")
    // {
    //   Rigidbody2D Player = other.GetComponent<Rigidbody2D>();
    // if (Player != null)
    //{
    //  Player.isKinematic = false;
    //Vector2 playerDifference = transform.position - enemy.transform.position;
    //playerDifference = playerDifference.normalized * playerForce;
    //Player.AddForce(playerDifference, ForceMode2D.Impulse);
    //StartCoroutine(KnockbackPlayerCo(Player));
    //}
    //}
    //}

    //private IEnumerator KnockbackPlayerCo(Rigidbody2D Player)
    //{
    //  if (Player != null)
    //{
    //  yield return new WaitForSeconds(playerknockTime);
    //Player.velocity = Vector2.zero;
    //Player.isKinematic = true;
    //}
    //}
}