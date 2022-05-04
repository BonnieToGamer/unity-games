using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snek : MonoBehaviour
{
    public float TileSize;
    public float StepTimer;
    public Vector2 boardSize;

    private Direction direction;
    private Direction previousDirection;
    private List<Direction> queue;
    private Vector2 movement;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        queue = new List<Direction>();
        queue.Add(Direction.Right);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queue.Add(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queue.Add(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queue.Add(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queue.Add(Direction.Right);
        }
    }

    private void FixedUpdate()
    {
        if (++timer >= StepTimer)
        {
            timer = 0;
            if (queue.Count != 0)
            {
                previousDirection = direction;
                direction = queue[0];
                queue.RemoveAt(0);
            }
            switch (direction)
            {
                case Direction.Up:
                    ChangeDirection(Vector3.up);
                    break;

                case Direction.Left:
                    ChangeDirection(Vector3.left);
                    break;

                case Direction.Down:
                    ChangeDirection(Vector3.down);
                    break;

                case Direction.Right:
                    ChangeDirection(Vector3.right);
                    break;
            }
        }
    }

    private void ChangeDirection(Vector3 direction)
    {
        if (ToVector(previousDirection) != direction * -1)
        {
            transform.localPosition += direction * TileSize;
        }
        
        else
        {
            transform.localPosition += ToVector(previousDirection) * TileSize;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.CompareTag("Apple"))
        {
            collision.gameObject.GetComponent<Apple>().MovePosition(TileSize, boardSize);
        }

        else
        {
            Debug.Log("ded");
        }
    }

    private Vector3 ToVector(Direction direction)
    {
        if (direction == Direction.Up)
        {
            return Vector3.up;
        }

        else if (direction == Direction.Left)
        {
            return Vector3.left;
        }

        else if (direction == Direction.Down)
        {
            return Vector3.down;
        }

        else
        {
            return Vector3.right;
        }
    }

    enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }
}
