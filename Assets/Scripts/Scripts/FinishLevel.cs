using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag=="Player" )
    {
      if( GameSystem.playerKeys >= 5 )
      {
        // По завершению уровня вызываем окно статистики
        LevelStatisticsScript.instance.ShowEndLevelStaistics();
        /*GameSystem.totalCoins += GameSystem.collectedCoinsOnLevel;
        GameSystem.currentLevel++;
        if( GameSystem.availableLevel < GameSystem.currentLevel )
          GameSystem.availableLevel = GameSystem.currentLevel;
        SaveDataManager.SaveGameData();
        SceneLoader.instance.LoadLevel(GameSystem.currentLevel);*/
      }
    }
  }
}
