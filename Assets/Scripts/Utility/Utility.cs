using UnityEngine;
using System.Collections;

public class Utility {

	public static Point GridIndexFromWorldPosition(Point gridSize, Point gridCenter, Point position) {
		float percentX = (position.x + (float)gridSize.x / 2 - gridCenter.x) / gridSize.x;
		float percentY = (position.y + (float)gridSize.y / 2 - gridCenter.y) / gridSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

		return new Point(x, y);
	}

}
