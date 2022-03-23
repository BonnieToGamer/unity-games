using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Maze MazePrefab;
	public Player playerPrefab;

	private Maze mazeInstance;
	private Player playerInstance;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(BeginGame());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			RestartGame();
		if (Input.GetKeyDown(KeyCode.Return))
			mazeInstance.ShowEveryRoom();
		if (Input.GetKeyDown(KeyCode.Backspace))
			mazeInstance.HideEveryRoom();
	}

	private IEnumerator BeginGame()
	{
		mazeInstance = Instantiate(MazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate());
		playerInstance = Instantiate(playerPrefab) as Player;
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
	}

	private void RestartGame()
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null)
			Destroy(playerInstance.gameObject);
		StartCoroutine(BeginGame());
	}
}
