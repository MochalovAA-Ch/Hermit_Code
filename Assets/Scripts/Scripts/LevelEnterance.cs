using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnterance : MonoBehaviour {

  //Номер уровня, в который входим
  public int LevelNum;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


  private void OnTriggerEnter(Collider other)
  {
    GameSystem.currentLevel = LevelNum;
    SceneLoader.instance.LoadLevel(LevelNum);
  }
}
