using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Pathfinder))]
public class Player : MonoBehaviour
{
	public bool inAction = false; // If the player is currently executing an action
	public static Player Instance = null;
	public Pathfinder pathfinder;
	public int[] inventory = { 0, 0, 0, 0, 0, 0, 0, 0 };
	public int[] caravan = { 0, 0, 0, 0, 0, 0, 0 };
	public static int[] caravanGoal = { 2, 2, 2, 2, 2, 2, 2 };
	public float transactionTime = 0.5f;

	public bool steal = false;

	List<GoapAction> currPlan;

    // Start is called before the first frame update
    void Start()
    {
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		pathfinder = GetComponentInChildren<Pathfinder>();

		Simulate();
    }

	// Upon failing to meet the preconditions of an existing plan, throw it out and create a new one
	public void Replan()
	{
		steal = false;
		currPlan = new List<GoapAction>();
		Simulate();
	}

	// Start simulation
	public void Simulate()
	{
		currPlan = Planner.Instance.GetPlan();
		StartCoroutine(ExecutePlan());
	}

	IEnumerator ExecutePlan()
	{
		UIManager.Instance.UpdatePlanText(currPlan);
		while (currPlan.Count > 0)
		{
			GoapAction a = currPlan[currPlan.Count-1];
			yield return StartCoroutine(a.ExecuteAction());
			currPlan.RemoveAt(currPlan.Count-1);

			UIManager.Instance.UpdatePlanText(currPlan);
		}
	}

	public bool HasArrived()
	{
		return pathfinder.HasArrived();
	}

	public void GoToObject(GameObject g)
	{
		pathfinder.SetDestination(g.transform.position);
	}
	




}
