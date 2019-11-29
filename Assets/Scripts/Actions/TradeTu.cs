using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTu : GoapAction
{
	public override Trader trader
	{
		get { return Trader.Tu;  }
	}

	int[] pre = new int[7];
	int[] post = { 2, 0, 0, 0, 0, 0, 0 };

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
