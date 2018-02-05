using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementMap : TiledMap {

	public Character character;

	Vector3 _destination;
	bool _shouldMove;

	void Start() {
		
	}
	
	void Update() {
		if (_shouldMove) {
			if (Vector3.Distance(character.transform.position, _destination) < 0.05f) {
				UpdateDestination();
			}

			character.transform.position = Vector3.Lerp(character.transform.position, _destination, 10f * Time.deltaTime);
		}
	}

	public void MoveTo(Point position) {
		if (_grid.path != null) {
			_destination = new Vector3(_grid.path[0].worldPosition.x, 0, _grid.path[0].worldPosition.y);
			_shouldMove = true;
		}
	}

	List<Node> _previousPath;
	public void GeneratePathTo(Point position) {
		_grid.GeneratePathTo(position);

		// Clear previous path
		if (_previousPath != null) {
			foreach (Node node in _previousPath) {
				UpdateMap(node.gridIndex, TileType.Default);
			}
		}

		// Show new path
		if (_grid.path != null) {
			_previousPath = _grid.path;

			foreach (Node node in _grid.path) {
				UpdateMap(node.gridIndex, TileType.Selected);
			}
		}
	}

	private void UpdateDestination() {
		character.transform.position = _destination;

		if (_grid.path.Count == 0) {
			_grid.path = null;
			_shouldMove = false;
			character.OnDestinationReached();
			return;
		}

		character.remainingMoves--;
		_destination = new Vector3(_grid.path[0].worldPosition.x, 0, _grid.path[0].worldPosition.y);
		_grid.path.RemoveAt(0);
	}

}
