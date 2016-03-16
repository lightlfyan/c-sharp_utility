using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Heap
{

	public int last = 0;
	public Node[] list;
	public Dictionary<int, int> dict;



	public Heap (int number)
	{
		list = new Node[number];
		dict = new Dictionary<int, int> ();
	}

	public void Clear ()
	{
		last = 0;
		Array.Clear (list, 0, list.Length);
		dict.Clear ();
	}

	public void Push (Node node)
	{
		last += 1;
		if (last > list.Length) {
			var newlist = new Node[list.Length * 2];
			Array.Copy (list, newlist, list.Length);
			list = newlist;
		}


		SetValue (last, node);
		if (last == 1) {
			return;
		}
		siftup (last);
	}

	public Node Pop ()
	{

		if (last == 0) {
			return null;
		}
		
		if (last == 1) {
			var node = GetValue (last);
			dict.Remove (GridManager.instance.GetGridIndexNoCheck (node.position));
//			dict.Remove (node.idx);
			SetValue (last, null);
			last = 0;
			return node;
		}

		var head = GetValue (1);

		dict.Remove (GridManager.instance.GetGridIndexNoCheck (head.position));
//		dict.Remove (head.idx);


		SetValue (1, GetValue (last));
		SetValue (last, null);
		last -= 1;
		siftdown (1);
		return head;
	}


	public void Remove (int id, int idx)
	{
		dict.Remove (id);
		SetValue (idx, GetValue (last));
		last -= 1;
		siftdown (idx);
	}



	void siftup (int idx)
	{
		if (idx == 1) {
			return;
		}

		var curr = GetValue (idx);
		var parentidx = idx / 2;
		var parent = GetValue (parentidx);

		if (parent.estimatedCost > curr.estimatedCost) {
			swap (parentidx, idx);
			siftup (parentidx);
		}
	}


	void siftdown (int idx)
	{
		if (idx == last) {
			return;
		}
		var curr = GetValue (idx);
		var lidx = idx * 2;
		var ridx = idx * 2 + 1;
		var lhs = GetValue (lidx);
		var rhs = GetValue (ridx);

		if (lhs == null && rhs == null) {
			return;
		}

		if (rhs == null) {
			if (lhs.estimatedCost < curr.estimatedCost) {
				swap (lidx, idx);
			} 
			return;
		}
		 
		if (lhs == null) {
			if (rhs.estimatedCost < curr.estimatedCost) {
				swap (ridx, idx);
			} 
			return;	
		}



		if (lhs.estimatedCost < rhs.estimatedCost) {
			if (lhs.estimatedCost < curr.estimatedCost) {
				swap (lidx, idx);
				siftdown (lidx);
			} 
		} else {
			if (rhs.estimatedCost < curr.estimatedCost) {
				swap (ridx, idx);
				siftdown (ridx);
			} 
		}
	}


	void SetValue (int idx, Node node)
	{
		list [idx - 1] = node;

		if (node != null) {
			dict [GridManager.instance.GetGridIndexNoCheck (node.position)] = idx;
//			dict [node.idx] = idx;
		}
	}


	Node GetValue (int idx)
	{
		if (idx > last) {
			return null;
		}
		return list [idx - 1];
	}

	void swap (int i1, int i2)
	{
		var tmp = GetValue (i1);
		SetValue (i1, GetValue (i2));
		SetValue (i2, tmp);
	}

}
