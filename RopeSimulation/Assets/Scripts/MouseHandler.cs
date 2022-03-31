using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    [SerializeField] private GameObject Stick;
    [SerializeField] private GameObject Point;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Simulation sim;
    [SerializeField] private float zSpawnOffset;
    [SerializeField] private Color pointerStickColor = Color.grey;
    [SerializeField] private LayerMask IgnoredLayerMask;
    [SerializeField] private LayerMask AcceptableLayerMask;
    [SerializeField] private float rayLength = 100;

    private GameObject pointerStick;
    private Vector3 clickPoint;
    private Vector3 bigVector = new Vector3(1000, 1000, 1000);
    private Vector3 originalMousePoint;

    // Start is called before the first frame update
    void Start()
    {
        pointerStick = Instantiate(Stick, bigVector, Quaternion.identity);
        pointerStick.layer = IgnoredLayerMask;
        pointerStick.GetComponent<Renderer>().material.color = pointerStickColor;
        clickPoint = bigVector;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zSpawnOffset;

        Vector3 objectPos = mainCamera.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            Ray rayMouse = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouse, out RaycastHit hit, rayLength, AcceptableLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Point"))
                {
                    Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                    renderer.material.color = renderer.material.color == Color.white ? Color.red : Color.white;

                    Point p = hit.transform.gameObject.GetComponent<Point>();
                    p.locked = !p.locked;
                }
            }

            else
            {
                Point p = Instantiate(Point, objectPos, Quaternion.identity).GetComponent<Point>();
                p.position = p.previousPosition = objectPos;

                sim.AddPoint(objectPos.x, objectPos.y);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray rayMouse = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouse, out RaycastHit hit, rayLength, AcceptableLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Stick"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (clickPoint == bigVector)
            {
                clickPoint = objectPos;
                originalMousePoint = Input.mousePosition;
            }

            Vector3 startPos = clickPoint;
            Vector3 endPos = objectPos;

            Transform pipe = Pipe(pointerStick.transform, startPos, endPos);

            pointerStick.transform.localScale = pipe.localScale;
            pointerStick.transform.position = pipe.position;
            pointerStick.transform.up = pipe.up;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Ray rayMouse = mainCamera.ScreenPointToRay(Input.mousePosition);
            Ray rayStart = mainCamera.ScreenPointToRay(originalMousePoint);
            if (Physics.Raycast(rayStart, out RaycastHit hitStart, rayLength, AcceptableLayerMask))
            {
                if (Physics.Raycast(rayMouse, out RaycastHit hitEnd, rayLength, AcceptableLayerMask))
                {
                    if (hitEnd.transform.gameObject.CompareTag("Point") && hitStart.transform.gameObject.CompareTag("Point") &&
                        hitEnd.transform.position != hitStart.transform.position)
                    {
                        GameObject newStick = Instantiate(Stick, Vector3.zero, Quaternion.identity);
                        Transform pipe = Pipe(pointerStick.transform, hitStart.transform.position, hitEnd.transform.position);

                        newStick.transform.localScale = pipe.localScale;
                        newStick.transform.position = pipe.position;
                        newStick.transform.up = pipe.up;

                        Stick stickScript = newStick.GetComponent<Stick>();

                        stickScript.pointA = hitStart.transform.gameObject.GetComponent<Point>();
                        stickScript.pointB = hitEnd.transform.gameObject.GetComponent<Point>();
                        stickScript.length = newStick.transform.localScale.y;

                        sim.AddStick(stickScript);
                    }
                }
            }
            clickPoint = bigVector;
            pointerStick.transform.position = bigVector;
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
