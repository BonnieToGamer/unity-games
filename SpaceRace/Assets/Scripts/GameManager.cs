using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider Slider;
    public Text WinText;
    public Text ResText;
    public Text LeftScore;
    public Text RightScore;
    public GameObject LeftShip;
    public GameObject RightShip;
    public static bool Active = true;

    private Vector2 LeftStart;
    private Vector2 RightStart;

    // Start is called before the first frame update
    void Start()
    {
        LeftStart  = LeftShip.transform.position;
        RightStart = RightShip.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Slider.value -= Time.deltaTime;
        if (Slider.value > 0)
            return;

        Active = false;
        
        WinText.text = SpaceShipMovement.LeftScore == SpaceShipMovement.RightScore ? "Tie" : SpaceShipMovement.LeftScore > SpaceShipMovement.RightScore ? "Player 1 Wins!" : "Player 2 Wins!";
        ResText.text = "Press Space To Restart";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Active = true;
            Slider.value = 100;
            WinText.text = "";
            ResText.text = "";
            LeftScore.text = "0";
            RightScore.text = "0";
            SpaceShipMovement.LeftScore = 0;
            SpaceShipMovement.RightScore = 0;
            LeftShip.transform.position = LeftStart;
            RightShip.transform.position = RightStart;
        }
    }
}
