using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour {

  public Outline outline;
  public float Distance;
  Transform player;

	// Use this for initialization
	void Start ()
  {
    //player = SceneGeneralObjects.instance.playerTr;//GameObject.FindObjectOfType<CharacterControllerScript>().transform;
	}
	
	// Update is called once per frame
	void Update ()
  {
    if( player == null )
    {
      //player = SceneGeneralObjects.instance.playerTr;
    }

    if( Vector3.Distance(SceneGeneralObjects.instance.playerTr.position, transform.position ) < Distance  )
    {
      outline.enabled = true;
    }
    else
    {
      outline.enabled = false;
    }
	}
}
