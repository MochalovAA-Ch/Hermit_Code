using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekenesisObject : MonoBehaviour {
  Rigidbody rb;

  public float moveSpeed;
  Transform player;
  Outline outline;

  public Vector3 startPos;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
    player = FindObjectOfType<CharacterControllerScript>().transform;
    outline = GetComponent<Outline>();
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (Vector3.Distance(transform.position, player.position) > 10)
    {
      outline.enabled = false;
    }
    else
    {
      outline.enabled = true;
    }
	}

  public void MoveTelObject(Vector3 newPosition)
  {
    rb.velocity = (startPos - transform.position + newPosition)*moveSpeed;
  }

  public void PickUp()
  {
    startPos = transform.position;
    rb.useGravity = false;
    outline.OutlineWidth = 10.0f;
  }

  public void Drop()
  {
    rb.useGravity = true;
    outline.OutlineWidth = 2.0f;
  }
}
