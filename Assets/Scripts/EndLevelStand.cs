using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelStand : MonoBehaviour {

  public List<GameObject> animObjects;
  public Animator animContr;
  public GameObject nextLevelWay;

  public bool isActivated = false;
	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update ()
  {
  
	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      Debug.Log(GameSystem.playerKeys);
      if( GameSystem.playerKeys >= 5 )
      {
        ActivateStand();
      }
    }
  }

  public void ActivateStand()
  {
    if (isActivated)
      return;
    EventsManager.TriggerEvent(EventsIds.END_LEVEL_STAND_ACTIVATED);
    for (int i = 0; i < animObjects.Count; i++)
    {
      animObjects[i].SetActive(true);
    }
    isActivated = true;
    animContr.Play("Activate");
    nextLevelWay.SetActive(true);
  }
}
