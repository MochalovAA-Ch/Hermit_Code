using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
  private Image jsContainer;
  private Image joystick;


  public bool isStartPressed = false;
  public bool isEndPressed = false;
  public bool isSwiping = false;
  public Vector3 StartPoint;
  public Vector3 InputDirection;
  float imageContainerRadius;

	// Use this for initialization
	void Start ()
  {
    jsContainer = GetComponent<Image>();
    joystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
    InputDirection = Vector3.zero;
    imageContainerRadius = jsContainer.rectTransform.sizeDelta.x / 2;
  }
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  public void OnDrag(PointerEventData ped)
  {
    Vector2 position = Vector2.zero;

    //To get InputDirection
    RectTransformUtility.ScreenPointToLocalPointInRectangle
            (jsContainer.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position);

    //position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
    //position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

    position.x = position.x - imageContainerRadius;
    position.y = position.y - imageContainerRadius;

    //    float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
    //    float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

    float x = position.x / imageContainerRadius;
    float y = position.y / imageContainerRadius;

    Debug.Log(x + " : " + y );

    InputDirection = new Vector3(x, y, 0);
    InputDirection = InputDirection.magnitude > 1.0f ? InputDirection.normalized : InputDirection;
  }

  public void OnPointerDown(PointerEventData ped)
  {
    isStartPressed = true;
    OnDrag(ped);
  }
  public void OnPointerUp(PointerEventData ped)
  {
    isStartPressed = false;
    InputDirection = Vector3.zero;
    joystick.rectTransform.anchoredPosition = Vector3.zero;
  }

}
