using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeSu2 : GoapAction
{
	
	// Composite action for getting Sumac
	public override Trader trader
	{
		get
		{
			return Trader.Caravan;
		}
	}

	List<GoapAction> composite = new List<GoapAction>()
	{
		new WithdrawCa(), new WithdrawCa(), new WithdrawCa(), new WithdrawCa(), new GoToCaravan()
	};

	public override bool HasPrecondition(int[] inventory, int[] caravan)
	{
		int[] nextInv = GoapNode.CopyArray(inventory);
		int[] nextCrv = GoapNode.CopyArray(caravan);
		// Check if the preconditions for the next 3 moves are met
		foreach (GoapAction action in composite)
		{
			if (action.HasPrecondition(nextInv, nextCrv	))
			{
				action.ApplyPostcondition(nextInv, nextCrv);
			}
			else
			{
				return false;
			}

		}


		return true;
		
	}

	public override void ApplyPostcondition(int[] inventory, int[] caravan)
	{
		foreach(GoapAction action in composite)
		{
			action.ApplyPostcondition(inventory, caravan);
		}
	}

	public override IEnumerator ExecuteAction()
	{
		foreach(GoapAction action in composite)
		{
			yield return action.ExecuteAction();
		}
	}

}
