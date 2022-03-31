using UnityEngine;

public class Stick : MonoBehaviour
{
    public Point pointA, pointB;
    public float length;

    public Stick(Point pointA, Point pointB, float length)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        this.length = length;
    }
}