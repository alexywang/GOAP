using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text inventoryText;
	public Text caravanText;
	public Text planText;
	public static UIManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
    }




	public string ArrayToString(int[] array)
	{
		string s = "";
		for(int i = 0; i < 7; i++)
		{
			s += array[i] + ", ";
		}
		return s;
	}

	public void UpdateInventory(int[] inventory, int[] caravan)
	{
		inventoryText.text = "Inv: " + ArrayToString(inventory);
		caravanText.text = "Car: " + ArrayToString(caravan);
	}

	public void UpdatePlanText(List<GoapAction> plan)
	{
		string p = "";
		for (int i = plan.Count - 1; i >= 0; i--)
		{
			p += plan[i].GetType() + "\n";
		}
		planText.text = p;
	}

    // Update is called once per frame
    void Update()
    {
       
    }
}
