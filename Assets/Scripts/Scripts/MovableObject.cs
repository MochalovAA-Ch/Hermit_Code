using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

  public float pushingSpeed;
  Rigidbody rb;

  Vector3 moveVector3;

  public float maxForceMagnitude;
	// Use this for initialization
	void Start ()
  {
    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Vector3 velocityVector;
      CalculatePushingVector(collision.transform);
      rb.velocity = moveVector3 * pushingSpeed;
      //Debug.Log( CalculatePushingVector(collision.transform) );
      //rb.velocity = collision.transform.forward * pushingSpeed;
      //rb.AddForce(collision.transform.forward * pushingSpeed*JoystickController.instance.InputDirection.magnitude - new Vector3(rb.velocity.x, 0.0f, rb.velocity.z) );
      // rb.velocity =
      //Debug.Log(  "Stolknulis" );
    }
  }

  private void OnCollisionStay(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      //rb.AddForce(collision.transform.forward * pushingSpeed * JoystickController.instance.InputDirection.magnitude - new Vector3(rb.velocity.x, 0.0f, rb.velocity.z));
      Debug.Log("Stolknulis");
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      //rb.velocity = Vector3.zero;
      Debug.Log("Stolknulis");
    }
  }


  //Вычисляем вектор перемещния
  void CalculatePushingVector(  Transform tr)
  {
    //rb.velocity = 0.0f;
    Vector3 dist =  transform.position - tr.position;

    if( Mathf.Abs( dist.x ) > Mathf.Abs( dist.z ) )
    {
      moveVector3 = new Vector3(dist.x, 0.0f, 0.0f);
    }
    else
    {
      moveVector3 = new Vector3( 0.0f, 0.0f, dist.z);
    }
  }

}
