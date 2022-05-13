using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour {


  public Vector3 direction;
  public float speed;
  float angle;
	// Use this for initialization
	void Start ()
  {
    angle = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
  {
    angle += Time.deltaTime;
    angle = angle % 360.0f;
    transform.Translate( direction * Mathf.Sin(angle * speed ) * Time.deltaTime );
	}
}
