using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVelocity : MonoBehaviour {

  Vector3 prevPos;

  public Vector3 velocity;
	// Use this for initialization
	void Start () {
    prevPos = transform.position;
    
	}
	
	// Update is called once per frame
	void Update () {

    velocity = transform.position - prevPos;
    prevPos = transform.position;
    
	}
}
