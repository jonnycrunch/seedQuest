using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapImage : MonoBehaviour {

    public GameObject Player;

    public int yScale;
    public int xScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3(-(Player.transform.position.x * xScale), -(Player.transform.position.z * yScale), 0);
	}
}
