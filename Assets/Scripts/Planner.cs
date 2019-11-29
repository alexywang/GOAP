using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Planner : MonoBehaviour
{
	public static Planner Instance = null;

	List<GoapAction> actions;

	// Start is called before the first frame update
	void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		actions = new List<GoapAction>
		{
			new TradeTu(), new TradeCa(), new TradeCi(),
			new TradeCl(), new TradePe(), new TradeSa(),
			new TradeSu(), new TradeSu2(), new GoToCaravan(),
			new WithdrawCa(), new WithdrawCi(), new WithdrawCl(),
			new WithdrawPe(), new WithdrawSa(), new WithdrawSu(),
			new WithdrawTu(), new CompositeSu(), new CompositeSu2()
		};
	}

	// Update is called once per frame
	void Update()
	{

	}

	public List<GoapAction> GetPlan()
	{

		List<GoapAction> plan = new List<GoapAction>();
		GoapNode startNode = new GoapNode(Player.Instance.inventory, Player.Instance.caravan, null, null);
		GoapGraph graph = new GoapGraph(startNode, actions);
		GoapNode goalNode = graph.GoapSearch();

		GoapNode curr = goalNode;
		while (curr != null)
		{
			if (curr.action == null)
			{
				//Debug.Log(planSize);
				return plan;
			}
			//Debug.Log(curr.action.GetType());
			//Debug.Log(GoapGraph.PrintArray(curr.inventory));
			//Debug.Log(GoapGraph.PrintArray(curr.caravan));
			plan.Add(curr.action);
			curr = curr.parent;
		}

		return plan;
	}

	public void Replan()
	{

	}
}

public class Cost
{
	static float Tu(int[] inv)
	{
		return 0.5f;
	}

	static float Sa(int[] inv)
	{
		return 1 + Math.Max(2 - inv[0], 0) * Tu(inv);
	}

	static float Ca(int[] inv)
	{
		return 1 + Math.Max(2 - inv[1], 0) * Sa(inv);
	}

	static float Ci(int[] inv)
	{
		return 1 + Math.Max(4 - inv[0], 0) * Tu(inv);
	}

	static float Cl(int[] inv)
	{
		return 1 + Math.Max(1 - inv[2], 0) * Ca(inv) + Math.Max(1 - inv[0], 0) * Tu(inv);
	}

	static float Pe(int[] inv)
	{
		return 1 + Math.Max(2 - inv[0], 0) * Tu(inv) + Math.Max(1 - inv[1], 0) * Sa(inv) + Math.Max(1 - inv[3], 0) * Ci(inv);
	}

	static float Su(int[] inv)
	{
		float trade1 = 1 + Math.Max(4 - inv[2], 0) * Ca(inv);
		float trade2 = 1 + Math.Max(1 - inv[1], 0) * Sa(inv) + Math.Max(1 - inv[3], 0) * Ci(inv) + Math.Max(1 - inv[4], 0) * Cl(inv);
		return Math.Min(trade1, trade2);
	}

	public static float Steps(int item, int[] inv)
	{
		int[] test = new int[7];

		switch (item)
		{
			case 0:
				return Tu(inv);
			case 1:
				return Sa(inv);
			case 2:
				return Ca(inv);
			case 3:
				return Ci(inv);
			case 4:
				return Cl(inv);
			case 5:
				return Pe(inv);
			case 6:
				return Su(inv);
			default:
				return 0;
		}
	}

	public static int[] tradesNeeded = { 1, 2, 5, 3, 7, 6, 12 };

}

public class Heuristic
{
	public static float[] itemValue = { 0.5f, 2, 5, 3, 5.5f, 7, 20.5f };
}

// Node in Goap Search Graph
public class GoapNode
{
	public GoapAction action;
	public int[] inventory;
	public int[] caravan;
	int[] caravanGoal = Player.caravanGoal;
	public float g;
	public float h;
	public float f
	{
		get { return g + h; }
	}

	public List<GoapNode> adjacent = new List<GoapNode>();
	public GoapNode parent;

	public GoapNode(int[] inventory, int[] caravan, GoapAction action, GoapNode parent)
	{
		this.inventory = inventory;
		this.caravan = caravan;
		this.action = action;
		this.parent = parent;
		g = parent == null ? 0 : parent.g++;
		h = hScore();
	}

	// Adds an edge to a new node if action preconditions are met
	public GoapNode AddNextNode(GoapAction action)
	{
		if (action.HasPrecondition(inventory, caravan))
		{
			int[] nextInv = CopyArray(inventory);
			int[] nextCrv = CopyArray(caravan);
			action.ApplyPostcondition(nextInv, nextCrv);

			GoapNode nextNode = new GoapNode(nextInv, nextCrv, action, this);
			adjacent.Add(nextNode);
			return nextNode;
		}
		else
		{
			return null;
		}
	}

