using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    [SerializeField] private float startTimeBtwSpawns;
    [SerializeField] private GameObject echo;

    private Player player;
    private float timeBtwSpawns;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Dashing)
        {
            if (timeBtwSpawns <= 0)
            {
                GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance, .3f);
                timeBtwSpawns = startTimeBtwSpawns;
            }

            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
