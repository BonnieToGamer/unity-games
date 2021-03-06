using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	public CharacterController controller;
	public float speed = 12f;
	public Transform playerCamera;
	public float mouseSensetivity = 100f;

	[SerializeField] private LayerMask rayLayer;


	private float xRotation;
	private MazeCell currentCell;
	private MazeDirection currentDirection;
	private Vector3 standingOnCell;
	private Quaternion previousRotation;
	private MazeCell[,] maze;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void Initialize(MazeCell[,] maze)
	{
		this.maze = maze;
	}

	public void SetLocation(MazeCell cell)
	{
		if (currentCell != null)
			currentCell.OnPlayerExited();

		currentCell = cell;
		controller.enabled = false;
		transform.localPosition = new Vector3(cell.transform.localPosition.x, transform.localPosition.y, cell.transform.localPosition.z);
		controller.enabled = true;
		currentCell.OnPlayerEntered();
		standingOnCell = GetCellBelow();
	}

	private void Look(MazeDirection direction)
	{
		Debug.Log(direction);
		transform.localRotation = direction.ToRotation();
		currentDirection = direction;
	}

	private void Move(MazeDirection direction)
	{
		MazeCellEdge edge = currentCell.GetEdge(direction);
		if (edge is MazePassage)
			SetLocation(edge.otherCell);
	}

	private void GetCurrentDirection()
	{
		// Look
		Vector3 eulers = GetClosestRotation(transform.rotation.eulerAngles);
		MazeDirection direction = Quaternion.Euler(eulers.x, eulers.y, eulers.z).ToMazeDirection();
		currentDirection = direction;
	}

	private Vector3 GetClosestRotation(Vector3 rotation)
	{
		Vector3[] rotations = new Vector3[MazeDirections.rotations.Count()];
		for (int i = 0; i < MazeDirections.rotations.Count(); i++)
		{
			rotations[i] = MazeDirections.rotations[i].eulerAngles;
		}

		Vector3 nearest = rotations.OrderBy(x => Mathf.Abs((long)x.y - rotation.y)).First();
		return nearest;
	}

	private Vector3 GetClosestCells()
	{
		return maze.To1DArray<MazeCell>().OrderBy(x => Mathf.Abs((long)x.transform.localPosition.x - transform.localPosition.x))
													 .OrderBy(z => Mathf.Abs((long)z.transform.localPosition.z - currentCell.transform.localPosition.z)).First().transform.localPosition;
	}

	private void MoveCurrentCell()
	{
		// Move
		MazeCellEdge edge = currentCell.GetEdge(currentDirection);
		// Debug.Log(edge is MazePassage);
		Debug.Log(currentCell.transform.position.ToString() + " " + standingOnCell.ToString());
		if (edge is MazePassage && edge.transform.position == standingOnCell)
			SetCurrentCell(edge.otherCell);
	}

	private void SetCurrentCell(MazeCell cell)
	{
		if (currentCell != null)
			currentCell.OnPlayerExited();
		Debug.Log("previous cell: " + currentCell.Coordinates.ToString());

		currentCell = cell;

		Debug.Log("new cell: " + currentCell.Coordinates.ToString());
		currentCell.OnPlayerEntered();
	}

	void Update()
	{
		standingOnCell = GetCellBelow();
		previousRotation = transform.rotation;
		MouseAiming();
		KeyboardMovement();
		if (transform.rotation != previousRotation)
			GetCurrentDirection();
		if (transform.localPosition != standingOnCell)
			MoveCurrentCell();
		// Debug.DrawRay(transform.localPosition, Vector3.down * 2);
		// ShootRayCast();

		// if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		// 	Move(currentDirection);
		// else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		// 	Move(currentDirection.GetNextClockwise());
		// else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		// 	Move(currentDirection.GetOpposite());
		// else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		// 	Move(currentDirection.GetNextCounterclockwise());
		// else if (Input.GetKeyDown(KeyCode.Q))
		// 	Look(currentDirection.GetNextCounterclockwise());
		// else if (Input.GetKeyDown(KeyCode.E))
		// 	Look(currentDirection.GetNextClockwise());
	}

	void MouseAiming()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);
	}

	void KeyboardMovement()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		controller.Move(move * speed * Time.deltaTime);
	}

	private Vector3 GetCellBelow()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.localPosition, Vector3.down);
		if (Physics.Raycast(ray, hitInfo: out hit, 2))
		{
			return hit.transform.position;
		}

		return Vector3.zero;
	}
}