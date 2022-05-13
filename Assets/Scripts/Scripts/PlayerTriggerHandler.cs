using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerHandler : MonoBehaviour {

  Transform tr;
	// Use this for initialization
	void Start () {
    //tr = FindObjectOfType<SuperCharacterController>().transform;
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Enter");
    if( other.tag == "MovingObject" )
    {
      tr.parent = other.transform;
    }
  }

  private void OnTriggerStay(Collider other)
  {

  }

  private void OnTriggerExit(Collider other)
  {
    Debug.Log("Exit");
    if (other.tag == "MovingObject")
    {
      tr.parent = null;//PlayerMachine.platformVelocityVec = Vector3.zero;
    }
  }

}

