using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public int moves;
	public int remainingMoves;

	MovementMap _movementMap;

	void Awake() {
		GameObject movementMapObject = Instantiate(Resources.Load("Prefabs/" + "MovementMap") as GameObject);
//		movementMapObject.transform.parent = transform;

		_movementMap = movementMapObject.GetComponent<MovementMap>();
		_movementMap.character = this;

		remainingMoves = moves;
	}
	
	void Update() {
	
	}

	public void OnCharacterSelected() {
		_movementMap.CreateMap(remainingMoves, new Point(transform.position.x, transform.position.z));
		_movementMap.CreateGrid();
	}

	public void OnDestinationReached() {
		_movementMap.CreateMap(remainingMoves, new Point(transform.position.x, transform.position.z));
		_movementMap.CreateGrid();
	}

}
