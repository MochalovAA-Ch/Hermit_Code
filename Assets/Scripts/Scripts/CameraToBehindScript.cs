using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraToBehindScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    InterfaceSoundsManager.instance.PlayBtnClip();
    EventsManager.TriggerEvent(EventsIds.CAMERA_TO_PLAYER_BEHIND);
  }

  public void OnPointerUp(PointerEventData pointerData)
  {

  }
}
