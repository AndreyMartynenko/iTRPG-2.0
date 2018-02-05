using System;
using System.Collections;
using Procurios.Public;
using UnityEngine;

public class Level1
{
    public string Name;
    public int Width;
    public int Height;
    public Vector2[,] Grid;

	public static Level1 LoadLevel(Hashtable o, int tileResolution)
    {
        var level = new Level1 {
            Name = (string)o["name"],
            Width = Convert.ToInt32(o["width"]),
            Height = Convert.ToInt32(o["height"])
        };

        level.Grid = LoadGrid(o, level);

        return level;
    }

    private static Vector2[,] LoadGrid(IDictionary o, Level1 level)
    {
        var grid = new Vector2[level.Width, level.Height];
        var theGrid = (ArrayList)o["grid"];

//        Debug.Log("w/h " + level.Width + "," + level.Height);

        for (var y = 0; y < level.Height; y++) {
            var columns = (ArrayList)theGrid[y];
            for (var x = 0; x < level.Width; x++)
            {
                var index = (ArrayList)columns[x];
                var tx = Convert.ToInt32(index[0]);
                var ty = Convert.ToInt32(index[1]);
                grid[x, y] = new Vector2(tx, ty);
//                Debug.Log(grid[x, y].x + "," + grid[x, y].y);
            }
        }

        return grid;
    }
}

public class LevelTest : MonoBehaviour
{
	private TiledMap _cm;
    private Level1 _level;

    public void OnEnable()
    {
		_cm = GetComponent<TiledMap>();

//        var json = Resources.Load("level1", typeof(TextAsset)).ToString();
//		_level = Level1.LoadLevel((Hashtable)JSON.JsonDecode(json), _cm.tileResolution);

//        _cm.CreatePlane(_level.Width, _level.Height);
		_cm.CreateMap(5, new Point(0, 0));

//		for (var x = 0; x < _level.Width; x++) {
//			for (var y = 0; y < _level.Height; y++) {
//				_cm.UpdateGrid(new Vector2(x, y), _level.Grid[x, y]);
//			}
//		}
    }
}