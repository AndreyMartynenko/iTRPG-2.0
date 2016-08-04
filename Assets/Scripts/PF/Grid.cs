using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid {

	public List<Node> path;

	public int maxSize {
		get {
			return (int)(_size.x * _size.y);
		}
	}

	Vector2 _size;
	Vector2 _center;
	Node[,] _nodes;
	Pathfinding _pathfinding;

	public Grid(int size, Vector2 offset, List<Vector2> obstacles) {
		_size = new Vector2(size, size);
		_pathfinding = new Pathfinding(this);
		_center = new Vector2((offset.x + size) - size / 2 - 1, (offset.y + size) - size / 2 - 1);

		_nodes = new Node[size, size];
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				Vector2 worldPosition = new Vector2(x + offset.x, y + offset.y);
				bool walkable = !obstacles.Contains(worldPosition) ? true : false;

				_nodes[x, y] = new Node(walkable, worldPosition, x, y);
			}
		}
	}

	public void Test(Vector2 position) {
		_pathfinding.FindPath(_center, position);
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < _size.x && checkY >= 0 && checkY < _size.y) {
					neighbours.Add(_nodes[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPosition(Vector2 worldPosition) {
		Vector2 index = Utility.GridIndexFromWorldPosition(_size, _center, worldPosition);

		return _nodes[(int)index.x, (int)index.y];
	}

}