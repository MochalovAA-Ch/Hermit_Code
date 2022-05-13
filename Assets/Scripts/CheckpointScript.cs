using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {

  bool isActive = false;
  public Outline outline;
  public GameObject fireParticle;
  public CheckPointsSounds checkPointsSounds;
  bool isPlayerWasNear = false;
  // Use this for initialization
	void Start ()
  {
	  	
	}
	
	// Update is called once per frame
	void Update ()
  {
    if( Vector3.Distance( transform.position, CharacterControllerScript.instance.transform.position ) < 5.0f )
    {
      isPlayerWasNear = true;
      if (CharacterControllerScript.instance.input.Info.interactInput )
      {
        checkPointsSounds.FireSound();
        GameController.instance.SetCheckpoint(transform);
        fireParticle.SetActive(true);
        GetComponent<OutlineController>().enabled = false;
        isActive = true;
        outline.enabled = false;
      }
      if( !isActive )
      {
        outline.enabled = true;
        GameUIController.instance.ShowInterractIcon();
        GameUIController.instance.SetInterractIcon();
      }
    }
    else
    {
      if( isPlayerWasNear )
      {
        GameUIController.instance.HideInterractIcon();
        isPlayerWasNear = false;
      }
    }
	}
}
