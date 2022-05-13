using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

  InputController inputInfo;
  public LayerMask layer;

  AudioSource audioSource;
  public AudioClip dropDown;
  public AudioClip pickUp;

  //public bool shouldReturnStartPoint

  Vector3 startPoint;

  Rigidbody rb;
  Collider coll;
  Transform playerTr;
  Transform playerMeshTr;

  float pickUpDistance = 10.0f;

  Transform physCube;
  Transform moveCube;

  public bool isPicked = false;

  enum moveObjectStates {  FALLING, GROUNDED };

  moveObjectStates state = moveObjectStates.FALLING;

  public bool isPlayerWasNear = false;

  // Use this for initialization
  void Start ()
  {
    audioSource = GetComponent<AudioSource>();
    startPoint = transform.position;
    rb = GetComponent<Rigidbody>();
    inputInfo = FindObjectOfType<InputController>();
    playerTr = FindObjectOfType<CharacterControllerScript>().transform;
    playerMeshTr = playerTr.GetChild(0);
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (!isPicked)
      checkForPickUp();


    switch (state)
    {
     /* case moveObjectStates.FALLING:
      {
        Falling();
        break;
      };
      case moveObjectStates.GROUNDED:
      {
        //Grounded();
        break;
      };
      */
    }
  }

  void Grounded()
  {
    RaycastHit hit;
    if (!Physics.BoxCast(transform.position, Vector3.one, -Vector3.up, out hit, Quaternion.identity, gravityVec.magnitude * Time.deltaTime, layer, QueryTriggerInteraction.Ignore))
    {
     // state = moveObjectStates.FALLING;
    }
  }

  void checkForPickUp()
  {
    if( Vector3.Distance( transform.position, playerTr.position ) > pickUpDistance )
    {
      if( isPlayerWasNear )
      {
        GameUIController.instance.HideInterractIcon();
        isPlayerWasNear = false;
      }
      return;
    }
    isPlayerWasNear = true;
    Vector3 direction = transform.position - playerTr.position; 
    direction.y = 0.0f;

    float angle = Vector3.SignedAngle(direction, playerMeshTr.forward, Vector3.up );

    if ( angle < 90.0f && angle > -90.0f )
    {
      if ( inputInfo.Info.interactInput )
      {
        isPicked = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        inputInfo.Info.interactInput = false;
        audioSource.clip = pickUp;
        audioSource.Play();
        CharacterControllerScript.instance.PickUp(this);
      }
      else
      {
        GameUIController.instance.ShowInterractIcon();
      }
    }
    else
    {
      GameUIController.instance.HideInterractIcon();
    }
  }

  public void DropDown( Vector3 velocity )
  {
    isPicked = false;
    GameUIController.instance.HideInterractIcon();
    rb.isKinematic = false;
    rb.useGravity = true;
    rb.AddForce(velocity*10.0f);
    gravityVec.y = -1.0f;
    state = moveObjectStates.FALLING;
  }

  float gravity = 0.8f;
  Vector3 gravityVec = new Vector3( 0.0f, -1.0f, 0.0f );
  Vector3 checkGroundVector= new Vector3( 2.0f, 0.5f, 2.0f ) * 0.5f;
  void Falling()
  {
    RaycastHit hit;
    if (Physics.BoxCast(transform.position, Vector3.one, -Vector3.up, out hit, Quaternion.identity, gravityVec.magnitude * Time.deltaTime, layer, QueryTriggerInteraction.Ignore))
    {
      gravityVec.y = -1.0f;
      state = moveObjectStates.GROUNDED;
    }
    else
    {
      gravityVec.y -= gravity;
      transform.position += gravityVec * Time.deltaTime;
    }
  }


  private void OnCollisionEnter(Collision collision)
  {
    if (isPicked)
      return;

    if( collision.transform.tag != "Player" )
    {
      if( Vector3.Distance( playerTr.position, transform.position) < 50.0f )
      audioSource.clip = dropDown;
      audioSource.Play();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if( other.tag == "MovableObjectTrigger" )
    {
      transform.position = startPoint;
    }
  }

  private void OnDrawGizmosSelected()
  {
   // Gizmos.DrawCube(transform.position + gravityVec, checkGroundVector);
  }
}
