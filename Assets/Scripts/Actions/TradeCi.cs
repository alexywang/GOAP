using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCi : GoapAction
{
	public override Trader trader
	{
		get { return Trader.Ci;  }
	}

	int[] pre = {4, 0, 0, 0, 0, 0, 0 };
	int[] post = { -4, 0, 0, 1, 0, 0, 0};

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

}
