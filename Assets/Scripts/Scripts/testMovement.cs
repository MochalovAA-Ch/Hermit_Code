using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement : MonoBehaviour {


  public Vector3 moveVector;
  public Vector3 moveDirection;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    moveDirection = Vector3.MoveTowards(moveDirection, moveVector, 10.0f * Time.deltaTime);
    transform.position += moveDirection * Time.deltaTime;
	}
}
