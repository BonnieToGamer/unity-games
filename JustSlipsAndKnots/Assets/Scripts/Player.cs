using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public Vector2 Movement { get => movement; }
    public bool Dashing;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float limiter = .75f;

    [Header("Dash variables")]
    [SerializeField] private float dashLength = .5f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashSpeed;
    

    private new Rigidbody2D rigidbody;
    private Vector2 movement;
    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCoolCounter;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                Dashing = true;
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                Dashing = false;
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        dashCoolCounter -= dashCoolCounter > 0 ? Time.deltaTime : 0;
    }

    private void FixedUpdate()
    {
        if (movement.x != 0 && movement.y != 0)
            movement *= limiter;

        movement.Normalize();
        rigidbody.velocity = activeMoveSpeed * Time.fixedDeltaTime * movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

        }
    }
}
