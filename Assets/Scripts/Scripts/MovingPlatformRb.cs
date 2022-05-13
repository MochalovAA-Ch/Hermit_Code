using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformRb : MonoBehaviour {

  public Vector3 moveVector3;
  public float moveDistance;
  public float moveSpeed;
  float currMovedDistanse;
  Transform tr;
  Rigidbody rb;
  // Use this for initialization
  void Start()
  {
    //loforward * moveSpeed;
    tr = GetComponent<Transform>();
    rb = GetComponent<Rigidbody>();
    //moveVector3 = tr.forward * moveSpeed;///*new Vector3(1.0f, 0.0f, 0.0f)*moveSpeed;*/tr.worldToLocalMatrix.MultiplyVector(tr.forward) * moveSpeed;
    currMovedDistanse = moveDistance / 2;
  }

  bool shouldMoveForward = true;
  // Update is called once per frame
  void FixedUpdate()
  {
    if (shouldMoveForward)
    {
      if (currMovedDistanse < moveDistance)
      {
        currMovedDistanse += moveVector3.magnitude * moveSpeed* Time.fixedDeltaTime;
        //tr.Translate(moveVector3 * Time.deltaTime, Space.World);
        rb.MovePosition(rb.position + moveVector3 * moveSpeed* Time.fixedDeltaTime);
        //rb.velocity = moveVector3;
      }
      else
      {
        moveVector3 *= -1;
        currMovedDistanse = 0.0f;
        shouldMoveForward = false;
        //EventsManager.TriggerEvent(EventsIds.MOVING_PLATFORM_VELOCITY_CHANGED);
        //Debug.Log("velocity changed");
      }
    }
    else
    {
      if (currMovedDistanse < moveDistance)
      {
        currMovedDistanse += moveVector3.magnitude * moveSpeed* Time.fixedDeltaTime;
        //rb.velocity = moveVector3;
        //tr.Translate(moveVector3 * Time.deltaTime, Space.World );
        rb.MovePosition(rb.position + moveVector3 * moveSpeed*Time.fixedDeltaTime);
        //tr.Translate(moveVector3 * Time.deltaTime, Space.World);
      }
      else
      {
        moveVector3 *= -1;
        currMovedDistanse = 0.0f;
        //rb.velocity = moveVector3;
        //tr.Translate(moveVector3 * Time.deltaTime, Space.World);
        rb.MovePosition(rb.position + moveVector3 * moveSpeed* Time.fixedDeltaTime);
        shouldMoveForward = false;
        //EventsManager.TriggerEvent(EventsIds.MOVING_PLATFORM_VELOCITY_CHANGED);
        // Debug.Log("velocity changed");
      }
    }
  }
}
