using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private string enemeyTag;
    [SerializeField] private Material selected, normal;
    [SerializeField] private Button attackButton;

    private IEnemy selectedEnemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit && hit.transform.gameObject.CompareTag(enemeyTag))
            {
                try
                {
                    selectedEnemy.Material = normal;
                }
                catch (Exception ex)
                {
                    if (ex is MissingReferenceException || ex is NullReferenceException)
                        selectedEnemy = null;
                }

                Debug.Log("Selected: " + hit.transform.gameObject.name);
                selectedEnemy = hit.transform.gameObject.GetComponent<IEnemy>();
                selectedEnemy.Material = selected;
            }
        }
    }

    public void Attack()
    {
        try
        {
            attackButton.interactable = false;
            Vector2 previousPos = transform.position;

            // move a to b
            StartCoroutine(Utilities.SmoothMove(transform.position, selectedEnemy.Position, 75, 100,
            (res) =>
            {
                transform.position = res;
            },
            () =>
            {
                selectedEnemy.Attack(10);
                StartCoroutine(Utilities.SmoothMove(transform.position, previousPos, 75, 100,
                (res) =>
                {
                    transform.position = res;
                }));
            }));

            attackButton.interactable = true;
        }
        catch (Exception ex)
        {
            if (ex is MissingReferenceException || ex is NullReferenceException)
                selectedEnemy = null;
            attackButton.interactable = true;
        }
    }
}
