using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public static GameController instance;

  public Transform  lastActiveCheckpoint;

  private void OnEnable()
  {
    EventsManager.StartListening( EventsIds.DECREASE_LIVES, CheckLivesCount );
  }

  private void OnDestroy()
  {
    EventsManager.StopListening( EventsIds.DECREASE_LIVES, CheckLivesCount );
  }

  // Use this for initialization
  void Start ()
  {
    instance = this;
  }
	
	// Update is called once per frame
	void Update ()
  {

		
	}

  //Проверяем, сколько жизней
  void CheckLivesCount()
  {
    if ( GameSystem.playerLives <= 0 )
    {
      EventsManager.TriggerEvent( EventsIds.GAME_OVER );
    }
  }

  public void SetCheckpoint( Transform tr )
  {
    lastActiveCheckpoint = tr;
  }

  public void SetPlayerToCheckpoint( Transform player )
  {
    player.position = lastActiveCheckpoint.position + lastActiveCheckpoint.forward*2 + Vector3.up;
  }
}
