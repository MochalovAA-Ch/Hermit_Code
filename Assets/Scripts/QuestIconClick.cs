using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class QuestIconClick : MonoBehaviour, IPointerDownHandler
{
  public int QuestIconIndex;
  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void OnPointerDown(PointerEventData pointerData)
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    GameSystem.QuestIconClicked = QuestIconIndex;
    EventsManager.TriggerEvent(EventsIds.QUEST_ICON_CLICKED);
  }
}
