using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSu2 : GoapAction
{
	public override Trader trader
	{
		get { return Trader.Su2;  }
	}

	int[] pre = {0, 1, 0, 1, 1, 0, 0};
	int[] post = {0, -1, 0, -1, -1, 0, 1};

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
