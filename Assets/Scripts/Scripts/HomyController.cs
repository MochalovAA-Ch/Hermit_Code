using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomyController : MonoBehaviour {

  enum HomyStates { FollowPlayer, Launched, Controlled, Stopped, ReturnToPlayer };

  public InputController input;
  public GameObject HomyIdle;
  public GameObject HomyThrowGameObject;
  public HomyThrow homyThrow;

  public Transform player;
  public Transform playerMesh;
  bool canLaunch;
  HomyStates currentState;
  // Use this for initialization
  void Start ()
  {
    
  }

  private void OnEnable()
  {
    canLaunch = true;
    currentState = HomyStates.FollowPlayer;
  }

  private void Update()
  {
    if (!GameSystem.isHomyAvaible)
    {
      gameObject.SetActive(false);
      GameUIController.instance.HideHomyThrow();
    }
      

    switch ( currentState )
    {
      case HomyStates.FollowPlayer:
      {
        HomyThrowGameObject.SetActive(false);
        FollowPlayer_Update(); 
        break;
      }

      case HomyStates.Launched:
      {
        Launch_Update();
        break;
      }

      case HomyStates.ReturnToPlayer:
      {
        ReturnToPlayer_Update();
        break;
      }
    }
  }

  // Update is called once per frame
  void LateUpdate ()
  {
    if (currentState == HomyStates.FollowPlayer)
    {
      FollowPlayer_Update();
      return;
    }
	}

  void FollowPlayer_Update()
  {
    if(  input.Info.homyLaunchInput )
    {
      input.Info.homyLaunchInput = false;
      currentState = HomyStates.Launched;
      LaunchHomy(  );
      return;
    }

    transform.rotation = playerMesh.rotation;
    transform.position = player.position - playerMesh.right*0.5f - playerMesh.forward*0.5f;
  }

  void LaunchHomy( )
  {
    currentState = HomyStates.Launched;
    SwitchToLaunch(true);
    homyThrow.LaunchHomy(  );
  }

  void Launch_Update()
  {
    if( input.Info.homyLaunchInput )
    {
      currentState = HomyStates.ReturnToPlayer;
      homyThrow.ReturnToPlayer();
      return;
    }

    if( homyThrow.homyThrowState == HomyThrow.HomyThrowState.ReturnToPlayer )
    {
      currentState = HomyStates.ReturnToPlayer;
      return;
    }

  }

  void ReturnToPlayer_Update()
  {
    if( homyThrow.homyThrowState == HomyThrow.HomyThrowState.FolowPlayer )
    {
      SwitchToLaunch(false);
      currentState = HomyStates.FollowPlayer;
    }
  }

  void ReturnHomy()
  {
    SwitchToLaunch(false);
    currentState = HomyStates.FollowPlayer;
  }

  void SwitchToLaunch( bool isLaunched )
  {
    HomyThrowGameObject.SetActive(isLaunched);
    HomyIdle.SetActive(!isLaunched);
  }
}
