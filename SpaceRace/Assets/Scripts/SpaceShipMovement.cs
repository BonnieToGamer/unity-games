using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMovement : MonoBehaviour
{
    public float MovementSpeed = 2.5f;
    public bool IsLeft;
    public KeyCode UpKey = KeyCode.W;
    public Text LeftScoreText;
    public Text RightScoreText;

    public static int LeftScore;
    public static int RightScore;

    private bool UpKeyPressed = false;
    private Rigidbody2D Rigidbody;
    private Vector2 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpKeyPressed = false;
        if (!GameManager.Active)
            return;

        UpKeyPressed = Input.GetKey(UpKey);
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        velocity.y = UpKeyPressed ? MovementSpeed : 0;

        Rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Top"))
        {
            if (IsLeft)
            {
                LeftScore++;
                LeftScoreText.text = LeftScore.ToString();
            }
            else
            {
                RightScore++;
                RightScoreText.text = RightScore.ToString();
            }

                transform.position = StartPosition;
        }

        if (collision.gameObject.CompareTag("Debris"))
            transform.position = StartPosition;
    }
}
