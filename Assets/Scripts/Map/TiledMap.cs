using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TiledMap : MonoBehaviour {
	
	public int tileResolution;
	public int tileScale;

	Point _size;
	Point _center;
	int _distance;
	Point _offset;

	float _tileSize;
	MainMap _mainMap;

	protected Grid _grid;

	protected enum TileType {
		Empty = 0,
		Default,
		Selected,
		Obstacle,
		Enemy
	}

	void Awake() {
		_mainMap = (GameObject.FindWithTag("MainMap") as GameObject).GetComponent<MainMap>();
		_tileSize = 1.0f / (GetComponent<MeshRenderer>().materials[0].mainTexture.width / tileResolution);
	}

	#region Public methods

	public void CreateMap(int gridWidth, int gridHeight) {
		CreateMap(new Point(gridWidth, gridHeight), Point.zero);
	}

	public void CreateMap(int distance, Point center) {
		if (distance < 0) {
			return;
		}

		_distance = distance;
		_center = center;

		int size = distance * 2 + 1;
		_size = new Point(size, size);

		int offset = distance * tileScale;
		_offset = new Point(_center.x - offset, _center.y - offset);

		CreateMap(_size, _offset);

		CreateObstacles();
	}

	public void CreateGrid() {
		if (_distance < 0) {
			return;
		}

		_grid = new Grid(_size, _mainMap.size, _offset, _mainMap.obstacles);
	}

	#endregion

	#region Protected methods

	protected Point TileIndexFromType(TileType type) {
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

	protected void UpdateMap(Point gridIndex, TileType tileType) {
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

	#endregion

	#region Private methods

	private void CreateMap(Point size, Point offset) {
		Mesh mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();

        int index = 0;
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				AddVertices(x, y, offset, tileScale, vertices);
                AddTriangles(ref index, triangles);
                AddNormals(normals);

				if (Utility.IsValidTileAt(_mainMap.size, new Point(x + offset.x, y + offset.y))) {
					AddUvs(TileIndexFromType(TileType.Default), uvs);
				} else {
					AddUvs(TileIndexFromType(TileType.Empty), uvs);
				}
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

	private void CreateObstacles() {
		foreach (Point obstacle in _mainMap.obstacles) {
			if (!Utility.IsValidTileAt(_mainMap.size, obstacle)) {
				continue;
			}

			Point index = Utility.GridIndexWithWorldPosition(_size, _center, obstacle);
			if (index.IsValid())
				UpdateMap(index, TileType.Obstacle);
		}
	}

	#endregion

	#region Map Helpers

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
		if (tileIndex == Point.zero) {
			uvs.Add(Vector2.zero);
			uvs.Add(Vector2.zero);
			uvs.Add(Vector2.zero);
			uvs.Add(Vector2.zero);
		} else {
			uvs.Add(new Vector2(tileIndex.x * _tileSize, tileIndex.y * _tileSize));
			uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, tileIndex.y * _tileSize));
			uvs.Add(new Vector2((tileIndex.x + 1) * _tileSize, (tileIndex.y + 1) * _tileSize));
			uvs.Add(new Vector2(tileIndex.x * _tileSize, (tileIndex.y + 1) * _tileSize));
		}
	}

	#endregion

}