using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManekenRotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public GameObject maneken;

  bool isDragging;
  Vector2 deltaPos;
  int touch_Id;
  public float speed;
  Vector2 prevPosition;

  float playerStartAngle = 180.0f;
  float angleH;

  // Use this for initialization
  void Start()
  {
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;
  }

  private void OnEnable()
  {
    playerStartAngle = 180.0f;
    angleH = 0;
    maneken.transform.rotation = Quaternion.Euler(0, playerStartAngle - angleH, 0);
  }

  // Update is called once per frame
  void Update()
  {

    //angleH += (playerCameraAngle + CameraLook.deltaPos.x * horizontalAimingSpeed) * Time.deltaTime;
    if( isDragging )
    {
      angleH += deltaPos.x *speed* Time.deltaTime;
      maneken.transform.rotation = Quaternion.Euler(0, playerStartAngle - angleH, 0);
    }
    

#if !UNITY_EDITOR
    if (Input.touchCount >= touch_Id + 1 && touch_Id != -1)
    {
      deltaPos = (Input.touches[touch_Id].position - prevPosition);
      prevPosition = Input.touches[touch_Id].position;
    }
#else

    deltaPos.x = (Input.mousePosition.x - prevPosition.x);
    deltaPos.y = (Input.mousePosition.y - prevPosition.y);
    prevPosition = new Vector2( Input.mousePosition.x, Input.mousePosition.y );
#endif
  }

  public void OnPointerDown(PointerEventData pointerData)
  {
    touch_Id = pointerData.pointerId;
    prevPosition = pointerData.position;
    isDragging = true;
  }


  public void OnPointerUp(PointerEventData pointerData)
  {
    isDragging = false;
    touch_Id = -1;
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;
  }
}
