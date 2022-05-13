using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomyInCage : MonoBehaviour {

  Transform player;
  public Transform playerPos;
  public Transform homyTr;
  public GameObject cage;
  public GameObject partAnim;
  public GameObject HomyContr;
  bool isEntered = false;
  public GameObject keyAether;
  public Hint hint;
	// Use this for initialization
	void Start () {

    if (GameSystem.isHomyAvaible)
    {
      keyAether.SetActive(true);
      Destroy(cage);
      Destroy( gameObject );
    }

    player = FindObjectOfType<CharacterControllerScript>().transform;
	}

  private void OnEnable()
  {
    
  }

  // Update is called once per frame
  void Update () {

    if( !isEntered )
      homyTr.LookAt(player);
    else
    {
      if ( hint == null )
      {
        GameSystem.collectedKeyIndex = Keys.AETHER;
        GameSystem.playerKeys++;
        EventsManager.TriggerEvent(EventsIds.CHANGE_KEYS_COUNT);
        GameSystem.isHomyAvaible = true;
        HomyContr.SetActive(true);
        GameUIController.instance.ShowHomyThrow();
        Destroy(gameObject);
      }
    }
	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      isEntered = true;
      Destroy(cage);
      player.position = playerPos.position;
      partAnim.SetActive(true);
      homyTr.LookAt(player);
     // CharacterControllerScript.instance.EnterHintState( hint );
      CharacterControllerScript.instance.charTrPublic.LookAt(transform);
      CharacterControllerScript.instance.charTrPublic.rotation = Quaternion.Euler(0.0f, CharacterControllerScript.instance.charTrPublic.rotation.eulerAngles.y, 0.0f);
      hint.ShowHint();
    }
  }

}
