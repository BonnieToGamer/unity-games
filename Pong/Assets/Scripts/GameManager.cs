using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] BallMovement Ball;

    // Start is called before the first frame update
    void Start()
    {
        Ball.Init(false);
    }
}
