using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using Game.Model.Battle;

public class PathInfo
{
	public int id;
	public Node start;
	public Node goal;
	public HeroModel origin;
	public HeroModel target;
	public bool Pass = false;
}

public class PathResult
{
	public int id;
	public ArrayList path;
}

public class PathFinder1
{
	//public static PriorityQueue closedList, openList;
	public List<Node> closedList = new List<Node> ();
	//	public List<Node> openList = new List<Node> ();
	public Dictionary<int, bool> closeddict = new Dictionary<int, bool> ();
	public Dictionary<int, bool> openddict = new Dictionary<int, bool> ();

	public Queue<PathInfo> workQueue = new Queue<PathInfo> ();
	public Dictionary<int, ArrayList> workResult = new Dictionary<int, ArrayList> ();

	private int workId = 0;
	private IEnumerator worker;
	private int currentId = 0;
	
	Thread workThread;

	public PathFinder1 ()
	{
	}

	private static ArrayList CalculatePath (Node node)
	{
		ArrayList list = new ArrayList ();
		while (node != null) {
			list.Add (node);
			node = node.parent;
		}
		list.Reverse ();
		return list;
	}

	public void Update ()
	{
		if (worker == null && workQueue.Count <= 0) {
			return;
		}

		if (worker != null) {
			if (!worker.MoveNext ()) {
				worker = null;
			}

		} else {
			if (workQueue.Count > 0) {
				var info = workQueue.Dequeue ();
				if (info.Pass) {
					return;
				}
				worker = FindPathWorker (info);
			} else {
				return;
			}
		}
	}

	private static float HeuristicEstimateCost (Node curNode, Node goalNode)
	{
		Vector3 vecCost = curNode.position - goalNode.position;
		return vecCost.magnitude;
	}

	public bool IsDone (int id)
	{
		if (currentId > id) {
			if (!workResult.ContainsKey (id)) {
				workResult.Add (id, null);
			}
			return true;
		}
		
		return workResult.ContainsKey (id);
	}

	public void RemoveTask (int id)
	{
		if (id == currentId) {
			worker = null;
			return;
		}

		foreach (var a in workQueue) {
			if (a.id == id) {
				a.Pass = true;
				return;
			}
		}

	}

	public ArrayList GetResult (int id)
	{
		var result = workResult [id];
		workResult.Remove (id);
		return result;
	}


	public int FindPath (Vector3 start, Vector3 end, HeroModel origin, HeroModel target)
	{
		var startNode = new Node (GridManager.instance.GetGridCellCenter (GridManager.instance.GetGridIndexNoCheck (start)));
		var goalNode = new Node (GridManager.instance.GetGridCellCenter (GridManager.instance.GetGridIndexNoCheck (end)));
		PathInfo info = new PathInfo ();
		info.id = workId + 1;
		info.start = startNode;
		info.goal = goalNode;
		info.origin = origin;
		info.target = target;
		workQueue.Enqueue (info);
		workId += 1;
		return workId;
	}



	private IEnumerator FindPathWorker (PathInfo info)
	{
		Node start = info.start;
		Node goal = info.goal;
		HeroModel origin = info.origin;
		HeroModel target = info.target;
		

		//Start Finding the path
//		openList = new List<Node> ();
//		openList.Add (start);

		var openHeap = new Heap (50);
		openHeap.Push (start);

		start.nodeTotalCost = 0.0f;
		start.estimatedCost = HeuristicEstimateCost (start, goal);

		closedList.Clear ();
		closeddict.Clear ();
		openddict.Clear ();

		Node node = null;
		
		int tick = 0;
		bool find = false;

		while (openHeap.last != 0) {
			if (tick >= 30) {
				tick = 0;
				yield return null;
			}
			
			node = openHeap.Pop ();

			if (node.position == goal.position) {
				workResult.Add (info.id, CalculatePath (node));
				find = true;
				break;
			}
			
			ArrayList neighbours = new ArrayList ();
			GridManager.instance.GetNeighbours (node, neighbours, goal, origin, target);


			for (int i = 0; i < neighbours.Count; i++) {

				Node tmp = (Node)neighbours [i];
				Node neighbourNode = new Node ();
//				neighbourNode.position = tmp.position;
				neighbourNode = tmp;
//				neighbourNode.idx = tmp.idx;


				var nodeidx = GridManager.instance.GetGridIndexNoCheck (neighbourNode.position);
//				var nodeidx = neighbourNode.idx;

				if (!closeddict.ContainsKey (nodeidx)) {
					float cost = HeuristicEstimateCost (node, neighbourNode);	
					float totalCost = node.nodeTotalCost + cost;
					float neighbourNodeEstCost = HeuristicEstimateCost (neighbourNode, goal);					
					
					neighbourNode.nodeTotalCost = totalCost;
					neighbourNode.parent = node;
					neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;


					if (!openHeap.dict.ContainsKey (nodeidx)) {
						openHeap.Push (neighbourNode);	
					} else {
						// heap
						var idx = openHeap.dict [nodeidx];	
						var existnode = openHeap.list [idx - 1];
						if (existnode.nodeTotalCost > neighbourNode.nodeTotalCost) {
							openHeap.Remove (nodeidx, idx);
							openHeap.Push (neighbourNode);
						}
					}
				}
			}
			
            
			closedList.Add (node);
			closeddict.Add (GridManager.instance.GetGridIndexNoCheck (node.position), true);
//			closeddict.Add (node.idx, true);

//			openList.RemoveAt (0);
//			openList.Sort ();
			
			tick++;
		}

		if (!find) {
			workResult.Add (info.id, null);
		}
       
	}

	public static void OnDrawGizmos (ArrayList pathArray, Color color)
	{
		if (pathArray == null)
			return;

		if (pathArray.Count > 0) {
			int index = 1;
			foreach (Node node in pathArray) {
				if (index < pathArray.Count) {
					Node nextNode = (Node)pathArray [index];
					Debug.DrawLine (node.position, nextNode.position, color, 2);
					index++;
				}
			}
			;
		}
	}

	public static void OnDrawGizmos (List<Node> pathArray, Color color, int time = 1)
	{
		if (pathArray == null)
			return;

		if (pathArray.Count > 0) {
			int index = 1;
			foreach (Node node in pathArray) {
				if (index < pathArray.Count) {
					Node nextNode = (Node)pathArray [index];
					Debug.DrawLine (node.position, nextNode.position, color, time);
					index++;
				}
			}
			;
		}
	}
}
