using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObjectsScript : MonoBehaviour {

  Rigidbody rb;
  public Vector3 velocity;

  Vector3 prevPos;
  Vector3 newPos;
	// Use this for initialization
	void Start ()
  {
   // rb = GetComponent<Rigidbody>();
	}

  private void OnEnable()
  {
    prevPos = transform.position;
    newPos = prevPos;
  }

  // Update is called once per frame
  void Update ()
  {
    //velocity *= Quaternion.Euler(0.0f, 4.5f, 0.0f);
    transform.Translate(velocity * Time.deltaTime);
    //rb.MovePosition( transform.position + velocity*Time.deltaTime );
	}

  private void OnCollisionEnter(Collision collision)
  {
    if( collision.gameObject.tag == "Player" )
    {
      Debug.Log("col");
    }
  }
}
