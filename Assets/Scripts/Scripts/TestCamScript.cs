using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamScript : MonoBehaviour {

  public Transform player;
  public Vector3 offset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void LateUpdate()
  {
    transform.position = player.position + offset;
  }
}
