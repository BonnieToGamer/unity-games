using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public int Speed = 10;
    public GameObject PaddleLeft;
    public GameObject PaddleRight;
    public Text ScoreTextRight;
    public Text ScoreTextLeft;
    public AudioSource AudioSource;
    public AudioClip PaddleSound;
    public AudioClip WallSound;
    public AudioClip ScoreSound;
    public ParticleSystem ExplosionParticle;

    private int rightScore = 0;
    private int leftScore = 0;
    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        Rigidbody = GetComponent<Rigidbody2D>();

        Invoke("StartBall", 2);
    }

    void ResetBall()
    {
        Rigidbody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void RestartGame()
    {
        ResetBall();
        Invoke("StartBall", 1);
    }


    void StartBall()
    {
        // Get random y start velocity
        float velocityX = Speed;
        float velocityY = (float)rand.NextDouble();

        // Select randomly if its negative
        velocityX = rand.Next(2) == 1 ? -velocityX : velocityX;
        velocityY = rand.Next(2) == 1 ? -velocityY : velocityY;

        Rigidbody.velocity = new Vector2(velocityX, velocityY);
        AudioSource.PlayOneShot(WallSound);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == PaddleLeft.gameObject.name || collision.gameObject.name == PaddleRight.gameObject.name)
        {
            AudioSource.PlayOneShot(PaddleSound);
            Vector2 vel;
            vel.x = Rigidbody.velocity.x;
            vel.y = (Rigidbody.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            Rigidbody.velocity = vel;
        }

        else if (collision.gameObject.tag == "Left")
        {
            PlayDeathAnimation();
            rightScore++;
            ScoreTextRight.text = rightScore.ToString();
            RestartGame();
        }

        else if (collision.gameObject.tag == "Right")
        {
            PlayDeathAnimation();
            leftScore++;
            ScoreTextLeft.text = leftScore.ToString();
            RestartGame();
        }

        else
        {
            AudioSource.PlayOneShot(WallSound);
        }
    }

    private void PlayDeathAnimation()
    {
        AudioSource.PlayOneShot(ScoreSound);
        ExplosionParticle.transform.position = transform.position;
        ExplosionParticle.Play();
    }
}