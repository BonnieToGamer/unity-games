using UnityEngine;

public class Stick : MonoBehaviour
{
    public Point pointA, pointB;
    public float length, zOffset;

    public Stick(Point pointA, Point pointB, float length)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        this.length = length;
    }

    public void UpdatePosition()
    {
        Transform pipe = Pipe(this.transform, pointA.position, pointB.position);

        this.transform.localScale = pipe.localScale;
        this.transform.position = pipe.position;
        this.transform.up = pipe.up;
    }
    private Transform Pipe(Transform original, Vector3 startPos, Vector3 endPos)
    {
        Transform transform = original;
        Vector3 initialScale = original.localScale;

        // change scale
        float distance = Vector3.Distance(startPos, endPos);
        transform.localScale = new Vector3(initialScale.x, distance / 2f, initialScale.z);

        // change position
        Vector3 middlePoint = (startPos + endPos) / 2f;
        middlePoint.z = zOffset;
        transform.position = middlePoint;

        // change rotation
        Vector3 rotationDirection = (endPos - startPos);
        transform.up = rotationDirection;

        return transform;
    }
}