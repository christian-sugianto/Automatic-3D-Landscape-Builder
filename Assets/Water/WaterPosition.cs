using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPosition : MonoBehaviour {

    // terrain object
    public GameObject terrain;

    // default water position offset
    private const float OFFSET = -50.0f;

	// set water location to align up with terrain
	void Start () {

        // get xz coordinates of terrain, and y to be slightly 
        this.transform.position = new Vector3(terrain.transform.position.x, OFFSET,
                                      terrain.transform.position.z);
    }

}
