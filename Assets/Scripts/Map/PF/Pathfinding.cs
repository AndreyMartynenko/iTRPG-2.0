using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding {

	Grid _grid;

	public Pathfinding(Grid grid) {
		_grid = grid;
	}

	public void FindPath(Point startPosition, Point targetPosition) {
		Node startNode = _grid.NodeFromWorldPosition(startPosition);
		Node targetNode = _grid.NodeFromWorldPosition(targetPosition);

		Heap<Node> openSet = new Heap<Node>(_grid.maxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in _grid.GetNeighbours(currentNode)) {
				if (!neighbour.isWalkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour)) {
						openSet.Add(neighbour);
					} else {
						openSet.UpdateItem(neighbour);
					}
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		_grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridIndex.x - nodeB.gridIndex.x);
		int dstY = Mathf.Abs(nodeA.gridIndex.y - nodeB.gridIndex.y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		else
			return 14 * dstX + 10 * (dstY - dstX);
	}

}
