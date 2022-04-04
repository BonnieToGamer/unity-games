using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public bool RunningSim { get; private set; }

    [SerializeField] private float gravity = 10f;
    [SerializeField] private float bounce = 0.1f;
    [SerializeField] private float friction = 0.999f;
    [SerializeField] private float numIterations = 5f;
    [SerializeField] private bool wallCollisions = true;
    [SerializeField] private bool useFriction = true;

    private List<Point> points;
    private List<Stick> sticks;
    private Rect ScreenSize;

    // Start is called before the first frame update
    void Start()
    {
        points = new List<Point>();
        sticks = new List<Stick>();
    }

    public void CalculateScreenSize(float distance, Vector3 pointSize)
    {
        distance -= Camera.main.transform.position.z;
        ScreenSize = new Rect
        {
            max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance)) - (pointSize / 2),
            min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)) + (pointSize / 2)
        };

    }

    // Update is called once per frame
    void Update()
    {
        RunningSim = Input.GetKeyDown(KeyCode.Space) ? !RunningSim : RunningSim;
        if (RunningSim)
        {
            Simulate();
            UpdatePoints();
            UpdateSticks();
        }

        for (int i = 0; i < sticks.Count; i++)
        {
            Stick s = sticks[i];
            if (s.pointA == null || s.pointB == null)
            {
                RemoveStick(s);
                Destroy(s.gameObject);
            }
        }
    }

    public void AddPoint(Point p)
    {
        points.Add(p);
    }

    public Point GetPoint(Vector2 pos)
    {
        return points.Find(p => p.position == pos);
    }

    public void AddStick(Stick stick)
    {
        sticks.Add(stick);
    }

    public void RemoveStick(Stick stick)
    {
        sticks.Remove(stick);
    }

    public void RemovePoint(Point point)
    {
        points.Remove(point);
    }

    public void SetLocked(float x, float y)
    {
        // find element
        int index = points.FindIndex(v => v.position.x == x && v.position.y == y);

        // set opposite
        Point p = points[index];
        p.locked = !p.locked;

        // set it back in array
        points[index] = p;
    }

    void Simulate()
    {
        sticks = Shuffle(sticks);

        // update points
        foreach (Point p in points)
        {
            if (!p.locked)
            {
                Vector2 positionBeforeUpdate = p.position;
                p.position += (p.position - p.previousPosition) * (useFriction ? friction : 1);
                p.position += gravity * Time.deltaTime * Time.deltaTime * Vector2.down;
                p.previousPosition = positionBeforeUpdate;
            }
        }

        for (int i = 0; i < numIterations; i++)
        {
            // update sticks
            foreach (Stick stick in sticks)
            {
                Vector2 stickCentre = (stick.pointA.position + stick.pointB.position) / 2;
                Vector2 stickDir = (stick.pointA.position - stick.pointB.position).normalized;
                if (!stick.pointA.locked)
                    stick.pointA.position = stickCentre + stickDir * stick.length / 2;
                if (!stick.pointB.locked)
                    stick.pointB.position = stickCentre - stickDir * stick.length / 2;
            }

            // constraint points
            foreach (Point p in points)
            {
                if (!p.locked && wallCollisions)
                {
                    Vector2 v = (p.position - p.previousPosition) * (useFriction ? friction : 1);

                    if (p.position.x > ScreenSize.xMax)
                    {
                        p.position.x = ScreenSize.xMax;
                        p.previousPosition.x = p.position.x + v.x * bounce;
                    }

                    else if (p.position.x < ScreenSize.xMin)
                    {
                        p.position.x = ScreenSize.xMin;
                        p.previousPosition.x = p.position.x + v.x * bounce;
                    }

                    if (p.position.y > ScreenSize.yMax)
                    {
                        p.position.y = ScreenSize.yMax;
                        p.previousPosition.y = p.position.y + v.x * bounce;
                    }

                    else if (p.position.y < ScreenSize.yMin)
                    {
                        p.position.y = ScreenSize.yMin;
                        p.previousPosition.y = p.position.y + v.x * bounce;
                    }
                }
            }
        }
    }

    void UpdatePoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i].UpdatePosition();
        }
    }

    void UpdateSticks()
    {
        for (int i = 0; i < sticks.Count; i++)
        {
            sticks[i].UpdatePosition();
        }
    }

    List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
