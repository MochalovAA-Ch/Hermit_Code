using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
  public static JoystickController instance;
  private Image jsContainer;
  private Image joystick;

  public Vector3 InputDirection;
  public bool isJoystickDragging;
  public float inputAngle; //Угол поворота джойтсика

  void Start()
  {
    instance = this;
    jsContainer = GetComponent<Image>();
    joystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
    InputDirection = Vector3.zero;
    inputAngle = 0.0f;
  }


  void OnEnable()
  {
    if( joystick == null )
    {
      joystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
    }

    joystick.rectTransform.anchoredPosition = Vector2.zero;
    isJoystickDragging = false;
    InputDirection = Vector3.zero;
    inputAngle = 0.0f;
  }

  public void OnDrag(PointerEventData ped)
  {
    Vector2 position = Vector2.zero;
    isJoystickDragging = true;
    //To get InputDirection
    RectTransformUtility.ScreenPointToLocalPointInRectangle
            (jsContainer.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position);

    position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
    position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

    float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
    float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

    InputDirection = new Vector3(x, y, 0);
    InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

    inputAngle = x < 0 ? -Vector2.Angle( Vector2.up, new Vector2(x, y) ) : Vector2.Angle( Vector2.up, new Vector2(x, y) );
      
    
    //to define the area in which joystick can move around
    joystick.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (jsContainer.rectTransform.sizeDelta.x / 3)
                                                           , InputDirection.y * (jsContainer.rectTransform.sizeDelta.y) / 3);
  }

  public void OnPointerDown(PointerEventData ped)
  {
    OnDrag(ped);
  }

  public void OnPointerUp(PointerEventData ped)
  {
    isJoystickDragging = false;
    InputDirection = Vector3.zero;
    joystick.rectTransform.anchoredPosition = Vector3.zero;
    inputAngle = 0.0f;
  }
}
