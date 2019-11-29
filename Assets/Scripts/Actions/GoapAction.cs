using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GoapAction
{
	public virtual int[] preconditions
	{
		get;
	}
	public virtual int[] postconditions
	{
		get;
	}
	const int MAX_INV = 4;

	public GameObject tradingPost; // Where the player should navigate to before peforming action
	public abstract Trader trader { get; }

	public GoapAction()
	{
		tradingPost = TraderManager.Instance.GetTrader(trader);
	}


	// NOTE: Must be overriden for Caravan
	public virtual bool HasPrecondition(int[] inventory, int[] caravan)
	{
		// Check for minimum requirements
		for (int i = 0; i < preconditions.Length; i++)
		{
			if (inventory[i] < preconditions[i])
			{
				return false;
			}
		}

		// Check for maximum inventory
		int deltaInv = 0;
		for(int i = 0; i < postconditions.Length; i++)
		{
			deltaInv += postconditions[i];
		}

		if (inventory[(int)Pre.Count] + deltaInv > MAX_INV)
		{
			return false;
		}
		return true;
	}

	// Change state of the player after the trade 
	public virtual void ApplyPostcondition(int[] inventory, int[] caravan)
	{
		int deltaCount = 0;
		// Add and remove Inventory Items
		for (int i = 0; i < postconditions.Length; i++)
		{
			inventory[i] += postconditions[i];
			deltaCount += postconditions[i];
		}

		inventory[(int)Pre.Count] += deltaCount;
		
	}

	// Move to trader then make trade, if fails return false.
	public virtual IEnumerator ExecuteAction()
	{
		Player.Instance.inAction = true;
		Debug.Log("--------------Executing Action " + this.GetType()+"-------------");
		Debug.Log("Pre" + GoapGraph.PrintArray(Player.Instance.inventory));
		Debug.Log("Pre" + GoapGraph.PrintArray(Player.Instance.caravan));

		if (!tradingPost)
		{
			tradingPost = TraderManager.Instance.GetTrader(trader);
		}

		// TODO: How to force a replan if they suddenly don't meet the preconditions?
		if (HasPrecondition(Player.Instance.inventory, Player.Instance.caravan) && !Player.Instance.steal)
		{
			Player.Instance.GoToObject(tradingPost);
		}
		else
		{
			//Debug.Log("I don't meet the preconditions (1)");
			Debug.Log("Replan1" + this.GetType());
			Debug.Log("Replan1" + GoapGraph.PrintArray(Player.Instance.inventory));
			Debug.Log("Replan1" + GoapGraph.PrintArray(Player.Instance.caravan));
			Player.Instance.StopAllCoroutines();
			Player.Instance.Replan();
			Player.Instance.inAction = false;
			yield break;
		}

		//Wait for player to arrive at trader
		while (!Player.Instance.HasArrived())
		{
			yield return new WaitForSeconds(0.2f);
		}

		// Wait to 0.5s to complete trade
		yield return new WaitForSeconds(Player.Instance.transactionTime);

		if (HasPrecondition(Player.Instance.inventory, Player.Instance.caravan) && !Player.Instance.steal)
		{

			Debug.Log("Applying Postconditions for " + this.GetType());
			ApplyPostcondition(Player.Instance.inventory, Player.Instance.caravan);
			UIManager.Instance.UpdateInventory(Player.Instance.inventory, Player.Instance.caravan);
			Debug.Log("Post"+GoapGraph.PrintArray(Player.Instance.inventory));
			Debug.Log("Post"+GoapGraph.PrintArray(Player.Instance.caravan));
		}
		else
		{
			Debug.Log("Replan2" + this.GetType());
			Debug.Log("Replan2" + GoapGraph.PrintArray(Player.Instance.inventory));
			Debug.Log("Replan2" + GoapGraph.PrintArray(Player.Instance.caravan));
			//Debug.Log("I don't meet the preconditions (2).");
			Player.Instance.StopAllCoroutines();
			Player.Instance.Replan();
			Player.Instance.inAction = false;
			yield break;
		}

		Player.Instance.inAction = false;

	}


}

public enum Pre
{
	Tu = 0,
	Sa = 1,
	Ca = 2, 
	Ci = 3, 
	Cl = 4, 
	Pe = 5, 
	Su = 6,
	Count = 7,
	Caravan = 8

}