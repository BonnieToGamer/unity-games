using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public bool RunningSim { get; private set; }

    [SerializeField] private float gravity;
    [SerializeField] private float numIterations;

    private List<Point> prevPoints;
    private List<Stick> prevSticks;
    private List<Point> points;
    private List<Stick> sticks;

    // Start is called before the first frame update
    void Start()
    {
        points = prevPoints = new List<Point>();
        sticks = prevSticks = new List<Stick>();
    }

    // Update is called once per frame
    void Update()
    {
        RunningSim = Input.GetKeyDown(KeyCode.Space) ? !RunningSim : RunningSim;

        if (RunningSim)
        {
            Simulate(points.ToArray(), sticks.ToArray());
        }
    }

    public void AddPoint(float x, float y)
    {
        points.Add(new Point(x, y));
    }

    public void AddStick(Stick stick)
    {
        sticks.Add(stick);
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

    void Simulate(Point[] points, Stick[] sticks)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Point p = points[i];
            Debug.Log(points[i].position.y);

            if (!p.locked)
            {
                Vector2 positionBeforeUpdate = p.position;
                p.position += p.position - p.previousPosition;
                p.position += gravity * Time.deltaTime * Time.deltaTime * Vector2.down;
                p.previousPosition = positionBeforeUpdate;
            }

            points[i] = p;
            Debug.Log(points[i].position.y);
        }

        for (int i = 0; i < sticks.Length; i++)
        {
            for (int j = 0; j < numIterations; j++)
            {
                Stick stick = sticks[i];

                Vector2 stickCentre = (stick.pointA.position + stick.pointB.position) / 2;
                Vector2 stickDir = (stick.pointA.position - stick.pointB.position).normalized;

                if (!stick.pointA.locked)
                    stick.pointA.position = stickCentre + stickDir * stick.length / 2;

                if (!stick.pointB.locked)
                    stick.pointB.position = stickCentre + stickDir * stick.length / 2;

                sticks[i] = stick;
            }
        }
    }
}
