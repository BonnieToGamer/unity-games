using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{
    public KeyCode RightKey = KeyCode.RightArrow;
    public KeyCode LeftKey = KeyCode.LeftArrow;

    public int RotateSpeed = 75;

    private bool Right;
    private bool Left;

    // Update is called once per frame
    void Update()
    {
        Right = Input.GetKey(RightKey);
        Left = Input.GetKey(LeftKey);
    }

    private void FixedUpdate()
    {
        if (Right && Left)
            return;
        if (Right)
            transform.eulerAngles -= Vector3.forward * (RotateSpeed * Time.fixedDeltaTime);
        if (Left)
            transform.eulerAngles += Vector3.forward * (RotateSpeed * Time.fixedDeltaTime);
    }
}
