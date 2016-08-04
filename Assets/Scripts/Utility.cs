using UnityEngine;
using System.Collections;

public class Utility {

	public static Vector2 GridIndexFromWorldPosition(Vector2 gridSize, Vector2 gridCenter, Vector2 position) {
		float percentX = (position.x + gridSize.x / 2 - gridCenter.x) / gridSize.x;
		float percentY = (position.y + gridSize.y / 2 - gridCenter.y) / gridSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

		return new Vector2(x, y);
	}

}
