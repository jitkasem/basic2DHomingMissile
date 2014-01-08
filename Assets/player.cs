using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	// Use this for initialization
	public GameObject[] leftCol;
	public GameObject[] rightCol;

	public void toggleDisableAllCollider()
	{		
		for(int iter = 0;iter < 3;iter++)
		{
			GameObject t = rightCol[iter];
			t.collider.enabled = false;
			GameObject g = leftCol[iter];
			g.collider.enabled = false;
		}
	}
	
	
	public void toggleLeftRight_collider(bool isRight)
	{
		if(isRight)
		{
			for(int iter = 0;iter < 3;iter++)
			{
				GameObject t = rightCol[iter];
				t.collider.enabled = true;
				GameObject g = leftCol[iter];
				g.collider.enabled = false;
			}
			
		}
		else
		{
			for(int iter = 0;iter < 3;iter++)
			{
				GameObject t = leftCol[iter];
				t.collider.enabled = true;
				GameObject g = rightCol[iter];
				g.collider.enabled = false;
			}
		}
		
	}

}
