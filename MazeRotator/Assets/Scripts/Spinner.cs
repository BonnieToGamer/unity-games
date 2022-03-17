using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public int RotationSpeed = 50;

    private void FixedUpdate()
    {
        transform.eulerAngles += Vector3.forward * (RotationSpeed * Time.fixedDeltaTime);
    }
}
