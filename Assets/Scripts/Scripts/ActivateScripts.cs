using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScripts : MonoBehaviour {

  public leverScript lever;
  public List<MonoBehaviour> scriptsList;
	// Use this for initialization
	void Start ()
  {
    lever.leverOn = EnableScripts;
  }
	
	// Update is called once per frame
	void Update ()
  {

	}

  void EnableScripts()
  {
    for( int i = 0; i < scriptsList.Count; i++ )
    {
      scriptsList[i].enabled = true;
    }
  }
}
