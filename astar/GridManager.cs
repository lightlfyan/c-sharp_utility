using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using SimpleJSON;
using Game.Model.Battle;

//Grid manager class handles all the grid properties
public class GridManager : MonoBehaviour
{
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	private static GridManager s_Instance = null;

	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static GridManager instance {
		get {
			return s_Instance;
		}
	}
	
	//	public GridManager (Vector3 _origin, int Row, int col, float cellsize)
	//	{
	//		this.origin = _origin;
	//		this.numOfRows = Row;
	//		this.numOfColumns = col;
	//		this.gridCellSize = cellsize;
	//
	//		this.Awake ();
	//
	//	}
	
	public void Dispose ()
	{
		s_Instance = null;
	}

	// Ensure that the instance is destroyed when the game is stopped in the editor.
	void OnApplicationQuit ()
	{
		s_Instance = null;
	}

	#region Fields

	public int numOfRows;
	public int numOfColumns;
	public float gridCellSize;
	public bool showGrid = true;
	public bool showObstacleBlocks = true;
	
	// row z
	// column x

	//	private Vector3 origin = new Vector3 ();
	private List<GameObject> obstacleList = new List<GameObject> ();

	public Node[,] nodes { get; set; }

	#endregion

	//Origin of the grid manager
	public Vector3 Origin {
		get { return transform.position; }
	}

	
	public List<Node> centerPath = new List<Node> ();
	public List<Node> moves = new List<Node> ();
	public List<Node> canmoves = new List<Node> ();

	//Initialise the grid manager
	void Awake ()
	{
		s_Instance = this;
	}

	/// <summary>
	/// Calculate which cells in the grids are mark as obstacles
	/// </summary>
	public void CalculateObstacles ()
	{
		//Initialise the nodes
		nodes = new Node[numOfColumns, numOfRows];
		int index = 0;
		for (int z = 0; z < numOfRows; z++) {
			for (int x = 0; x < numOfColumns; x++) {
				Vector3 cellPos = GetGridCellCenter (index);
				Node node = new Node (cellPos);

				var obstacle = DataManager.Instance.Battle.Map.GetTag (x, z);
				if (obstacle == 0) {
					node.MarkAsObstacle ();
				} else {
					canmoves.Add (node);
				}
				node.idx = index;
				node.height = DataManager.Instance.Battle.Map.GetHeight (x, z);


				nodes [x, z] = node;
				index++;
			}
		}
	}


	public IEnumerator CalculateObstaclesEnum ()
	{
		//Initialise the nodes
		nodes = new Node[numOfColumns, numOfRows];
		int index = 0;
		for (int z = 0; z < numOfRows; z++) {
			for (int x = 0; x < numOfColumns; x++) {
				Vector3 cellPos = GetGridCellCenter (index);
				Node node = new Node (cellPos);

				var obstacle = DataManager.Instance.Battle.Map.GetTag (x, z);
				if (obstacle == 0) {
					node.MarkAsObstacle ();
				} else {
					canmoves.Add (node);
				}
				node.idx = index;
				node.height = DataManager.Instance.Battle.Map.GetHeight (x, z);


				nodes [x, z] = node;
				index++;

				if (index % 100 == 0) {
					yield return null;
				}
			}
		}
	}

	public Node GetNode (int x, int y)
	{
		return nodes [x, y];
	}

	public Node GetNode (Vector3 pos)
	{
		if (!IsInBounds (pos)) {
			return null;
		}
		

		var idx = GetGridIndex (pos);
		int c = GetColumn (idx);
		int r = GetRow (idx);

		try {
			return nodes [c, r];
		} catch (Exception) {
			Debug.Log (c + " " + r);
			return null;
		}
	}


	public Node GetNodeNocheck (Vector3 pos)
	{
		var idx = GetGridIndexNoCheck (pos);
		int c = GetColumn (idx);
		int r = GetRow (idx);
		return nodes [c, r];
	}
	
	public Node GetNode (int idx)
	{
		int c = GetColumn (idx);
		int r = GetRow (idx);
		return nodes [c, r];
	}


	public void AddObstacle (Vector3 pos)
	{
		int indexCell = GetGridIndex (pos);
		int col = GetColumn (indexCell);
		int row = GetRow (indexCell);
		nodes [col, row].bObstacle = true;
	}
	
	// 3x3
	public void MoveObstacle (HeroModel actor, Vector3 pos)
	{
		DelMoveObstacle (actor);
		AddMoveObstacle (actor);
	}


	public void AddMoveObstacle (HeroModel actor)
	{
		int indexCell = GetGridIndex (actor.Position);
		int col = GetColumn (indexCell);
		int row = GetRow (indexCell);
		
		
		if (nodes [col, row].actor != null && nodes [col, row].actor.Tag != actor.Tag) {
			Debug.Log (nodes [col, row].actor.Tag + " " + actor.Tag);
		}

		nodes [col, row].actor = actor;
		nodes [col, row].MarkAsObstacle ();
		moves.Add (nodes [col, row]);
		return;
	}

