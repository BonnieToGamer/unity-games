using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public int MovementSpeed = 10;
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public float BoundY = 2.75f;

    private Rigidbody2D Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = Vector2.zero;
        if (Input.GetKey(Up))
            velocity.y = MovementSpeed;
        if (Input.GetKey(Down))
            velocity.y = -MovementSpeed;
        if (Input.GetKey(Up) && Input.GetKey(Down))
            velocity.y = 0;

        Rigidbody.velocity = velocity;
        Vector2 pos = transform.position;
        
        if (pos.y > BoundY)
            pos.y = BoundY;

        if (pos.y < -BoundY)
            pos.y = -BoundY;

        transform.position = pos;
    }
}
