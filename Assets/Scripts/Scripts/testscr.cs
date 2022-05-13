using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscr : MonoBehaviour {

  Rigidbody rb;
  public float gravity;
  public float xDist;

  public Transform playerTr;

  bool startFalling;
  float fallingTime;
  float velX;

  float startFallTime;
  float endFallTime;

	// Use this for initialization
	void Start ()
  {
    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
  {
		if( Input.GetKey( KeyCode.S ) )
    {
      startFalling = true;
      Vector3 direction = playerTr.position - rb.position;
      //Горизонтальный веткор направления
      Vector3 vectX = new Vector3(direction.x, 0.0f, direction.z);
      startFallTime = Time.time;
      Vector3 velocityX = vectX.normalized * GameUtils.getFallingVx(-direction.y, vectX.magnitude, gravity);//getFallingVx(-direction.y, 0, vectX.magnitude);//GameUtils.getFallingVx(-direction.y, vectX.magnitude, gravity);
      rb.velocity = velocityX;// + new Vector3(0.0f, gravity, 0.0f);
      //velX = getFallingVx( -direction.y, 0, xDist);
      //rb.velocity = new Vector3(velX, 0.0f, 0.0f);
      //rb.useGravity = true;
    }
    /*if( startFalling )
    {
      rb.velocity += new Vector3(0.0f, gravity, 0.0f) * Time.deltaTime;
      fallingTime += Time.deltaTime;
    }*/
    
  }

  private void FixedUpdate()
  {
    if (startFalling)
    {
      rb.velocity += new Vector3(0.0f, gravity, 0.0f) * Time.fixedDeltaTime;
      //Debug.Log(rb.velocity);
      ///fallingTime += Time.fixedDeltaTime;
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log( Time.time - startFallTime );
    //.Log(transform.position.x);
  }


  float getFallingVx( float h, float v0, float xDistance )
  {
    float temp = 2 * h /  -gravity;
    float fallTime = Mathf.Sqrt(temp);
    return xDistance / fallTime;
  }
}
