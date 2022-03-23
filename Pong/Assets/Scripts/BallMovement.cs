using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public bool GoingLeft { get; private set; }

    public bool newBounceMethod = true;
    [SerializeField] private int Speed = 10;
    [SerializeField] private GameObject PaddleLeft;
    [SerializeField] private GameObject PaddleRight;
    [SerializeField] private Text ScoreTextRight;
    [SerializeField] private Text ScoreTextLeft;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip PaddleSound;
    [SerializeField] private AudioClip WallSound;
    [SerializeField] private AudioClip ScoreSound;
    [SerializeField] private ParticleSystem ExplosionParticle;

    private Rigidbody2D Rigidbody;
    private int rightScore = 0;
    private int leftScore = 0;
    private System.Random rand;
    private bool isGhost = false;
    private bool paused = false;
    private Vector2 oldVelocity;

    public void Init(bool isGhost)
    {
        this.isGhost = isGhost;
        Rigidbody = GetComponent<Rigidbody2D>();

        rand = new System.Random();

        if (isGhost)
            StartBall();
        else
            Invoke(nameof(StartBall), 2);
    }

    void ResetBall()
    {
        Rigidbody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void RestartGame()
    {
        ResetBall();
        Invoke(nameof(StartBall), 1);
    }

    public void PauseBall()
    {
        paused = !paused;
        if (paused)
        {
            oldVelocity = Rigidbody.velocity;
            Rigidbody.velocity = Vector2.zero;
        }

        else 
        {
            Rigidbody.velocity = oldVelocity;
        }
    }

    void StartBall()
    {
        // Get random y start velocity
        float velocityX = Speed;
        float velocityY = (float)rand.NextDouble();

        // Select randomly if its negative
        velocityX = rand.Next(2) == 1 ? -velocityX : velocityX;
        velocityY = rand.Next(2) == 1 ? -velocityY : velocityY;

        GoingLeft = velocityX < 0;

        Rigidbody.velocity = new Vector2(velocityX, velocityY);

        if (!isGhost)
            AudioSource.PlayOneShot(WallSound);
    }

    private float HitFactor(Vector2 ballpos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballpos.y - paddlePos.y) / paddleHeight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGhost)
            return;

        // collision with paddle(s)
        if (collision.gameObject.name == PaddleLeft.name || collision.gameObject.name == PaddleRight.name)
        {
            GoingLeft = !GoingLeft;
            AudioSource.PlayOneShot(PaddleSound);

            Vector2 vel;
            if (newBounceMethod)
            {
                vel.x = Rigidbody.velocity.x;
                vel.y = (Rigidbody.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            }

            else
            {
                float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

                vel = new Vector2((collision.gameObject.name == PaddleLeft.name ? 1 : -1) * rand.Next(1, 2), y).normalized;

                vel *= Speed;
            }

            Rigidbody.velocity = vel;
        }

        // collision with left wall
        else if (collision.gameObject.CompareTag("Left"))
        {
            PlayDeathAnimation();
            rightScore++;
            ScoreTextRight.text = rightScore.ToString();
            RestartGame();
        }

        // collision with right wall
        else if (collision.gameObject.CompareTag("Right"))
        {
            PlayDeathAnimation();
            leftScore++;
            ScoreTextLeft.text = leftScore.ToString();
            RestartGame();
        }

        // collision with top or bottom wall
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

    public void SetVelocity(Vector2 newVelocity)
    {
        Rigidbody.velocity = newVelocity;
    }
}