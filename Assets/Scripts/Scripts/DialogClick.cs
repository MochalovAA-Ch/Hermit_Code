using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogClick : MonoBehaviour, IPointerUpHandler
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if( Input.GetKeyDown(KeyCode.C) )
    {
      EventsManager.TriggerEvent(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC);
    }
  }

  public void OnPointerUp(PointerEventData pointerData)
  {
    Debug.Log("asdasdas");
    EventsManager.TriggerEvent(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC);
  }
}
