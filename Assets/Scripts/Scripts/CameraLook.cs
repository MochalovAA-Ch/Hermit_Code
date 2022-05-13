using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CameraLook : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

  public static bool isDragging;
  public static Vector2 deltaPos;
  public static bool shouldReturnToDefaultVert = false;
  public static bool hasStartedCameraReturn = false;
  int touch_Id;
  public static float smoothness;
  private Vector2 prevPosition;
  public float returnToDefaultTimerLimit;
  float returnToDefaultTimer;

	// Use this for initialization
	void Start ()
  {
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;
    smoothness = 0.05f;
	}

  private void OnEnable()
  {
    //deltaPos = Vector2.zero;
   // isDragging = true;
  }

  // Update is called once per frame
  void Update()
  {
    if ( !isDragging )
    {
      if ( ThirdPersonOrbitCam.instance.isCamInCorutine )
      {
        returnToDefaultTimer = 0.0f;
      }
      else
      {
        //Если еще не начали возвращать камеру в исходное положение
        if (!hasStartedCameraReturn)
        {
          if (returnToDefaultTimer < returnToDefaultTimerLimit)
          {
            returnToDefaultTimer += Time.deltaTime;
            shouldReturnToDefaultVert = false;
          }
          else
          {
            shouldReturnToDefaultVert = true;
            //hasReturnedToDefaultVert = false;
          }
        }
      }
      return;
    }

#if !UNITY_EDITOR
    if (Input.touchCount >= touch_Id + 1 && touch_Id != -1)
    {
      deltaPos = ( Input.touches[touch_Id].position - prevPosition) * smoothness;
      prevPosition = Input.touches[touch_Id].position;
    }
#else
    
    deltaPos.x = ( Input.mousePosition.x - prevPosition.x ) * smoothness;
    deltaPos.y = ( Input.mousePosition.y - prevPosition.y ) * smoothness;
    prevPosition = new Vector2( Input.mousePosition.x, Input.mousePosition.y );
#endif
  }

  public void OnPointerDown( PointerEventData pointerData )
  {
    touch_Id = pointerData.pointerId;
    prevPosition = pointerData.position;
    isDragging = true;
    shouldReturnToDefaultVert = false;
    hasStartedCameraReturn = false;

#if !UNITY_EDITOR
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, 100.0f))
    {
      Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
    }
#else
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, 100.0f))
    {
      if( hit.transform.tag == "Telek" )
      {
        PickUptelekenesis(hit);
      }
    }
#endif

  }


  public void OnPointerUp ( PointerEventData pointerData )
  {
    returnToDefaultTimer = 0.0f;
    isDragging = false;
    touch_Id = -1;
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;
  }

  void PickUptelekenesis( RaycastHit hit )
  {
    isDragging = false;
    Debug.Log("Telek");
    Telekenesis.telObject = hit.transform.gameObject.GetComponent<TelekenesisObject>();
    CharacterControllerScript.instance.isTelekenesisState = true;
    CharacterControllerScript.instance.charTrPublic.LookAt(hit.transform);
    CharacterControllerScript.instance.charTrPublic.rotation = Quaternion.Euler(0.0f, CharacterControllerScript.instance.charTrPublic.rotation.eulerAngles.y, 0.0f);
    //Telekenesis.distanceToObject = Vector3.Distance( Camera.main.transform.position, hit.point );
    ThirdPersonOrbitCam.instance.SetCamBehindPlayerSmooth();
    EventsManager.TriggerEvent(EventsIds.PIKC_UP_TELEKENESIS);
  }
}
