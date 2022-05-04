using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region physcis
    [Header("Physics")]

    [Range(0, 1)]
    [Tooltip("The lower the number the higher the friction")]
    [SerializeField] private float friction;
    [SerializeField] private float speed;
    [Range(0, 1000)]
    [SerializeField] private float gravity;
    #endregion

    private Vector2 position;
    private Vector2 previousPosition;
    private Vector2 movement;
    private bool[] collision;
    private Ray2D[] rays;
    private float raycastLength;
    private Bounds playerSize;
    private Bounds[] boundCollisions;

    // Start is called before the first frame update
    void Start()
    {
        position = previousPosition = transform.position;
        collision = new bool[4]; // 4 for the number of directions
        rays = new Ray2D[4]
        {
            new Ray2D(transform.position, Vector2.up),
            new Ray2D(transform.position, Vector2.left),
            new Ray2D(transform.position, Vector2.down),
            new Ray2D(transform.position, Vector2.right)
        };

        boundCollisions = new Bounds[4] { new Bounds(), new Bounds(), new Bounds(), new Bounds() };

        playerSize = GetComponent<Renderer>().bounds;
        raycastLength = playerSize.size.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 positionBeforeUpdate = position;

        CheckCollision();

        CalculateMovement();
        CalculateJump();

        previousPosition = positionBeforeUpdate;
        transform.position = position;
    }

    private void CheckCollision()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            // update origin
            rays[i].origin = position;

            // shoot ray
            RaycastHit2D hit = Physics2D.Raycast(rays[i].origin, rays[i].direction, raycastLength);
            Debug.DrawRay(rays[i].origin, rays[i].direction);
            collision[i] = hit.collider != null;

            if (hit.collider != null)
            {
                boundCollisions[i] = hit.collider.gameObject.GetComponent<Renderer>().bounds;
            }
        }
    }

    private void CalculateMovement()
    {
        position += (position - previousPosition) * friction; // apply velocity

        if ((movement.x < 0 && !collision[1]) || (movement.x > 0 && !collision[3]))
            position.x += speed * Time.fixedDeltaTime * movement.x; // apply movement

        else if (collision[1] || collision[3])
        {
            position.x = collision[1] ? boundCollisions[1].max.x + raycastLength : boundCollisions[3].min.x - raycastLength;
        }
    }

    private void CalculateJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump
        }

        else if (!collision[2])
        {
            position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime * Vector2.down; // gravity
        }

        else
        {
            //position.y = boundCollisions[2].max.y + raycastLength;
        }
    }
}