using UnityEngine;

public class Point : MonoBehaviour
{
	public Vector2 position, previousPosition;
	public bool locked;
    public float zOffset;
    public Color white, red;

    public Point(float x, float y)
    {
		this.position = this.previousPosition = new Vector2(x, y);
        this.locked = false;
    }

    public void UpdatePosition()
    {
        transform.position = new Vector3(position.x, position.y, zOffset);

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = locked ? red : white;
    }

    public override string ToString()
    {
        return $"({position.x}, {position.y}, {zOffset}) locked? {locked}";
    }
}