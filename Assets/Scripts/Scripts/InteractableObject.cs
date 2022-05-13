using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

  public Outline outline;
  public float enableOutlineDistance;
  Transform tr;
  Transform player;
	// Use this for initialization
	void Start () { 
	}

  private void OnEnable()
  {
    //player = FindObjectOfType<CharacterControllerScript>().transform;
  }

  // Update is called once per frame
  void Update () {

   /* if (Vector3.Distance(player.position, this.transform.position) > enableOutlineDistance)
    {
      outline.enabled = false;
    }
    else
      outline.enabled = true;*/
	}
}
