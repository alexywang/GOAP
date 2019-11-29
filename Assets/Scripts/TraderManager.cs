using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Initialize trading posts
public class TraderManager : MonoBehaviour
{
	public static TraderManager Instance;
	public GameObject caravan;
	public GameObject[] traders;
	public int[] traderAssignments; // What item each trader is assigned to give out

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

		List<int> unassignedTraders = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7});
		traderAssignments = new int[traders.Length];
		// Assign every trader with an item
		for (int i = 0; i < 8; i++)
		{
			// Pick random unassigned trader
			int index = UnityEngine.Random.Range(0, unassignedTraders.Count);
			int assignTo = unassignedTraders[index];
			unassignedTraders.RemoveAt(index);
			traderAssignments[i] = assignTo;
			// Rename the trader in scene
			traders[assignTo].GetComponentInChildren<TextMesh>().text = Enum.GetName(typeof(Trader), (Trader)i);
		}

		Debug.Log(GetTrader(Trader.Tu));
    }

	// Return the trader that trades this item
	public GameObject GetTrader(Trader t)
	{
		if(t == Trader.Caravan)
		{
			return caravan;
		}
		else
		{
			int traderIndex = traderAssignments[(int)t];
			return traders[traderIndex];
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

}

public enum Trader
{
	Tu = 0,
	Sa = 1,
	Ca = 2,
	Ci = 3,
	Cl = 4,
	Pe = 5,
	Su = 6,
	Su2 = 7,
	Caravan = 8

}

