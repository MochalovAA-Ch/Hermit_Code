using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneScipt : MonoBehaviour {

  public bool isPlayerInZone;
  public bool isPlayerNoticed;
	// Use this for initialization
	void Start () {
    isPlayerInZone = false;
    isPlayerNoticed = false;

  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      isPlayerInZone = true;
      isPlayerNoticed = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if( other.tag == "Player" )
    {
      isPlayerInZone = false;
    }
  }
}
