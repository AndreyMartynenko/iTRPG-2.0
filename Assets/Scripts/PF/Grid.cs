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

	public Grid(int size, Point offset, List<Point> obstacles) {
		_size = new Point(size, size);
		_center = new Point((offset.x + size) - size / 2 - 1, (offset.y + size) - size / 2 - 1);
		_pathfinder = new Pathfinding(this);

		_nodes = new Node[size, size];
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				Point gridIndex = new Point(x, y);
				Point worldPosition = new Point(x + offset.x, y + offset.y);
				bool isWalkable = !obstacles.Contains(worldPosition) ? true : false;

				_nodes[x, y] = new Node(gridIndex, worldPosition, isWalkable);
			}
		}
	}

	public void Test(Point position) {
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
		Point index = Utility.GridIndexFromWorldPosition(_size, _center, worldPosition);

		return _nodes[index.x, index.y];
	}

}