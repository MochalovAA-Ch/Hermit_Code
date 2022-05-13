using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutSceneTrigger : MonoBehaviour {

  public int cutSceneNum;

  Collider coll;
	// Use this for initialization
	void Start () {
    coll = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      CutScenesManager.instance.PlayCutScene(cutSceneNum);
      coll.enabled = false;
    }
  }
}
