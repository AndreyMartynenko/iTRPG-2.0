using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
public class CharacterOver : MonoBehaviour {

	public GameObject characterObject;

	Character _character;

	void Awake() {
		_character = GetComponent<Character>();
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo)) {
				if (hitInfo.collider.gameObject == characterObject) {
					_character.OnCharacterSelected();
				}
			}
		}
	}

}
