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

    //particle effect that symbolises that the player died
    public GameObject DeathSplosion;
    public PlayerController Player;
    public GameObject bullet;

    //variables used for the player shaking and storing where the player died for the camera to zoom
    private Vector3 deathPos;
    private bool shaking;
    public float shakeAmount;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        //makes the player shake and the camera to zoom onto the players position
        if (shaking)
        {
            Vector3 newPos = deathPos + Random.insideUnitSphere * (Time.deltaTime * shakeAmount);
            newPos.z = transform.position.z;
            transform.position = newPos;

            cam.orthographicSize -= 0.05f;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(transform.position.x, transform.position.y, cam.transform.position.z), 0.3f);
        }
    }

    void FixedUpdate()
    {
        //Moves the player
        if (canMove) rb.MovePosition(new Vector2(Mathf.Clamp(rb.position.x + moveVelocity.x, playerMinX, playerMaxX), Mathf.Clamp(rb.position.y + moveVelocity.y, playerMinY, playerMaxY)));

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
        //makes it so if the player clicks with the mouse then a bullet will be fired
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Bullet newBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
            newBullet.instantiateBullet("player", 0.3f, transform.rotation, Color.green);
        }

        if (invincibilityCounter > 0)
        {
            invincibilityCounter -=1;
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
                StartCoroutine(KnockbackCo(enemy, "enemy"));

            }

            //is the same thing as above for the knockback of the enemies, but this is the knock back of the player
            rb.isKinematic = false;
            canMove = false;
            Vector2 playerDifference = transform.position - other.transform.position;
            playerDifference = playerDifference.normalized * playerForce;
            rb.AddForce(playerDifference, ForceMode2D.Impulse);
            StartCoroutine(KnockbackCo(rb, "Player"));

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
            StartCoroutine("DeathSequence");
        }
    }

    //Player won't keep moving backwards
    //Turns Kinematic mode of player back on so it doesn't screw with the rest of our program
    //allows for the player and the enemy to be knocked back from each other when they collide
    private IEnumerator KnockbackCo(Rigidbody2D rigidB, string type)
    {
        if (rigidB != null)
        {
            if (type == "enemy") yield return new WaitForSeconds(enemyknockTime);
            if (type == "player") yield return new WaitForSeconds(playerknockTime);
            if (type == "player") canMove = true;
            rigidB.velocity = Vector2.zero;
            rigidB.isKinematic = true;
        }
    }
    
    IEnumerator DeathSequence()
    {
        //stops the player from moving
        canMove = false;
        //stores the position in which the players health =< 0
        deathPos = transform.position;
        //makes shaking true so that the player will shake
        if (shaking == false)
        {
            shaking = true;
        }
        //waits for a little bit before making the player disappear
        yield return new WaitForSeconds(0.75f);
        shaking = false;
        //deletes the player - dies and disappears
        Player.gameObject.SetActive(false);
        //runs the particle effect
        Instantiate(DeathSplosion, Player.transform.position, Player.transform.rotation);
    }
}