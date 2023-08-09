using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {


	private static bool exists = false;

	// Use this for initialization
	void Start () 
	{
		if(exists) Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		exists = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
