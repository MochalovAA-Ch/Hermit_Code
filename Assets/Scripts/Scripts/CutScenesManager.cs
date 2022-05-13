using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenesManager : MonoBehaviour {

  public static CutScenesManager instance;

 // public Camera MainCamera;
 // public Camera CutSceneCamera;
 // public GameObject gameUi;
 // public GameObject cutSceneUI;
  public List<PlayableDirector> cutScenes;
  int currCutSceneNum;

	// Use this for initialization
	void Start ()
  {
    instance = this;
	}
	
	// Update is called once per frame
	void Update ()
  {

    //cutScenes[0].p
  }

  public void PlayCutScene( int cutSceneNum )
  {
    currCutSceneNum = cutSceneNum;
    cutScenes[cutSceneNum].Play();//Play( "cutScene");
    GameSystem.playedCutScenes[cutSceneNum] = true;
  }

  public void StopCutScene()
  {
    cutScenes[currCutSceneNum].Stop();
  }
}
