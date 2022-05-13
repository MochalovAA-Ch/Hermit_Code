using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject2 : MonoBehaviour {

  public float pushingSpeed;
  Rigidbody rb;

  public LayerMask layer;

  Vector3 moveVector3;

  bool shouldMove;

  public float maxForceMagnitude;
  // Use this for initialization
  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    if( shouldMove )
    {
      rb.velocity = moveVector3 * pushingSpeed;
    }
   /* if (shouldMove)
    {
      if (Physics.CheckBox(transform.position + moveVector3 * 0.1f, new Vector3(1.5f, 1.5f, 1.5f), transform.rotation, layer, QueryTriggerInteraction.Ignore))
      {
        Debug.Log("Задеваем");
        return;
      }
      transform.position += moveVector3 * pushingSpeed * Time.deltaTime;
    }*/
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      shouldMove = true;
      Vector3 velocityVector;
      CalculatePushingVector(other.transform);
      //rb.velocity = moveVector3 * pushingSpeed;
      //Debug.Log( CalculatePushingVector(collision.transform) );
      //rb.velocity = collision.transform.forward * pushingSpeed;
      //rb.AddForce(collision.transform.forward * pushingSpeed*JoystickController.instance.InputDirection.magnitude - new Vector3(rb.velocity.x, 0.0f, rb.velocity.z) );
      // rb.velocity =
      //Debug.Log(  "Stolknulis" );
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      //rb.AddForce(collision.transform.forward * pushingSpeed * JoystickController.instance.InputDirection.magnitude - new Vector3(rb.velocity.x, 0.0f, rb.velocity.z));
      //Debug.Log("Stolknulis");
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      shouldMove = false;
      //rb.velocity = Vector3.zero;
      //Debug.Log("Stolknulis");
    }
  }


  //Вычисляем вектор перемещния
  void CalculatePushingVector(Transform tr)
  {
    //rb.velocity = 0.0f;
    Vector3 dist = transform.position - tr.position;

    if (Mathf.Abs(dist.x) > Mathf.Abs(dist.z))
    {
      moveVector3 = new Vector3(dist.x, 0.0f, 0.0f);
    }
    else
    {
      moveVector3 = new Vector3(0.0f, 0.0f, dist.z);
    }
  }


}
