using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public int Speed = 10;
    public GameObject PaddleLeft;
    public GameObject PaddleRight;
    public Text ScoreTextLeft;
    public Text ScoreTextRight;
    public AudioSource AudioSource;
    public AudioClip PaddleSound, WallSound, ScoreSound;
    public ParticleSystem ExplosionParticle;

    private Rigidbody2D Rigidbody;
    private System.Random Random;
    private int RightScore = 0;
    private int LeftScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Random = new System.Random();

        Invoke("StartBall", 2);
    }

    void StartBall()
    {
        float velocityX = Speed;
        float velocityY = (float)Random.NextDouble();

        velocityX = Random.Next(2) == 1 ? -velocityX : velocityX;
        velocityY = Random.Next(2) == 1 ? -velocityY : velocityY;

        Rigidbody.velocity = new Vector2(velocityX, velocityY);
        AudioSource.PlayOneShot(WallSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == PaddleLeft.name ||
            collision.gameObject.name == PaddleRight.name)
        {
            Vector2 velocity;
            velocity.x = Rigidbody.velocity.x;
            velocity.y = (Rigidbody.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            Rigidbody.velocity = velocity;
            AudioSource.PlayOneShot(PaddleSound);
        }

        else if (collision.gameObject.CompareTag("Left"))
        {
            PlayDeathAnimation();
            LeftScore++;
            ScoreTextLeft.text = LeftScore
            RestartGame();
        }

        else if (collision.gameObject.CompareTag("Right"))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        ResetBall();
        Invoke("StartBall", 1);
    }

    void ResetBall()
    {
        Rigidbody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }
}
