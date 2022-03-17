using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Invoke("ChangeLevel", 1);
    }

    private void ChangeLevel()
    {
        MapManager.ChangeLevel();
    }
}