	public void DelMoveObstacle (HeroModel actor)
	{
	
		int indexCell = GetGridIndex (actor.PositionSaved);
		int col = GetColumn (indexCell);
		int row = GetRow (indexCell);


		if (nodes [col, row].actor != null && nodes [col, row].actor.Tag != actor.Tag) {
			return;
		}

		nodes [col, row].actor = null;
		nodes [col, row].bObstacle = false;
		moves = moves.Where (o => o.position != nodes [col, row].position).ToList ();

		return;
	}
	
	
	// 5x5
	public void AddMoveObstacle2 (HeroModel actor, int i)
	{
		int indexCell = GetGridIndex (actor.Position);
		int col = GetColumn (indexCell);
		int row = GetRow (indexCell);

		for (int x = col - i; x <= col + i; x++) {
			for (int z = row - i; z <= row + i; z++) {
				if (x < 0 || z < 0 || x >= numOfColumns || z >= numOfRows) {
					continue;
				}
				nodes [x, z].actor = actor;
				nodes [x, z].MarkAsObstacle ();
				moves.Add (nodes [x, z]);
			}
		}
		
	}

	public void DelMoveObstacle2 (HeroModel actor, int i)
	{
		
		int indexCell = GetGridIndex (actor.Position);
		int col = GetColumn (indexCell);
		int row = GetRow (indexCell);

		
		for (int x = col - i; x <= col + i; x++) {
			for (int z = row - i; z <= row + i; z++) {
				if (x < 0 || z < 0 || x >= numOfColumns || z >= numOfRows) {
					continue;
				}
				nodes [x, z].actor = null;
				nodes [x, z].bObstacle = false;
				moves = moves.Where (o => o.position != nodes [x, z].position).ToList ();
			}
		}
	}
	
	public Node GetEmptyNode (HeroModel actor)
	{
		
		var idx = GetGridIndex (actor.Position);
		var col = GetColumn (idx);
		var row = GetRow (idx);
		
		if (nodes [col, row].actor == null) {
			return nodes [col, row];
		}
		
		float dis = 100f;
		Node result = null;
		
		for (int i = col-1; i <= col+1; i++) {
			for (int j= row-1; j <= row+1; j++) {
				var node = nodes [i, j];
				if (node.actor != null) {
					continue;
				}
				var dis2 = (node.position - actor.Position).sqrMagnitude;
				
				if (dis2 < dis) {
					dis = dis2;
					result = node;
				}
			}
		}
		
		return result;
	}


	public bool IsInSameGrid (Vector3 v1, Vector3 v2)
	{
		int id1 = GetGridIndex (v1);
		int id2 = GetGridIndex (v2);

		return id1 == id2;
	}

	public KeyValuePair<bool, List<HeroModel>> CanMoveTo (HeroModel origin, Vector3 next)
	{
    
		if (IsInSameGrid (origin.Position, next)) {
			return new KeyValuePair<bool, List<HeroModel>> (true, new List<HeroModel> ());
		}

		int idx1 = GetGridIndex (next);
		int row1 = GetRow (idx1);
		int col1 = GetColumn (idx1);

		List<HeroModel> actors = new List<HeroModel> ();
		List<HeroModel> empty = new List<HeroModel> ();

		var node = nodes [col1, row1];
		if (node.bObstacle && node.actor == null) {
			return new KeyValuePair<bool, List<HeroModel>> (false, empty);	
		}


		if (node.actor != null && node.actor.Tag != origin.Tag && node.actor.Camp == origin.Camp) {
			actors.Add (node.actor);
			return new KeyValuePair<bool, List<HeroModel>> (false, actors);
		}

		return new KeyValuePair<bool, List<HeroModel>> (true, empty);
	}

    

	/// <summary>
	/// Returns position of the grid cell in world coordinates
	/// </summary>
	public Vector3 GetGridCellCenter (int index)
	{
		Vector3 cellPosition = GetGridCellPosition (index);
		cellPosition.x += (gridCellSize / 2.0f);
		cellPosition.z += (gridCellSize / 2.0f);

		return cellPosition;
	}

	/// <summary>
	/// Returns position of the grid cell in a given index
	/// </summary>
	public Vector3 GetGridCellPosition (int index)
	{
		int row = GetRow (index);
		int col = GetColumn (index);
		float xPosInGrid = col * gridCellSize;
		float zPosInGrid = row * gridCellSize;

		return Origin + new Vector3 (xPosInGrid, 0.0f, zPosInGrid);
	}

