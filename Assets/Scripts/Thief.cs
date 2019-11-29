using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Thief : MonoBehaviour
{
	public GameObject caravan;
	public GameObject player;
	Pathfinder pathfinder;

	public float startTime;
	public float maxWanderTime = 5;
	public float wanderTime;
	public float wanderRadius = 3f;
	public Vector3 nextWander;
	public float wanderSpeed = 2f;
	public float stealSpeed = 6f;

	Renderer renderer;

	public Material stealMaterial;
	public Material wanderMaterial;

	int state = 0; // 0 wander, 1 player steal, 2 caravan steal

    // Start is called before the first frame update
    void Start()
    {
		pathfinder = GetComponentInChildren<Pathfinder>();
		Vector3 point;
		// Start at random point
		if(RandomSpawn(transform.position, out point))
		{
			transform.position = point;
		}

		float startTime = Time.time;

		nextWander = WanderPoint(transform.position, wanderRadius);
		pathfinder.SetDestination(nextWander);
		renderer = GetComponent<Renderer>();
    }



    // Update is called once per frame
    void Update()
    {

		// Every 5 seconds try to change state
		if (wanderTime > maxWanderTime && state == 0)
		{
			wanderTime = 0;
			float nextState = UnityEngine.Random.Range(0f, 1f);
			if(nextState < 0.33)
			{
				state = 1;
			}
			else if(nextState > 0.66)
			{
				state = 2;
			}
			else
			{
				state = 0;
			}

		}

		// Wander
		if(state == 0)
		{
			renderer.material = wanderMaterial;
			pathfinder.navAgent.speed = wanderSpeed;
			wanderTime += Time.deltaTime;
			if (Vector3.Distance(nextWander, transform.position) < 0.5f)
			{
				nextWander = WanderPoint(transform.position, wanderRadius);
				pathfinder.SetDestination(nextWander);
			}
		} 

		// Steal from player
		if(state == 1)
		{
			renderer.material = stealMaterial;

			pathfinder.navAgent.speed = stealSpeed;
			Vector3 playerPos = player.transform.position;
			pathfinder.SetDestination(playerPos);
			if (pathfinder.HasArrived())
			{
				// Steal from player
				StealFrom(Player.Instance.inventory);
				state = 0;
				wanderTime = 0;
				nextWander = WanderPoint(transform.position, wanderRadius);
				pathfinder.SetDestination(nextWander);
			}
			
		}

		if(state == 2)
		{
			renderer.material = stealMaterial;

			Vector3 caravanPos = caravan.transform.position;
			pathfinder.SetDestination(caravanPos);
			if (pathfinder.HasArrived())
			{
				// Steal from player
				StealFrom(Player.Instance.caravan);
				state = 0;
				wanderTime = 0;
				nextWander = WanderPoint(transform.position, wanderRadius);
				pathfinder.SetDestination(nextWander);
			}
		}

		// Update is called once per frame
		void Update()
		{

			if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
			{
				maxWanderTime /= 2;
			}
			if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore))
			{
				maxWanderTime *= 2;
			}
		}

	}

	public void StealFrom(int[] inv)
	{
		for(int i = 0; i < 2; i++)
		{
			List<int> stealFrom = AvailableSteal(inv);
			if (stealFrom.Count == 0)
			{
				break;
			}
			int steal = stealFrom[Random.Range(0, stealFrom.Count)];
			inv[steal]--;
			if(inv.Length > 7)
				inv[(int)Pre.Count] --;
			Player.Instance.steal = true;
		}
		UIManager.Instance.UpdateInventory(Player.Instance.inventory, Player.Instance.caravan);
	}


	// Get list of indices thief can steal from
	List<int> AvailableSteal(int[] inv)
	{
		List<int> hasItem = new List<int>();

		for (int i = 0; i < 7; i++)
		{
			if(inv[i] > 0)
			{
				hasItem.Add(i);
			}
		}

		return hasItem;
	}

	public bool RandomSpawn(Vector3 center, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 random = center;
			random.x += Random.Range(-35, 35);
			random.z += Random.Range(-35, 35);
			NavMeshHit hit;
			if (NavMesh.SamplePosition(random, out hit, 1.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}


	// Pick a point to wander to.
	public Vector3 WanderPoint(Vector3 center, float maxDistance)
	{
		Vector3 direction = Random.insideUnitSphere * maxDistance;
		direction += center;
		NavMeshHit hit;
		NavMesh.SamplePosition(direction, out hit, maxDistance, NavMesh.AllAreas);
		return hit.position;
	}

}
