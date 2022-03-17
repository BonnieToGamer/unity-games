using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisManager : MonoBehaviour
{
    public float DebrisSpeed = 10f;
    public int SpawnRate = 10;
    public int BoundY = 5;
    public GameObject Debris;
    public GameObject Left;
    public GameObject Right;

    private static System.Random Random = new System.Random();
    private int SpawnCounter = 0;

    private void FixedUpdate()
    {
        if (!GameManager.Active)
            return;

        SpawnCounter = SpawnCounter++ > SpawnRate ? 0 : SpawnCounter;
     
        if (SpawnCounter > SpawnRate)
        {
            GameObject temp = Instantiate(Debris);
            temp.transform.parent = gameObject.transform;
            int rand = Random.Next(2);
            float Direction = rand == 0 ? -DebrisSpeed : DebrisSpeed;
            temp.transform.position = rand == 0 ? Right.transform.position : Left.transform.position;
            temp.transform.position = new Vector2(temp.transform.position.x, Random.Next(-BoundY + 2, BoundY));

            Rigidbody2D Rigidbody = temp.GetComponent<Rigidbody2D>();
            Rigidbody.velocity = new Vector2(Direction, 0);
        }
    }
}
