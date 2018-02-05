using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid {

	public List<Node> path;

	public int maxSize {
		get {
			return _size.x * _size.y;
		}
	}

	Point _size;
	Point _center;
	Node[,] _nodes;
	Pathfinding _pathfinder;

	public Grid(Point size, Point mainMapSize, Point offset, List<Point> obstacles) {
		_size = size;
		_center = new Point((offset.x + size.x) - size.x / 2 - 1, (offset.y + size.y) - size.y / 2 - 1);
		_pathfinder = new Pathfinding(this);

		_nodes = new Node[size.x, size.y];
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				Point gridIndex = new Point(x, y);
				Point worldPosition = new Point(x + offset.x, y + offset.y);
				bool isWalkable = !obstacles.Contains(worldPosition) && Utility.IsValidTileAt(mainMapSize, worldPosition) ? true : false;

				_nodes[x, y] = new Node(gridIndex, worldPosition, isWalkable);
			}
		}
	}

	public void GeneratePathTo(Point position) {
		_pathfinder.FindPath(_center, position);
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridIndex.x + x;
				int checkY = node.gridIndex.y + y;

				if (checkX >= 0 && checkX < _size.x && checkY >= 0 && checkY < _size.y) {
					neighbours.Add(_nodes[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPosition(Point worldPosition) {
		Point index = Utility.GridIndexWithWorldPosition(_size, _center, worldPosition);

		return _nodes[index.x, index.y];
	}

}