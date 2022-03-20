using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public bool SimpleAI = false;
    public bool BetterAI = false;
    public readonly float boundY = 2.75f;

    [SerializeField] private int MovementSpeed = 10;
    [SerializeField] private KeyCode up = KeyCode.UpArrow;
    [SerializeField] private KeyCode down = KeyCode.DownArrow;
   
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SimpleAI || BetterAI)
            return;

        #region playerMovement
        Vector2 velocity = Vector2.zero;
        if (Input.GetKey(up))
            velocity.y = MovementSpeed;

        if (Input.GetKey(down))
            velocity.y = -MovementSpeed;

        if (Input.GetKey(up) && Input.GetKey(down))
            velocity.y = 0;

        rigidBody.velocity = velocity;

        Vector2 pos = transform.position;
        if (pos.y > boundY)
            pos.y = boundY;

        else if (pos.y < -boundY)
            pos.y = -boundY;

        transform.position = pos;
        #endregion
    }

    public void MoveToPosition(float y, bool betterAI = true)
    {
        if (betterAI)
        {
            if (transform.position.y > y - 0.1f && transform.position.y < y + 0.1f)
                return;

            Vector3 target = new Vector2(transform.position.x, y);
            Vector3 direction = (target - transform.position).normalized;
            rigidBody.MovePosition(transform.position + 2 * MovementSpeed * Time.deltaTime * direction);
        }

        else
        {
            rigidBody.MovePosition(new Vector2(transform.position.x, y));
        }
    }
}
