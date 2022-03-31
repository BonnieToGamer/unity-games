using UnityEngine;

public class Point : MonoBehaviour
{
	public Vector2 position, previousPosition;
	public bool locked;

    public Point(float x, float y)
    {
		this.position = this.previousPosition = new Vector2(x, y);
        this.locked = false;
    }

    private void Update()
    {
        transform.localPosition = position;
    }

    public override string ToString()
    {
        return $"{position.x}, {position.y} locked? {locked}";
    }
}