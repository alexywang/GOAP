using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawSu : GetFromCaravan
{
	int[] pre = { 0, 0, 0, 0, 0, 0, 1 };
	int[] post = { 0, 0, 0, 0, 0, 0, 1 };

	public override int[] preconditions
	{
		get { return pre; }
	}

	public override int[] postconditions
	{
		get { return post; }
	}
}
