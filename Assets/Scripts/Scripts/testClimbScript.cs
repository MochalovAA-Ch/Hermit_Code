using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testClimbScript : MonoBehaviour {

  public Collider other;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
    if( Input.GetKeyDown( KeyCode.P ) )
    {
      testFn();
    }
	}
  private void OnTriggerEnter(Collider other)
  {
   /* BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
    if (boxCollider == null)
      return;
    //other.
    float halfXwidth = boxCollider.size.x * 0.5f;
    Debug.Log( transform.right );
    Plane plane = new Plane( transform.right, new Vector3( transform.position.x, other.transform.position.y, transform.position.z));
    float distance = plane.GetDistanceToPoint( other.transform.position );
    //float distance = plane.GetDistanceToPoint( charController.transform.position );
    Debug.Log(distance);
    //Debug.Log(plane.GetDistanceToPoint(charController.transform.position));
    other.transform.rotation = transform.rotation * Quaternion.Euler(0.0f, -90.0f, 0.0f);

    if (distance < halfXwidth)
    {
      Debug.Log("Сносим");
      // charController.transform.position += other.transform.right * (halfXwidth - distance);
      other.transform.position += transform.right * (halfXwidth - distance);
    }*/
  }
  void testFn()
  {
    BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
    if (boxCollider == null)
      return;
    //other.
    float halfXwidth = boxCollider.size.x * 0.5f;
    Debug.Log(transform.right);
    Plane plane = new Plane(transform.right, new Vector3(transform.position.x, other.transform.position.y, transform.position.z));
    float distance = plane.GetDistanceToPoint(other.transform.position);
    //float distance = plane.GetDistanceToPoint( charController.transform.position );
    Debug.Log(distance);
    //Debug.Log(plane.GetDistanceToPoint(charController.transform.position));
    other.transform.rotation = transform.rotation * Quaternion.Euler(0.0f, -90.0f, 0.0f);

    if (distance < halfXwidth)
    {
      Debug.Log("Сносим");
      // charController.transform.position += other.transform.right * (halfXwidth - distance);
      other.transform.position += transform.right * (halfXwidth - distance);
    }
  }
}
