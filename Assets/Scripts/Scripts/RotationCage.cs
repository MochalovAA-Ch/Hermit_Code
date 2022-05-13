using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCage : MonoBehaviour {

  public leverScript lever;
  public RotateScript rot;
	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update () {
    lever.leverOn = StopRotation;
	}


  void StopRotation()
  {
    rot.enabled = false;
  }

}
