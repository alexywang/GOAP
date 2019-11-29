using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePe : GoapAction
{
	public override Trader trader
	{
		get { return Trader.Pe;  }
	}

	int[] pre = {2, 1, 0, 1, 0, 0, 0 };
	int[] post = {-2, -1, 0, -1, 0, 1, 0};

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
