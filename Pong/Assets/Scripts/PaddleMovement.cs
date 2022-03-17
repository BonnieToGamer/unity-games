using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public int MovementSpeed = 10;
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public Rigidbody2D RigidBody;
    public float boundY = 4;

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = Vector2.zero;
        if (Input.GetKey(Up))
        {
            velocity.y = MovementSpeed;
        }

        if (Input.GetKey(Down))
        {
            velocity.y = -MovementSpeed;
        }

        if (Input.GetKey(Up) && Input.GetKey(Down))
        {
            velocity.y = 0;
        }

        RigidBody.velocity = velocity;

        Vector2 pos = transform.position;
        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < -boundY)
        {
            pos.y = -boundY;
        }

        transform.position = pos;
    }
}
