using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementMap))]
public class MovementMapOver : MonoBehaviour {
	
	MovementMap _movementMap;
	Point _currentGridIndex;
	Point _previousGridIndex;
	
	void Awake() {
		_movementMap = GetComponent<MovementMap>();
	}

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity)) {
			if (Input.GetMouseButtonDown(0)) {
				int x = Mathf.FloorToInt(hitInfo.point.x / _movementMap.tileScale);
				int y = Mathf.FloorToInt(hitInfo.point.z / _movementMap.tileScale);

				_currentGridIndex = new Point(x, y);

				if (_currentGridIndex == _previousGridIndex) {
					_movementMap.MoveTo(_currentGridIndex);
				} else {
					_movementMap.GeneratePathTo(_currentGridIndex);
				}

				_previousGridIndex = _currentGridIndex;
			}
		} else {
		}
	}
}
