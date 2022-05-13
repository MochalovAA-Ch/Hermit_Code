using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectCloth : MonoBehaviour, IPointerDownHandler
{
  public enum ClothType { HAIRS, EYES, BEARDS, EARS, TSHIRTS, COATS, PANTS, BOOTS, WEAPONS };

  public int ClothIndex;
  public ClothType clothType;

  public static SelectCloth pressedSelectCloth;

  public int rawImageIndex;
	// Use this for initialization
	void Start () {
		
	}

  // Update is called once per frame
  void Update () {
		
	}
  public void OnPointerDown(PointerEventData ped)
  {
    pressedSelectCloth = this;
    switch (SelectCloth.pressedSelectCloth.clothType )
    {
      case SelectCloth.ClothType.HAIRS:
      {
        GameSystem.hairs = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.EYES:
      {
        GameSystem.eyes = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.BEARDS:
      {
        GameSystem.beards = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.TSHIRTS:
      {
        GameSystem.tshirts = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.COATS:
      {
        GameSystem.coats = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.PANTS:
      {
        GameSystem.pants = ClothIndex;
        break;
      }

      case SelectCloth.ClothType.BOOTS:
      {
        GameSystem.boots = ClothIndex;
        break;
      }
      case SelectCloth.ClothType.WEAPONS:
      {
        GameSystem.weapons = ClothIndex;
        break;
      }
    }
    
    EventsManager.TriggerEvent(EventsIds.SELECT_CLOTH);
  }
}
