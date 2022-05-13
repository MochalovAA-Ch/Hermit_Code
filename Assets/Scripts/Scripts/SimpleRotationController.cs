using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotationController : MonoBehaviour {

  public SimpleInteractRotation rotScript;
  bool isTriggered;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    if (isTriggered)
      return;

    if( other.tag == "Staff" )
    {
      if( StaffController.isStaffInHitt )
      {
        isTriggered = true;
        rotScript.StartRotationToAngle();
      }
    }
    if( other.tag == "Homy" )
    {
      isTriggered = true;
      rotScript.StartRotationToAngle();
    }
  }
}
