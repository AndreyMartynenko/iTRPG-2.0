using UnityEngine;
using System.Collections;

public class Utility {

	public static Point GridIndexWithWorldPosition(Point gridSize, Point gridCenter, Point position) {
		float percentX = (position.x + (float)gridSize.x / 2 - gridCenter.x) / gridSize.x;
		float percentY = (position.y + (float)gridSize.y / 2 - gridCenter.y) / gridSize.y;

		if (percentX < 0 || percentX > 1 || percentY < 0 || percentY > 1) {
			return new Point(-1, -1);
		}

		int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

		return new Point(x, y);
	}

	public static Vector3 WorldPositionWithGridIndex(Point index) {
		return new Vector3(index.x, 0, index.y);
	}

	public static bool IsValidTileAt(Point mainMapSize, Point position) {
		return (position.x >= 0 && position.x < mainMapSize.x &&
				position.y >= 0 && position.y < mainMapSize.y);
	}

}
