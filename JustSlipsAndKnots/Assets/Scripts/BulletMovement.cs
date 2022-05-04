using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector2 Direction;
    public float Speed;

    private new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = Speed * Time.fixedDeltaTime * Direction;

        if (transform.position.x >  GameManager.Instance.ScreenSize.xMax ||
            transform.position.x < -GameManager.Instance.ScreenSize.xMax ||
            transform.position.y >  GameManager.Instance.ScreenSize.yMax ||
            transform.position.y < -GameManager.Instance.ScreenSize.yMax)
        {
            gameObject.SetActive(false);
        }
    }
}