	/// <summary>
	/// Get the grid cell index in the Astar grids with the position given
	/// </summary>
	private int GetGridIndex (Vector3 pos)
	{
		if (!IsInBounds (pos)) {
			return -1;
		}

		pos -= Origin;
		int col = Mathf.FloorToInt (pos.x / gridCellSize);
		int row = Mathf.FloorToInt (pos.z / gridCellSize);
		return (row * numOfColumns + col);
	}


	public int GetGridIndexNoCheck (Vector3 pos)
	{
		pos -= Origin;
		int col = Mathf.FloorToInt (pos.x / gridCellSize);
		int row = Mathf.FloorToInt (pos.z / gridCellSize);
		return (row * numOfColumns + col);
	}



	/// <summary>
	/// Get the row number of the grid cell in a given index
	/// </summary>
	public int GetRow (int index)
	{
		int row = index / numOfColumns;
		return row;
	}

	/// <summary>
	/// Get the column number of the grid cell in a given index
	/// </summary>
	public int GetColumn (int index)
	{
		int col = index % numOfColumns;
		return col;
	}

	/// <summary>
	/// Check whether the current position is inside the grid or not
	/// </summary>
	public bool IsInBounds (Vector3 pos)
	{
		float width = numOfColumns * gridCellSize;
		float height = numOfRows * gridCellSize;

		return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.x <= Origin.z + height && pos.z >= Origin.z);
	}


	/// <summary>
	/// Get the neighour nodes in 4 different directions
	/// </summary>
	public void GetNeighbours (Node node, ArrayList neighbors, Node goal, HeroModel origin, HeroModel target)
	{
		Vector3 neighborPos = node.position;
		int neighborIndex = GetGridIndex (neighborPos);

		int row = GetRow (neighborIndex);
		int column = GetColumn (neighborIndex);


		for (int x = column - 1; x <= column + 1; x++) {
			for (int z = row - 1; z <= row + 1; z++) {
				if (x == column && z == row) {
					continue;
				}
				
				if (x != column && (z != row)) {
					continue;
				}
				
				
				
				if (x < 0 || x >= numOfColumns || z < 0 || z >= numOfRows) {
					continue;
				}
				var n = nodes [x, z];


				if (n.bObstacle && n.actor == null) {
					continue;
				}

				var other = n.actor;
				if (other != null && other.Tag != origin.Tag && other.Camp == origin.Camp) {
					continue;
				}

				neighbors.Add (n);
			}
		}
	}
	

	/// <summary>
	/// Show Debug Grids and obstacles inside the editor
	/// </summary>
	void OnDrawGizmos ()
	{
		//Draw Grid
		if (showGrid) {
			DebugDrawGrid (Origin, numOfRows, numOfColumns, gridCellSize, new Color (1f, 0.5f, 0.5f, 0.2f));
		}
		

		//Grid Start Position
		Gizmos.DrawSphere (Origin, 0.5f);
		
		foreach (var n in moves) {
			Gizmos.DrawSphere (n.position, 0.5f);
		}


//		foreach (var n in canmoves) {
//			Gizmos.DrawCube (n.position, new Vector3 (0.1f, 0.1f, 0.1f));
//		}
		
		//Draw Obstacle obstruction
		if (showObstacleBlocks) {
			Vector3 cellSize = new Vector3 (gridCellSize, 1.0f, gridCellSize);

			if (obstacleList != null && obstacleList.Count > 0) {
				foreach (GameObject data in obstacleList) {
					Gizmos.DrawCube (GetGridCellCenter (GetGridIndex (data.transform.position)), cellSize / 2);
				}
			}
		}


//		foreach (var n in centerPath) {
//			Gizmos.DrawSphere (n.position, 0.5f);	
//		}
	}

	/// <summary>
	/// Draw the debug grid lines in the rows and columns order
	/// </summary>
	public void DebugDrawGrid (Vector3 origin, int numRows, int numCols, float cellSize, Color color)
	{
		float width = (numCols * cellSize);
		float height = (numRows * cellSize);

		// Draw the horizontal grid lines
		for (int i = 0; i < numRows + 1; i++) {
			Vector3 startPos = origin + i * cellSize * new Vector3 (0.0f, 0.0f, 1.0f);
			Vector3 endPos = startPos + width * new Vector3 (1.0f, 0.0f, 0.0f);
			Debug.DrawLine (startPos, endPos, color);
		}

		// Draw the vertial grid lines
		for (int i = 0; i < numCols + 1; i++) {
			Vector3 startPos = origin + i * cellSize * new Vector3 (1.0f, 0.0f, 0.0f);
			Vector3 endPos = startPos + height * new Vector3 (0.0f, 0.0f, 1.0f);
			Debug.DrawLine (startPos, endPos, color);
		}
		

	}
}
