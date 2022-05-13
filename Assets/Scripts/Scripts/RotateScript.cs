using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {


  public Vector3 rotationVector3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    transform.Rotate(rotationVector3 * Time.deltaTime);
	}
}
