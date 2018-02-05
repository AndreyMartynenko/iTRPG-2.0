using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {
	
	public Point gridIndex;
	public Point worldPosition;
	public bool isWalkable;

	public int gCost;
	public int hCost;
	public Node parent;

	int _heapIndex;
	
	public Node(Point gridIndex, Point worldPosition, bool isWalkable) {
		this.gridIndex = gridIndex;
		this.worldPosition = worldPosition;
		this.isWalkable = isWalkable;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	#region IHeapItem interface

	public int HeapIndex {
		get {
			return _heapIndex;
		}
		set {
			_heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}

		return -compare;
	}

	#endregion

}
