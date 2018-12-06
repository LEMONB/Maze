using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = 0, Right, Up, Down };
public enum Building { HorizontalWall = 0, VerticalWall, RemoveHorizontalWall, RemoveVerticalWall };

public static class Utilities
{
	public static int FieldWidth { get; set; } = 10;

	public static int FieldHeight { get; set; } = 10;

	public static Building SelectedBuilding = Building.HorizontalWall;

	[SerializeField]
	public static GameObject WallPrefab { get; set; }


	public static bool ShowConstruction { get; set; } = false;

	public static bool ShowControls { get; set; } = true;

	private static System.Random rng = new System.Random();
	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
