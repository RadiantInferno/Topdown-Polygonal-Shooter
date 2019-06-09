using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Vector3 mousePosition;

    private Rigidbody2D rb;
    private LineRenderer lr;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
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
    }

    void FixedUpdate()
    {
        //Moves the player
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        //Makes the player point towards the mouse cursor
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 mouseDirection = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = mouseDirection;
    }
}
