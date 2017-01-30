using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject cube;
    public float offs;

	// Use this for initialization
	void Start () {
        for (int x = -5; x < 5; x++) {
            for (int y = 0; y < 10; y++) {
                var c = Instantiate(cube, new Vector3(x + offs * x, y + offs * y, 0) + transform.position, Quaternion.identity) as GameObject;
                c.transform.SetParent(transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
