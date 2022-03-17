using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public GameObject ConnectedPortal;
    private static bool JustTeleported = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !JustTeleported)
        {
            JustTeleported = true;
            collision.transform.position = ConnectedPortal.GetComponent<Collider2D>().transform.position;
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && JustTeleported)
        {
            StartCoroutine("WaitAndTurnFalse");
        }
    }

    private IEnumerator  WaitAndTurnFalse()
    {
        yield return new WaitForSeconds(1);
        JustTeleported = false;
    }
}
