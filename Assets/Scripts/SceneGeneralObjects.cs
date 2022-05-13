using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGeneralObjects : MonoBehaviour {

  public static SceneGeneralObjects instance;

  public CharacterControllerScript charController;
  public Transform playerTr;
  public Transform mainCamera;
  public ThirdPersonOrbitCam thirdPersonCameraScript;
	// Use this for initialization
	void Start ()
  {
    instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
