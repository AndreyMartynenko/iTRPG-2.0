using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapOver : MonoBehaviour {
	
	TileMap _tileMap;
	Vector3 _currentTileCoord;
	
	public Transform selectionCube;
	
	void Awake() {
		_tileMap = GetComponent<TileMap>();
	}

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity)) {
			if (Input.GetMouseButtonDown(0)) {
				int x = Mathf.FloorToInt(hitInfo.point.x / _tileMap.tileScale);
				int z = Mathf.FloorToInt(hitInfo.point.z / _tileMap.tileScale);

				_currentTileCoord.x = x;
				_currentTileCoord.z = z;

				selectionCube.transform.position = _currentTileCoord * _tileMap.tileScale;

				_tileMap.Test(new Point(_currentTileCoord.x, _currentTileCoord.z));
			}
		} else {
		}
	}
}
