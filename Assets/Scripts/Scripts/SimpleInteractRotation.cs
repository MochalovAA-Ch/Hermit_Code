using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractRotation : MonoBehaviour
{
  public Transform target;
  public float rotationAngle;
  public float rotationSpeed;
  public float rotationAcceleration;

  float endRotation;

  bool isStartedRotation;

  float angleRotated;
  float rotationDir;

	// Use this for initialization
	void Start ()
  {
    if (rotationAngle < 0)
      rotationDir = -1.0f;
    else
      rotationDir = 1.0f;

  }
	
	// Update is called once per frame
	void Update ()
  {
    if (isStartedRotation)
    {
      target.rotation *= Quaternion.Euler(0.0f, 0.0f, rotationSpeed * rotationDir * Time.deltaTime);
      angleRotated += rotationSpeed * Time.deltaTime;
      rotationSpeed += rotationAcceleration;

      if ( angleRotated > Mathf.Abs( rotationAngle ) )
      {
        GetComponent<OutlineController>().enabled = false;
        GetComponent<Outline>().enabled = false;
        isStartedRotation = false;
      }
    }
	}

  public void StartRotationToAngle()
  {
    isStartedRotation = true;
    angleRotated = 0.0f;
  }
}
