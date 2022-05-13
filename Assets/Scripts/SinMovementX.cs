using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovementX : MonoBehaviour {

  //public float distance;
  public float maxSpeed;
  public float minSpeed;
  public float distance;
  float accelerate;
  float speed;
  bool shouldMoveForward = true;

  Vector3 startPos;
	// Use this for initialization
	void Start ()
  {
    startPos = transform.position;
    speed = maxSpeed;
    accelerate = (minSpeed * minSpeed - maxSpeed * maxSpeed )/( 2 * distance );
	}

  float path = 0;
  float time = 0;
  float v;

  // Update is called once per frame
  void Update() {
    v = maxSpeed + accelerate * time;
    path = maxSpeed * time + accelerate * (time * time) / 2;
    transform.position = startPos + transform.right * path;
    time += Time.deltaTime;

    //transform.position += transform.right * distance * Mathf.Sin(Time.time * speed) * Time.deltaTime;
    //angle += speed * Time.deltaTime;

    //transform.position = startPos + transform.right*distance* Mathf.Sin( angle ) * Time.deltaTime;
  }
}
