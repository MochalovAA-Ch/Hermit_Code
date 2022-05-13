using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceViewScipt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
  private void OnTriggerEnter(Collider other)
  {
   // other.transform.GetChild(0).gameObject.SetActive(true);
  }

  private void OnTriggerExit(Collider other)
  {
    //other.transform.GetChild(0).gameObject.SetActive(false);
  }
}
