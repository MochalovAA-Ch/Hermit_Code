using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
  public List<GameObject> keys;
  public GameObject levelMap;
  public EndLevelStand endLevelStand;
  public Hint hint;
  //public Hint wardrobeHint;
  public Transform playerTr;
  public GameObject meteor;

  public Transform spawnPoint;

  bool isStartPlayFirstScene;
  public bool ShouldPlayHint;
  
  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.END_LEVEL_STAND_ACTIVATED, EndTutorial);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.END_LEVEL_STAND_ACTIVATED, EndTutorial);
  }

  // Use this for initialization
  void Start ()
  {
		if( GameSystem.isTutorialComplete )
    {
      DisableTutorialObjects();
      //endLevelStand.ActivateStand();
      //wardrobeHint.gameObject.SetActive(false);
      playerTr.position = spawnPoint.position;
    }
    else
    {
      EnableTutorialsObjects();
      //wardrobeHint.gameObject.SetActive(true);
    }
	}
	
	// Update is called once per frame
	void Update ()
  {
    if ( GameSystem.isTutorialComplete )
    {
      GameController.instance.SetCheckpoint(spawnPoint);
      ShouldPlayHint = false;
      return;
    }

    /*if( !GameSystem.playedCutScenes[0] && !isStartPlayFirstScene)
    {
      CutScenesManager.instance.PlayCutScene(0);
      isStartPlayFirstScene = true;
      return;
    }*/


    if ( ShouldPlayHint )
    {
     // CharacterControllerScript.instance.EnterHintState(hint);
      hint.ShowHint();
      isStartPlayFirstScene = false;
      ShouldPlayHint = false;
    }
	}

  void EnableTutorialsObjects()
  {
    for( int i = 0; i < keys.Count; i++ )
    {
      keys[i].SetActive(true);
    }
  }

  void DisableTutorialObjects()
  {
    meteor.SetActive(false);
    for (int i = 0; i < keys.Count; i++)
    {
      if (keys[i] == null)
        continue;
      keys[i].SetActive(false);
    }
  }

  //Заканчиваем обучение на 0 уровне, выключаем ненужные объекты, включаем нужные
  public void EndTutorial()
  {
    if (GameSystem.isTutorialComplete)
      return;
    DisableTutorialObjects();
    GameSystem.isTutorialComplete = true;
    GameSystem.availableLevel = 2;
    SaveDataManager.SaveGameData();
  }
}
