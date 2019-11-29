﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawPe : GetFromCaravan
{
	int[] pre = { 0, 1, 0, 1, 1, 0, 0 };
	int[] post = { 0, 1, 0, 1, 1, 0, 0 };

	public override int[] preconditions
	{
		get { return pre; }
	}

	public override int[] postconditions
	{
		get { return post; }
	}

	
}
