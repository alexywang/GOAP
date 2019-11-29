using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Class that can be used to move the object using pathfinding
[RequireComponent(typeof(NavMeshAgent))]
public class Pathfinder : MonoBehaviour
{
	[SerializeField] Vector3 currDestination;
	public NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
		navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
     
		if(Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
		{
			navAgent.speed *= 2;
			Player.Instance.transactionTime /= 2;
		}
		if(Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore))
		{
			navAgent.speed /= 2;
			Player.Instance.transactionTime *= 2;
		}
    }

	public void SetDestination(Vector3 newDest)
	{
		if (!navAgent)
		{
			navAgent = GetComponent<NavMeshAgent>();
		}
		currDestination = newDest;
		navAgent.SetDestination(currDestination);
	}

	public bool HasArrived()
	{
		return Vector3.Distance(gameObject.transform.position, currDestination) <= 0.22f;
	}
}
