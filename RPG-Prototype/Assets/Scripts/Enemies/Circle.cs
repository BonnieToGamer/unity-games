using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IEnemy
{
    #region variables
    public float Health { get; set; }
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    public Material Material
    {
        set
        {
            GetComponent<Renderer>().material = value;
        }
    }

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public Slider HealthBar { get; set; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        if (HealthBar)
            HealthBar.maxValue = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(float damage)
    {
        Vector3 positionBeforeUpdate = transform.position;
        transform.position -= new Vector3(0.5f, 0, 0f);
        StartCoroutine(Utilities.Spring(transform.position, positionBeforeUpdate, 0.9f, 100,
        (res) =>
        {
            transform.position = new Vector3(res.x, transform.position.y, transform.position.z);
        }));
        Health -= damage;
        if (HealthBar)
            HealthBar.value = Health;

        Debug.Log(name + " attacked for " + damage + " damage. Health is at: " + Health);
        if (Health <= 0)
        {
            Debug.Log(name + " died");
            Destroy(HealthBar.transform.gameObject);
            Destroy(gameObject);
        }
    }
}
