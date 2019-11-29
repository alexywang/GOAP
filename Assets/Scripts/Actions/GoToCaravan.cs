using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GoToCaravan : GoapAction
{
	public override Trader trader
	{
		get
		{
			return Trader.Caravan;
		}
	}

	int[] pre = { 0, 0, 0, 0, 0, 0, 0 };
	int[] post = { 0, 0, 0, 0, 0, 0, 0 };

	public override int[] preconditions
	{
		get
		{
			return pre;
		}
	}

	public override int[] postconditions
	{
		get
		{
			return post;
		}
	}

	// Have at least one item
	public override bool HasPrecondition(int[] inventory, int[] caravan)
	{
		return inventory[(int)Pre.Count] >= 0;
	}

	public override void ApplyPostcondition(int[] inventory, int[] caravan)
	{
		inventory[inventory.Length - 1] = 0;
		for(int i = 0; i < caravan.Length; i++)
		{
			caravan[i] += inventory[i];
			inventory[i] = 0;
		}

		
	}


}
