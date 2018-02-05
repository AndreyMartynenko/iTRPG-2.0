using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMap : MonoBehaviour {

	public List<Point> obstacles;

	private Point _size;
	public Point size {
		get {
			return _size;
		}
	}

	void Awake() {
		_size = new Point(16, 16);
		CreateObstacles();
	}
	
	void Update() {
	
	}

	private void CreateObstacles() {
		obstacles = new List<Point>();

		obstacles.Add(new Point(2, 2));
		obstacles.Add(new Point(3, 2));
		obstacles.Add(new Point(0, 5));
		obstacles.Add(new Point(1, 5));
		obstacles.Add(new Point(2, 5));
		obstacles.Add(new Point(5, 5));
		obstacles.Add(new Point(5, 6));

		obstacles.Add(new Point(10, 2));
		obstacles.Add(new Point(11, 2));
		obstacles.Add(new Point(8, 5));
		obstacles.Add(new Point(9, 5));
		obstacles.Add(new Point(10, 5));
		obstacles.Add(new Point(13, 5));
		obstacles.Add(new Point(13, 6));

		obstacles.Add(new Point(2, 10));
		obstacles.Add(new Point(3, 10));
		obstacles.Add(new Point(0, 13));
		obstacles.Add(new Point(1, 13));
		obstacles.Add(new Point(2, 13));
		obstacles.Add(new Point(5, 13));
		obstacles.Add(new Point(5, 14));

		obstacles.Add(new Point(10, 10));
		obstacles.Add(new Point(11, 10));
		obstacles.Add(new Point(8, 13));
		obstacles.Add(new Point(9, 13));
		obstacles.Add(new Point(10, 13));
		obstacles.Add(new Point(13, 13));
		obstacles.Add(new Point(13, 14));
	}

}
