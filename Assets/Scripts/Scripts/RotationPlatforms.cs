using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlatforms : MonoBehaviour
{

  public GameObject platform;

  public int platformCounts;

  public float radius;

  public float rotationSpeed;

	// Use this for initialization
	void Start ()
  {
    float angle = 360.0f / platformCounts;

    float currAngle = 0.0f;
    for ( int i = 0; i < platformCounts; i++ )
    {
      float x = radius * Mathf.Sin( currAngle * Mathf.Deg2Rad );
      float z = radius * Mathf.Cos( currAngle * Mathf.Deg2Rad );
      Instantiate(platform, gameObject.transform.position + new Vector3( x, 0, z ), Quaternion.identity, gameObject.transform);
      currAngle += angle;
    }

	}
	
	// Update is called once per frame
	void Update ()
  {
    transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		
	}
}
