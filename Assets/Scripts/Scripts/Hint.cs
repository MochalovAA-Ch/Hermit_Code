using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HintIds
{
  
  FireHint = 10,
  SaeonHome = 11,
  HomySaveHint = 12,
  HomyStartLv2Hint  = 13,
// WadrobeHint = 55,

  TutorialHint = 0,
  Tutorial_1 = 1,
  Tutorial_2 = 2,
  Tutorial_3 = 3,
  Tutorial_4 = 4,
  Tutorial_5 = 5,
  Tutorial_6 = 6,
  Tutotial_7 = 7,
  Tutorial_8 = 8,
  Tutorial_9 = 9,
// Tutorial_10 = 10
}

public class Hint : MonoBehaviour
{
  public HintIds hintId;

  int currentHintIndex;

  public bool isTriggerHint;

  public bool isClosed;
  public bool shouldClosedWhenPlayed;

  public Outline outline;

  ThirdPersonOrbitCam camScript;

  public Hint()
  {
    //currentid++;
    //id = currentid;
  }

  public List<Transform> CameraPosition;

  public void Start()
  {
    if( isTriggerHint && shouldClosedWhenPlayed )
    {
      if( GameSystem.playedHints[(int)hintId] )
      {
        this.enabled = false;
      }
    }
  }

  public void Update()
  {
  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.SHOW_NEXT_HINT_REPLIC, ShowNextHint);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.SHOW_NEXT_HINT_REPLIC, ShowNextHint);
  }

  public void ShowHint()
  {
    CharacterControllerScript.instance.EnterHintState(this);
    camScript = Camera.main.GetComponent<ThirdPersonOrbitCam>();
    camScript.SetHintCameraState( CameraPosition[0] );
    isClosed = false;
    currentHintIndex = 0;
    GameSystem.currentHintId = (int)hintId;
    GameUIController.instance.ShowHint(LangResources.GetHintText((int)hintId, currentHintIndex) );//HintText[currentHintIndex] );
  }

  public bool IsAlreadyPlayed()
  {
    return GameSystem.playedHints[(int)hintId];
  }

  void ShowNextHint()
  {
    if (GameSystem.currentHintId != (int)hintId )
      return;

    currentHintIndex++;
    if (currentHintIndex >= LangResources.GetHintCount( (int) hintId ) )
    {
      CloseHint();
    }
    else
    {
      if( currentHintIndex < CameraPosition.Count )
      {
        camScript.ChangeCamTarget(CameraPosition[currentHintIndex] );
        //camScript.transform.position = CameraPosition[currentHintIndex].position;
        //camScript.transform.rotation = CameraPosition[currentHintIndex].rotation;
      }
      GameUIController.instance.ShowHint(LangResources.GetHintText((int)hintId, currentHintIndex));
    }
  }

  public void CloseHint()
  {

    if (GameSystem.currentHintId != (int)hintId)
      return;
    if (camScript == null)
      return;
    camScript.SetDefaultCameraState();
    isClosed = true;
    GameUIController.instance.CloseHint();
    GameSystem.playedHints[(int)hintId] = true;
    if ( shouldClosedWhenPlayed )
    {
      //GameSystem.playedHints[(int)hintId] = true;
      Destroy(gameObject);
    }
  }
}
