using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFromCaravan : GoapAction
{
	public override Trader trader
	{
		get
		{
			return Trader.Caravan;
		}
	}


	int[] pre;
	int[] post;
 

	public override bool HasPrecondition(int[] inventory, int[] caravan)
	{
		// Check if the items are in the caravan
		for(int i = 0; i < caravan.Length; i++)
		{
			if(caravan[i] < preconditions[i])
			{
				return false;
			}
		}

		// Check if player has enough inventory space
		if(inventory[(int)Pre.Count] > 3){
			return false;
		}

		return true;
	}

	public override void ApplyPostcondition(int[] inventory, int[] caravan)
	{
		// Remove item from the caravan and transfer it to the player
		for(int i = 0; i < caravan.Length; i++)
		{
			caravan[i] -= postconditions[i];
			inventory[i] += postconditions[i];
		}

		inventory[(int)Pre.Count]++;
	}
}
