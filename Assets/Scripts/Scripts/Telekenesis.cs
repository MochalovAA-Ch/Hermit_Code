using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekenesis : MonoBehaviour {

  RaycastHit telHit;
  public float startCheckDistance;
  public LayerMask telMask;
  bool isPicked;
  public static TelekenesisObject telObject;
  public static float distanceToObject;
  Vector3 prevPoint;
  Vector3 currPoint;
  public Transform testObject;


  public LineRenderer telLineRend;
  public Transform charTr;
	// Use this for initialization
	void Start ()
  {

	}

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.PIKC_UP_TELEKENESIS, PickUp );
  }

  private void OnDisable()
  {
    EventsManager.StopListening(EventsIds.PIKC_UP_TELEKENESIS, PickUp);
  }

  // Update is called once per frame
  void Update ()
  {

		if( CharacterControllerScript.instance.isTelekenesisState )
    {
      //StartRenderTelLine();
      Debug.DrawRay(transform.position, transform.forward * startCheckDistance, Color.red);
      /*if ( checkTelObject() )
      {
        if( Input.GetKeyDown( KeyCode.P ) )
        {
          isPicked = true;
          ThirdPersonOrbitCam.isTelekenesis = true;
          distanceToObject = Vector3.Distance(telHit.transform.position, transform.position);
          prevPoint = transform.position + transform.forward * distanceToObject;
          currPoint = prevPoint;
          telObject = telHit.transform.gameObject.GetComponent<TelekenesisObject>();
          //telObject.startPos = telObject.transform.position;
          telObject.PickUp();
        }
       // Debug.Log("asdasd");
      }
      else
      {
        //Если между лучом и предметом что-то встает, бросаем его
      }*/
    }
    if( Input.GetKeyDown( KeyCode.P ) )
    {
      if (!isPicked)
        PickUp();
      else
        Drop();
    }

    if( isPicked )
    {
      currPoint = transform.position + transform.forward * distanceToObject;
      testObject.position = currPoint;
      telObject.MoveTelObject(currPoint - prevPoint);
    }
	}

  private void FixedUpdate()
  {
    if( isPicked )
    {

     // prevPoint = currPoint;
    }
  }

  void PickUp()
  {
    if (!CharacterControllerScript.instance.isTelekenesisState)
      return;

    isPicked = true;
    ThirdPersonOrbitCam.instance.isTelekenesis = true;
    prevPoint = transform.position + transform.forward * distanceToObject;
    currPoint = prevPoint;
    telObject.PickUp();
  }

  void Drop()
  {
    isPicked = false;
    ThirdPersonOrbitCam.instance.isTelekenesis = false;
    telObject.Drop();

  }

  void StartRenderTelLine()
  {
    telLineRend.SetPosition(0, charTr.position + charTr.forward * 0.5f);
    telLineRend.SetPosition(1, charTr.position + charTr.forward * 15.5f);
  }

  void StopRenderTelLine()
  {
    
  }

}
