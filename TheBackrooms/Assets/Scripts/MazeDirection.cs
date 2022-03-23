using UnityEngine;

public enum MazeDirection
{
	North,
	East,
	South,
	West
}

public static class MazeDirections
{
	public const int Count = 4;

	public static MazeDirection RandomValue
	{
		get
		{
			return (MazeDirection)Random.Range(0, Count);
		}
	}

	private static IntVector2[] vectors =
	{
		new IntVector2(0,1),
		new IntVector2(1,0),
		new IntVector2(0,-1),
		new IntVector2(-1,0)
	};

	public static IntVector2 ToIntVector2(this MazeDirection direction)
	{
		return vectors[(int)direction];
	}

	private static MazeDirection[] opposites = {
		MazeDirection.South,
		MazeDirection.West,
		MazeDirection.North,
		MazeDirection.East
	};

	public static MazeDirection GetOpposite(this MazeDirection direction)
	{
		return opposites[(int)direction];
	}

	public static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270, 0f),
		Quaternion.Euler(0f, 359, 0f)
	};

	public static Quaternion ToRotation(this MazeDirection direction)
	{
		return rotations[(int)direction];
	}

	public static MazeDirection ToMazeDirection(this Quaternion directon)
	{
		if (rotations[0] == directon)
			return MazeDirection.North;
		else if (rotations[1] == directon)
			return MazeDirection.East;
		else if (rotations[2] == directon)
			return MazeDirection.South;
		else if (rotations[3] == directon)
			return MazeDirection.West;
		else if (rotations[4] == directon)
			return MazeDirection.North;

		throw new System.Exception("Unkown direction");
	}

	public static MazeDirection GetNextClockwise(this MazeDirection direction)
	{
		return (MazeDirection)(((int)direction + 1) % Count);
	}

	public static MazeDirection GetNextCounterclockwise(this MazeDirection direction)
	{
		return (MazeDirection)(((int)direction + Count - 1) % Count);
	}
}