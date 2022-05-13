using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WardrobeCharController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public static Vector2 deltaPos;
  public float smoothness;
  private Vector2 prevPosition;
  bool isDragging;
  int touch_Id;

  float angle;

  public Transform playerTr;


	// Use this for initialization
	void Start ()
  {
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (!isDragging)
      return;
    
#if !UNITY_EDITOR
    if (Input.touchCount >= touch_Id + 1 && touch_Id != -1)
    {
      deltaPos = ( Input.touches[touch_Id].position - prevPosition) / smoothness;//Input.touches[touch_Id].position.posion
      prevPosition = Input.touches[touch_Id].position;
    }
#else
      deltaPos.x = (Input.mousePosition.x - prevPosition.x) / smoothness;
      deltaPos.y = (Input.mousePosition.y - prevPosition.y) / smoothness;
      prevPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

    angle -= deltaPos.x * Time.deltaTime;
    playerTr.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

  }
  public void OnPointerDown(PointerEventData pointerData)
  {
    touch_Id = pointerData.pointerId;
    prevPosition = pointerData.position;
    isDragging = true;
    angle = playerTr.rotation.eulerAngles.y;
  }

  public void OnPointerUp(PointerEventData pointerData)
  {
    isDragging = false;
    touch_Id = -1;
    deltaPos = Vector2.zero;
    
    prevPosition = Vector2.zero;
  }
}
