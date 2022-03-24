public static class Utilities
{
	public static T[] To1DArray<T>(this T[,] input)
	{
		// Get the length of previous array
		// and make a 1D array of that size
		int size = input.Length;
		T[] result = new T[size];

		int write = 0;

		// loop thorugh 2d array
		for (int i = 0; i <= input.GetUpperBound(0); i++)
		{
			for (int z = 0; z <= input.GetUpperBound(1); z++)
			{
				// write 2d data to the 1d array
				result[write++] = input[i, z];
			}
		}

		return result;
	}
}