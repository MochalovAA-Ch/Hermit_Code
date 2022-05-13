using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationRbPlatform : MonoBehaviour {

  Rigidbody rb;
  public float angle;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
  {
    rb.MoveRotation(rb.rotation * Quaternion.Euler(0.0f, angle * Time.fixedDeltaTime, 0.0f));	
	}
}
