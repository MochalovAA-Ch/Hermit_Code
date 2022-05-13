using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomyLadderScript : HomyInteractibleObject {

  public SimpleInteractRotation ladderRot;

	// Use this for initialization
	void Start ()
  {
  }
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  public override void Interract()
  {
    if (hasInterracted)
      return;

    hasInterracted = true;
    ladderRot.StartRotationToAngle();
  }
}
