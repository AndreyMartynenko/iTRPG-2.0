using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	public int tileResolution;
	public int tileScale;

	Point _size;
	Point _center;
	float _tileSize;
	List<Point> _obstacles;

	Grid _grid;

	enum TileType {
		Empty = 0,
		Default,
		Selected,
		Obstacle,
		Enemy
	}

    void Update() {
		
    }

	List<Node> _previousGridPath;
	public void Test(Point position) {
		_grid.Test(position);

		// Clear previous path
		if (_previousGridPath != null) {
			foreach (Node node in _previousGridPath) {
				UpdateMesh(node.gridIndex, TileType.Default);
			}
		}

		// Show new path
		if (_grid.path != null) {
			_previousGridPath = _grid.path;

			foreach (Node node in _grid.path) {
				UpdateMesh(node.gridIndex, TileType.Selected);
			}
		}
	}

	public void CreateMesh(int gridWidth, int gridHeight) {
		CreateMesh(gridWidth, gridHeight, Point.zero);
	}

	public void CreateMesh(int distance, Point center) {
		_center = center;

		int gridSize = distance * 2 + 1;
		int offsetValue = distance * tileScale;
		Point offset = new Point(center.x - offsetValue, center.y - offsetValue);

		CreateMesh(gridSize, gridSize, offset);

		CreateObstacles();

		// Creating the grid for pathfinding
		_grid = new Grid(gridSize, offset, _obstacles);
	}

	private void CreateMesh(int gridWidth, int gridHeight, Point offset) {
		_size = new Point(gridWidth, gridHeight);
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
				AddUvs(TileIndexFromType(TileType.Default), uvs);
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

	private void AddVertices(int x, int y, Point offset, int tileScale, ICollection<Vector3> vertices) {
		vertices.Add(new Vector3((x * tileScale) + offset.x, 			 0, (y * tileScale) + offset.y));
		vertices.Add(new Vector3((x * tileScale) + tileScale + offset.x, 0, (y * tileScale) + offset.y));
		vertices.Add(new Vector3((x * tileScale) + tileScale + offset.x, 0, (y * tileScale) + tileScale + offset.y));
		vertices.Add(new Vector3((x * tileScale) + offset.x, 			 0, (y * tileScale) + tileScale + offset.y));
	}

	private void AddVertices(int x, int y, int tileScale, ICollection<Vector3> vertices) {
		AddVertices(x, y, Point.zero, tileScale, vertices);
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
		AddUvs(new Point(tileRow, tileColumn), uvs);
	}

	private void AddUvs(Point tileIndex, ICollection<Vector2> uvs) {
		uvs.Add(new Vector2(tileIndex.x * _tileSize, tileIndex.y * _tileSize));
		uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, tileIndex.y * _tileSize));
		uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, (tileIndex.y + 1) * _tileSize));
		uvs.Add(new Vector2(tileIndex.x * _tileSize, (tileIndex.y + 1) * _tileSize));
    }

	private void UpdateMesh(Point gridIndex, TileType tileType) {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = mesh.uv;

		Point tileIndex = TileIndexFromType(tileType);

		List<Vector2> updatedUvs = new List<Vector2>();
		AddUvs(tileIndex, updatedUvs);

		for (int idx = 0; idx < 4; idx++) {
			uvs[(_size.y * gridIndex.x + gridIndex.y) * 4 + idx] = updatedUvs[idx];
		}

		mesh.uv = uvs;
		mesh.RecalculateNormals();
	}

	private void CreateObstacles() {
		// Setup
		_obstacles = new List<Point>();

		_obstacles.Add(new Point(0, 3));
		_obstacles.Add(new Point(1, 3));
		_obstacles.Add(new Point(2, 3));
		_obstacles.Add(new Point(3, 3));

		_obstacles.Add(new Point(0, 2));
		_obstacles.Add(new Point(0, -2));

		_obstacles.Add(new Point(0, -3));
		_obstacles.Add(new Point(-1, -3));
		_obstacles.Add(new Point(-2, -3));
		_obstacles.Add(new Point(-3, -3));

		foreach (Point obstacle in _obstacles) {
			UpdateMesh(Utility.GridIndexFromWorldPosition(_size, _center, obstacle), TileType.Obstacle);
		}
	}

	private Point TileIndexFromType(TileType type) {
		switch (type) {
		case TileType.Empty:
			return new Point(0, 0);
		case TileType.Default:
			return new Point(1, 0);
		case TileType.Selected:
			return new Point(0, 1);
		case TileType.Obstacle:
			return new Point(1, 1);
		case TileType.Enemy:
			return new Point(1, 1);

		default:
			return new Point(0, 0);
		}
	}

}