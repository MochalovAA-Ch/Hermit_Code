using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPiedestal : MonoBehaviour {

  public TutorialManager tutorialManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      if( GameSystem.playerKeys == 5 )
      {
        tutorialManager.EndTutorial();
      }
    }
  }
}
