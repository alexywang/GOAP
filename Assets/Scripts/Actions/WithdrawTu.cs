﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawTu : GetFromCaravan
{
	int[] pre = { 1, 0, 0, 0, 0, 0, 0 };
	int[] post = { 1, 0, 0, 0, 0, 0, 0 };

	public override int[] preconditions
	{
		get { return pre; }
	}

	public override int[] postconditions
	{
		get { return post; }
	}
}