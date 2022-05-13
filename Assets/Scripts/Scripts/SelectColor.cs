using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectColor : MonoBehaviour, IPointerDownHandler
{
  public Material material;

  Image img;
  
  // Use this for initialization
  void Start()
  {
    img = GetComponent<Image>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnPointerDown(PointerEventData ped)
  {
    material.color = img.color;
  }
}
