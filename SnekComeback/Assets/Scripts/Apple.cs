using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public void MovePosition(float tileSize, Vector2 boardSize)
    {
        float randomx = (Mathf.Round(Random.Range(-boardSize.x, boardSize.x) / tileSize)) * tileSize;
        float randomy = (Mathf.Round(Random.Range(-boardSize.y, boardSize.y) / tileSize)) * tileSize;
        Vector3 newPos = new Vector3(randomx, randomy);

        while (newPos == transform.position)
        {
            randomx = (Mathf.Round(Random.Range(-boardSize.x, boardSize.x) / tileSize)) * tileSize;
            randomy = (Mathf.Round(Random.Range(-boardSize.y, boardSize.y) / tileSize)) * tileSize;
            newPos = new Vector3(randomx, randomy);
        }

        transform.position = newPos;
    }
}