	// Distance from start
	public float gScore()
	{
		//float tradesMade = 0;
		//for (int i = 0; i < caravan.Length; i++)
		//{
		//	tradesMade += Cost.tradesNeeded[i] * (inventory[i] + caravan[i]);
		//}

		//return tradesMade;

		float distance = 0;
		for (int i = 0; i < caravan.Length; i++)
		{
			distance += inventory[i] + caravan[i];
		}
		return 0;
	}

	// Trades required to get to goal
	public float hScore()
	{
		//float tradesRequired = 0;
		//int[] requiredItems = new int[7];
		//for (int i = 0; i < requiredItems.Length; i++)
		//{
		//	requiredItems[i] = Math.Max(2 - caravan[i], 0);
		//}
		//// Trades required to get to your goal with an empty inventory
		//for (int i = 0; i < requiredItems.Length; i++)
		//{
		//	tradesRequired += requiredItems[i] * Cost.Steps(i, inventory);
		//}

		//return tradesRequired;

		float distance = 0;
		for (int i = 0; i < caravan.Length; i++)
		{
			distance += Math.Max(2 - inventory[i] - caravan[i], 0) * Heuristic.itemValue[i];
		}
		return distance;
	}

	// If this node is at the goal state or not
	public bool IsGoal(int[] goal)
	{
		for (int i = 0; i < goal.Length; i++)
		{
			if (caravan[i] < goal[i])
			{
				return false;
			}
		}
		return true;
	}

	// Needed since ApplyPostcondition works in-place.
	public static int[] CopyArray(int[] array)
	{
		int[] copy = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			copy[i] = array[i];
		}
		return copy;
	}
}




// Enitre Goap Graph
public class GoapGraph
{
	public GoapNode startNode;
	public int[] caravanGoal = Player.caravanGoal;
	List<GoapAction> actions;
	public int nodeCount = 0;

	public GoapGraph(GoapNode startNode, List<GoapAction> actions)
	{
		this.startNode = startNode;
		this.actions = actions;
	}


	// Generates the next set of actions for a given node
	public void GenerateEdges(GoapNode node)
	{
		foreach (GoapAction action in actions)
		{
			GoapNode added = node.AddNextNode(action);
		}
	}

	public GoapNode GoapSearch()
	{
		List<GoapNode> candidates = new List<GoapNode>();
		List<GoapNode> visited = new List<GoapNode>();
		GoapNode curr = startNode;
		visited.Add(startNode);
		GenerateEdges(startNode);
		candidates.AddRange(curr.adjacent);
		int iterations = 0;
		while (candidates.Count != 0)
		{
			int lowest = GetLowestIndex(candidates);
			curr = candidates[lowest];
			candidates.RemoveAt(lowest);
			visited.Add(curr);
			GenerateEdges(curr);
			candidates.AddRange(curr.adjacent);
			//Debug.Log(curr.action.GetType() + ": " + curr.f + "=" + curr.g + "+" + curr.h);
			if (curr.parent != null)
			{
				//Debug.Log("Prev Inv: " + PrintArray(curr.parent.inventory));
				//Debug.Log("Prev Car: " + PrintArray(curr.parent.caravan));
			}


			//Debug.Log("Inv     : " + PrintArray(curr.inventory));
			//Debug.Log("Car     : " + PrintArray(curr.caravan));
			if (curr.IsGoal(Player.caravanGoal))
			{
				Debug.Log("Found goal" + PrintArray(curr.caravan));
				return curr;
			}
			iterations++;

			if (iterations > 10000)
			{
				Debug.Log("Max iterations.");
				break;
			}
		}

		return null;
	}

	// Get index of lowest score GoapNode
	int GetLowestIndex(List<GoapNode> set)
	{
		GoapNode lowest = set[0];
		int index = 0;
		List<int> ties = new List<int>();
		for (int i = 0; i < set.Count; i++)
		{
			if (set[i].f < lowest.f)
			{
				lowest = set[i];
				index = i;
			}
			else if (set[i].f == lowest.f)
			{
				if (set[i].h < lowest.h)
				{
					lowest = set[i];
					index = i;
				}
				else if (set[i].h == lowest.h)
				{
					ties.Add(i);
				}
			}
			else
			{
				continue;
			}
		}
		if (ties.Count > 1)
		{
			int random = UnityEngine.Random.Range(0, ties.Count);
			index = ties[random];
		}

		return index;
	}




	public static string PrintArray(int[] array)
	{
		string s = "[";
		foreach (int i in array)
		{
			s += i + ", ";
		}
		s += "]";
		return s;
	}

}