using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RollOverScript : MonoBehaviour, IPointerDownHandler
{

  public InputController input;
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void OnPointerDown(PointerEventData pointerData)
  {
    input.rolloverBtnPressed = true;
    input.Info.rolloverInput = true;
  }
}
