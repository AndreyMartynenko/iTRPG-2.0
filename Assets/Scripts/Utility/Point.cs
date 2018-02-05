//[System.Serializable]
public struct Point {
	
	public int x, y;

	public Point(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public Point(float x, float y) {
		this.x = (int)x;
		this.y = (int)y;
	}

	public static Point operator -(Point pointA, Point pointB) {
		if ((object)pointA == null || (object)pointB == null)
			return new Point(-1, -1);

		return new Point(pointA.x - pointB.x, pointA.y - pointB.y);
	}

	public static bool operator ==(Point pointA, Point pointB) {
		if ((object)pointA == null || (object)pointB == null)
			return false;

		return (pointA.x == pointB.x) && (pointA.y == pointB.y);
	}

	public static bool operator !=(Point pointA, Point pointB) {
		return !(pointA == pointB);
	}

	public override bool Equals(object obj) {
		if (obj == null)
			return false;

		// If parameter cannot be casted to Point return false
		Point point = (Point)obj;
		if ((object)point == null)
			return false;

		return (x == point.x) && (y == point.y);
	}

	public bool Equals(Point point) {
		if ((object)point == null)
			return false;

		return (x == point.x) && (y == point.y);
	}

	public override int GetHashCode() {
		return x ^ y;
	}

	public static Point zero {
		get {
			return new Point(0, 0);
		}
	}

	public bool IsValid() {
		return x != -1 && y != -1;
	}

}
