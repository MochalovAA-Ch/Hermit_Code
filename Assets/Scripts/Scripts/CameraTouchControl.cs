using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraTouchControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public void OnPointerDown(PointerEventData pointerData)
  {
    List<GameObject> hoveredGameObjectsList = pointerData.hovered;
    for( int i = 0; i < hoveredGameObjectsList.Count; i++ )
    {
      //Debug.Log(hoveredGameObjectsList[i]);
    }
    
  }

  public void OnPointerUp(PointerEventData pointerData)
  {
    /*returnToDefaultTimer = 0.0f;
    isDragging = false;
    touch_Id = -1;
    deltaPos = Vector2.zero;
    prevPosition = Vector2.zero;*/
  }

}
