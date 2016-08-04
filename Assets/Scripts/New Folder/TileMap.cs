using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	public int tileResolution;
	public int tileScale;
	public Vector2 defaultTile;
	public bool shouldDrawRandomTiles;

	Vector2 _size;
	Vector2 _center;
	float _tileSize;
	List<Vector2> _obstacles;

	Grid _grid;

    void Update() {
        if (shouldDrawRandomTiles) {
            DrawRandomTiles();
        }
    }

	public void UpdateGrid(Vector2 gridIndex, Vector2 tileIndex) {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = mesh.uv;

		List<Vector2> updatedUvs = new List<Vector2>();
		AddUvs(tileIndex, updatedUvs);

		for (int idx = 0; idx < 4; idx++) {
			uvs[(int)(_size.y * gridIndex.x + gridIndex.y) * 4 + idx] = updatedUvs[idx];
		}

		mesh.uv = uvs;
		mesh.RecalculateNormals();
	}

	List<Node> _previousGridPath;
	public void Test(Vector2 position) {
		_grid.Test(position);

		// Clear previous path
		if (_previousGridPath != null) {
			foreach (Node node in _previousGridPath) {
				UpdateGrid(new Vector2(node.gridX, node.gridY), defaultTile);
			}
		}

		// Show new path
		if (_grid.path != null) {
			_previousGridPath = _grid.path;

			foreach (Node node in _grid.path) {
				UpdateGrid(new Vector2(node.gridX, node.gridY), new Vector2(0, 1));
			}
		}
	}

	public void CreatePlane(int gridWidth, int gridHeight) {
		CreatePlane(gridWidth, gridHeight, Vector2.zero);
	}

	public void CreatePlane(int distance, Vector2 center) {
		_center = center;

		int gridSize = distance * 2 + 1;
		int offset = distance * tileScale;

		Vector2 off = new Vector2(center.x - offset, center.y - offset);

		CreatePlane(gridSize, gridSize, off);

		CreateObstacles();

		// Creating the grid for pathfinding
		_grid = new Grid(gridSize, off, _obstacles);
	}

	private void CreatePlane(int gridWidth, int gridHeight, Vector2 offset) {
		_size = new Vector2(gridWidth, gridHeight);
		_tileSize = 1.0f / (GetComponent<MeshRenderer>().materials[0].mainTexture.width / tileResolution);

		Mesh mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();

        int index = 0;
		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				AddVertices(x, y, offset, tileScale, vertices);
                AddTriangles(ref index, triangles);
                AddNormals(normals);
				AddUvs(defaultTile, uvs);
            }
        }

        mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		MeshCollider meshCollider = GetComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
    }

	private void AddVertices(int x, int y, Vector2 offset, int tileScale, ICollection<Vector3> vertices) {
		vertices.Add(new Vector3((x * tileScale) + offset.x, 			 0, (y * tileScale) + offset.y));
		vertices.Add(new Vector3((x * tileScale) + tileScale + offset.x, 0, (y * tileScale) + offset.y));
		vertices.Add(new Vector3((x * tileScale) + tileScale + offset.x, 0, (y * tileScale) + tileScale + offset.y));
		vertices.Add(new Vector3((x * tileScale) + offset.x, 			 0, (y * tileScale) + tileScale + offset.y));
	}

	private void AddVertices(int x, int y, int tileScale, ICollection<Vector3> vertices) {
		AddVertices(x, y, Vector2.zero, tileScale, vertices);
	}

	private void AddTriangles(ref int index, ICollection<int> triangles) {
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index);
        triangles.Add(index);
        triangles.Add(index + 3);
        triangles.Add(index + 2);

        index += 4;
    }

    private void AddNormals(ICollection<Vector3> normals) {
		normals.Add(Vector3.up);
		normals.Add(Vector3.up);
		normals.Add(Vector3.up);
		normals.Add(Vector3.up);
    }

	private void AddUvs(int tileRow, int tileColumn, ICollection<Vector2> uvs) {
		AddUvs(new Vector2(tileRow, tileColumn), uvs);
	}

	private void AddUvs(Vector2 tileIndex, ICollection<Vector2> uvs) {
		uvs.Add(new Vector2(tileIndex.x * _tileSize, tileIndex.y * _tileSize));
		uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, tileIndex.y * _tileSize));
		uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, (tileIndex.y + 1) * _tileSize));
		uvs.Add(new Vector2(tileIndex.x * _tileSize, (tileIndex.y + 1) * _tileSize));
    }

	private void DrawRandomTiles() {
		var tileColumn = Random.Range(0, tileResolution);
		var tileRow = Random.Range(0, tileResolution);

		var x = Random.Range(0, _size.x);
		var y = Random.Range(0, _size.y);

		UpdateGrid(new Vector2(x, y), new Vector2(tileColumn, tileRow));
	}

	private void CreateObstacles() {
		// Setup
		_obstacles = new List<Vector2>();

		_obstacles.Add(new Vector2(0, 3));
		_obstacles.Add(new Vector2(1, 3));
		_obstacles.Add(new Vector2(2, 3));
		_obstacles.Add(new Vector2(3, 3));

		_obstacles.Add(new Vector2(0, 2));
		_obstacles.Add(new Vector2(0, -2));

		_obstacles.Add(new Vector2(0, -3));
		_obstacles.Add(new Vector2(-1, -3));
		_obstacles.Add(new Vector2(-2, -3));
		_obstacles.Add(new Vector2(-3, -3));

		foreach (Vector2 obstacle in _obstacles) {
			UpdateGrid(Utility.GridIndexFromWorldPosition(_size, _center, obstacle), new Vector2(1, 1));
		}
	}

}