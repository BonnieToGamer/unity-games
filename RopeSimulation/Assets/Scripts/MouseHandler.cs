using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    [SerializeField] private GameObject Stick;
    [SerializeField] private GameObject Point;
    [SerializeField] private Simulation sim;
    [SerializeField] private float zSpawnOffset;
    [SerializeField] private Color pointerStickColor = Color.grey;
    [SerializeField] private LayerMask IgnoredLayerMask;
    [SerializeField] private LayerMask AcceptableLayerMask;
    [SerializeField] private float rayLength = 100;
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color greyColor;
    [SerializeField] private float scrollSpeed;

    private GameObject pointerStick;
    private Vector3 clickPoint;
    private Vector3 bigVector = new Vector3(1000, 1000, 1000);
    private Vector3 originalMousePoint;

    // Start is called before the first frame update
    void Start()
    {
        zSpawnOffset += Camera.main.transform.position.z;
        pointerStick = Instantiate(Stick, bigVector, Quaternion.identity);
        pointerStick.layer = IgnoredLayerMask;
        pointerStick.GetComponent<Renderer>().material.color = pointerStickColor;
        clickPoint = bigVector;
        sim.CalculateScreenSize(zSpawnOffset, Point.GetComponent<Renderer>().bounds.size);

        //Invoke(nameof(PlaceObjects), 0.1f);
    }

    void PlaceObjects()
    {
        for (int x = -16; x < 16; x += 2)
        {
            for (int y = -6; y < 10; y += 2)
            {
                Point p = Instantiate(Point, new Vector2(x, y), Quaternion.identity).GetComponent<Point>();
                p.position = p.previousPosition = new Vector2(x, y);
                p.zOffset = zSpawnOffset;
                p.red = redColor;
                p.white = whiteColor;

                if (y == 8)
                {
                    p.locked = true;
                }

                p.UpdatePosition();

                sim.AddPoint(p);
            }
        }

        for (int x = -16; x < 16; x += 2)
        {
            for (int y = -6; y < 10; y += 2)
            {
                if (y != 8 && x != 14)
                {
                    Vector2 vector = new Vector2(x, y);
                    Stick sh = Instantiate(Stick, Vector2.zero, Quaternion.identity).GetComponent<Stick>();
                    sh.pointA = sim.GetPoint(vector);
                    sh.pointB = sim.GetPoint(vector + new Vector2(2, 0));
                    sh.zOffset = zSpawnOffset;
                    Vector2 difference = sh.pointB.position - sh.pointA.position;
                    sh.length = Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y);
                    sh.UpdatePosition();
                    sim.AddStick(sh);

                    Stick sv = Instantiate(Stick, Vector2.zero, Quaternion.identity).GetComponent<Stick>();
                    sv.pointA = sim.GetPoint(vector);
                    sv.pointB = sim.GetPoint(vector + new Vector2(0, 2));
                    sv.zOffset = zSpawnOffset;
                    difference = sv.pointB.position - sv.pointA.position;
                    sv.length = Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y);
                    sv.UpdatePosition();
                    sim.AddStick(sv);
                }

                else if (x == 14 && y != 8)
                {
                    Vector2 vector = new Vector2(x, y);
                    Stick sv = Instantiate(Stick, Vector2.zero, Quaternion.identity).GetComponent<Stick>();
                    sv.pointA = sim.GetPoint(vector);
                    sv.pointB = sim.GetPoint(vector + new Vector2(0, 2));
                    sv.zOffset = zSpawnOffset;
                    Vector2 difference = sv.pointB.position - sv.pointA.position;
                    sv.length = Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y);
                    sv.UpdatePosition();
                    sim.AddStick(sv);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zSpawnOffset - Camera.main.transform.position.z; // take minus since ScreenToWorldPoint takes account of z pos.

        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

            // If ray hit a point change it lock state and color
            if (Physics.Raycast(rayMouse, out RaycastHit hit, rayLength, AcceptableLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Point"))
                {
                    Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                    renderer.material.color = renderer.material.color == whiteColor ? redColor : whiteColor;

                    Point p = hit.transform.gameObject.GetComponent<Point>();
                    p.locked = !p.locked;
                }
            }

            // else add a new point
            else
            {
                Point p = Instantiate(Point, objectPos, Quaternion.identity).GetComponent<Point>();
                p.position = p.previousPosition = objectPos;
                p.zOffset = zSpawnOffset;
                p.red = redColor;
                p.white = whiteColor;

                p.UpdatePosition();

                sim.AddPoint(p);
            }
        }

        // Middle mouse button
        if (Input.GetMouseButton(2))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if ray hit something and it's a stick destroy the stick
            if (Physics.Raycast(rayMouse, out RaycastHit hit, rayLength, AcceptableLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Stick"))
                {
                    sim.RemoveStick(hit.transform.gameObject.GetComponent<Stick>());
                    Destroy(hit.transform.gameObject);
                }

                if (hit.transform.gameObject.CompareTag("Point"))
                {
                    sim.RemovePoint(hit.transform.gameObject.GetComponent<Point>());
                    Destroy(hit.transform.gameObject);
                }
            }
        }

        // Holding right mouse
        if (Input.GetMouseButton(1))
        {
            // set clickpoint if not already set
            if (clickPoint == bigVector)
            {
                clickPoint = objectPos;
                originalMousePoint = Input.mousePosition;
            }

            // draw pointerStick between two points
            Vector3 startPos = clickPoint;
            Vector3 endPos = objectPos;

            Transform pipe = Pipe(pointerStick.transform, startPos, endPos);

            pointerStick.transform.localScale = pipe.localScale;
            pointerStick.transform.position = new Vector3(pipe.position.x, pipe.position.y, zSpawnOffset);
            pointerStick.transform.up = pipe.up;
        }

        // release right mouse
        if (Input.GetMouseButtonUp(1))
        {
            // shoot two rays and check if both collide with points, if so add a Stick between them
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray rayStart = Camera.main.ScreenPointToRay(originalMousePoint);
            if (Physics.Raycast(rayStart, out RaycastHit hitStart, rayLength, AcceptableLayerMask))
            {
                if (Physics.Raycast(rayMouse, out RaycastHit hitEnd, rayLength, AcceptableLayerMask))
                {
                    if (hitEnd.transform.gameObject.CompareTag("Point") && hitStart.transform.gameObject.CompareTag("Point") &&
                        hitEnd.transform.position != hitStart.transform.position)
                    {
                        // create the new stick
                        GameObject newStick = Instantiate(Stick, Vector3.zero, Quaternion.identity);

                        // set color
                        newStick.GetComponent<Renderer>().material.color = greyColor;

                        // set its position
                        Transform pipe = Pipe(pointerStick.transform, hitStart.transform.position, hitEnd.transform.position);

                        newStick.transform.localScale = pipe.localScale;
                        newStick.transform.position = pipe.position;
                        newStick.transform.up = pipe.up;

                        // set its variables
                        Stick stickScript = newStick.GetComponent<Stick>();

                        stickScript.pointA = hitStart.transform.gameObject.GetComponent<Point>();
                        stickScript.pointB = hitEnd.transform.gameObject.GetComponent<Point>();
                        stickScript.zOffset = zSpawnOffset;

                        Vector2 difference = stickScript.pointB.position - stickScript.pointA.position;
                        float distance = Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y);

                        stickScript.length = distance;

                        sim.AddStick(stickScript);
                    }
                }
            }

            // remove pointerStick from view
            clickPoint = bigVector;
            pointerStick.transform.position = bigVector;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.transform.position = Camera.main.transform.position - new Vector3(0, 0, scrollSpeed);
            sim.CalculateScreenSize(zSpawnOffset, Point.GetComponent<Renderer>().bounds.size);
        }

        else if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(0, 0, scrollSpeed);
            sim.CalculateScreenSize(zSpawnOffset, Point.GetComponent<Renderer>().bounds.size);
        }
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
        transform.position = middlePoint;

        // change rotation
        Vector3 rotationDirection = (endPos - startPos);
        transform.up = rotationDirection;

        return transform;
    }
}