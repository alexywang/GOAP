using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSu : GoapAction
{
	public override Trader trader
	{
		get { return Trader.Su;  }
	}

	int[] pre = {0, 0, 4, 0, 0, 0, 0 };
	int[] post = {0, 0, -4, 0, 0, 0, 1};


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
