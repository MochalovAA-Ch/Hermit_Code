using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveDataManager
{
  public static void SaveGameData()
  {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream stream = new FileStream(Application.persistentDataPath + "/player.save", FileMode.Create);
    GameData gameData = new GameData();
    gameData.SaveEnabledLists();
    gameData.SaveMaterialsList();
    bf.Serialize(stream, gameData);
    stream.Close();
  }

  public static void LoadPlayer()
  {
    if( File.Exists(Application.persistentDataPath + "/player.save") )
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream stream = new FileStream(Application.persistentDataPath + "/player.save", FileMode.Open);
      GameData gameData = bf.Deserialize(stream) as GameData;
      gameData.LoadEnabledList();
      gameData.LoadMaterialsList();

      GameSystem.isTutorialComplete = gameData.isTutorialCompleted;
      GameSystem.availableLevel = gameData.availableLevel;
      GameSystem.totalCoins = gameData.coins;
      GameSystem.hairs = gameData.hairsSet;
      GameSystem.eyes = gameData.eyesSet;
      GameSystem.beards = gameData.beardsSet;
      GameSystem.ears = gameData.earsSet;
      GameSystem.tshirts = gameData.tshirtsSet;
      GameSystem.coats = gameData.coatsSet;
      GameSystem.pants = gameData.pantsSet;
      GameSystem.boots = gameData.bootsSet;
      GameSystem.weapons = gameData.weaponsSet;
      GameSystem.cameraSensetive = gameData.cameraSensetive;
      GameSystem.isDataLoaded = true;
      GameSystem.isFreeCam = gameData.isFreeCam;
      GameSystem.isHomyAvaible = gameData.isHomyEnabled;
      GameSystem.language = gameData.language;
      GameSystem.isSoundEnabled = gameData.isSoundEnabled;
      if( GameSystem.isTutorialComplete )
      {
        GameSystem.shouldHomeLevelEndLevelMusicPlay = false;
      }
      else
      {
        GameSystem.shouldHomeLevelEndLevelMusicPlay = true;
      }
    }
  }
}

[System.Serializable]
public class GameData
{
  public int availableLevel;
  public bool isTutorialCompleted;
  public int coins;
  public int hairsSet;
  public int eyesSet;
  public int beardsSet;
  public int earsSet;
  public int tshirtsSet;
  public int coatsSet;
  public int pantsSet;
  public int bootsSet;
  public int weaponsSet;
  public bool isFreeCam;
  public bool isHomyEnabled;
  public bool isMute;
  public float cameraSensetive;

  public int language;

  public bool[] enabledHairs;
  public bool[] enabledEyes;
  public bool[] enabledBeards;
  public bool[] enabledEars;
  public bool[] enabledtShirts;
  public bool[] enabledCoats;
  public bool[] enabledPants;
  public bool[] enabledBoots;
  public bool[] enabledWeapons;
  public bool[] playedCutScenes;
  public bool[] playedHints;

  //Стандартные цвета материала
  public float[] hairs_main_mat;
  public float[] hairs_wood_mat;
  public float[] hairs_leafs_mat;
  public float[] hairs_acc_mat;

  public float[] eyes_1_mat;
  public float[] eyes_2_mat;
  public float[] eyes_3_mat;
  public float[] glases_mat;
  public float[] glasses_lens_mat;

  public float[] beards_main_mat;
  public float[] beards_acc_mat;

  public float[] ears_acc_mat;

  public float[] horns_main_mat;

  public float[] tshirts_main_mat;
  public float[] tshirts_colar_mat;

  public float[] coats_main_mat;
  public float[] coats_colars_mat;
  public float[] coats_acc_mat;
  public float[] neck_main_mat;

  public float[] pants_main_mat;
  public float[] body_leafs_mat;
  public float[] skin;
  public float[] leg_acc_mat;

  public float[] hat_material;
  public float[] hats_jewel_mat;
  public float[] boots_main_mat;

  public float[] arms_main_mat;

  public bool isSoundEnabled;
  public GameData()
  {
    language = GameSystem.language;
    isTutorialCompleted = GameSystem.isTutorialComplete;
    availableLevel = GameSystem.availableLevel;
    coins = GameSystem.totalCoins;
    hairsSet = GameSystem.hairs;
    eyesSet = GameSystem.eyes;
    beardsSet = GameSystem.beards;
    earsSet = GameSystem.ears;
    tshirtsSet = GameSystem.tshirts;
    coatsSet = GameSystem.coats;
    pantsSet = GameSystem.pants;
    bootsSet = GameSystem.boots;
    weaponsSet = GameSystem.weapons;

    cameraSensetive = GameSystem.cameraSensetive;
    isFreeCam = GameSystem.isFreeCam;
    isHomyEnabled = GameSystem.isHomyAvaible;
    enabledHairs = new bool[GameSystem.enabledHairs.Count];
    enabledEyes = new bool[GameSystem.enabledEyes.Count];
    enabledBeards = new bool[GameSystem.enabledBeards.Count];
    enabledEars = new bool[GameSystem.enabledEars.Count];
    enabledtShirts = new bool[GameSystem.enabledTShirts.Count];
    enabledCoats = new bool[GameSystem.enabledCoats.Count];
    enabledPants = new bool[GameSystem.enabledPants.Count];
    enabledBoots = new bool[GameSystem.enabledBoots.Count];
    enabledWeapons = new bool[GameSystem.enabledWeapons.Count];
    playedCutScenes = new bool[GameSystem.cutScenesCount];
    playedHints = new bool[GameSystem.playedHintsCount];

    hairs_main_mat = new float[3];
    hairs_wood_mat = new float[3];
    hairs_leafs_mat = new float[3];
    hairs_acc_mat = new float[3];

    eyes_1_mat = new float[3];
    eyes_2_mat = new float[3];
    eyes_3_mat = new float[3];
    glases_mat = new float[3];
    glasses_lens_mat = new float[3];
    beards_main_mat = new float[3];
    beards_acc_mat = new float[3];
    ears_acc_mat = new float[3];
    horns_main_mat = new float[3];
    tshirts_main_mat = new float[3];
    tshirts_colar_mat = new float[3];
    coats_main_mat = new float[3];
    coats_colars_mat = new float[3];
    coats_acc_mat = new float[3];
    neck_main_mat = new float[3];

    pants_main_mat = new float[3];
    body_leafs_mat = new float[3];
    skin = new float[3];
    leg_acc_mat = new float[3];
    hat_material = new float[3];
    hats_jewel_mat = new float[3];
    boots_main_mat = new float[3];

    arms_main_mat = new float[3];

    isSoundEnabled = GameSystem.isSoundEnabled;
  }

  public void SaveEnabledLists()
  {
    saveEnabledList( enabledHairs, GameSystem.enabledHairs );
    saveEnabledList( enabledEyes, GameSystem.enabledEyes );
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList(enabledEyes, GameSystem.enabledEyes);
    saveEnabledList( playedHints, GameSystem.playedHints );
  }

  void saveEnabledList( bool[] gameDataList, List<bool> gameSystemList )
  {
    for( int i = 0; i < gameDataList.Length; i++ )
    {
      gameDataList[i] = gameSystemList[i];
    }
  }

  public void LoadEnabledList()
  {
    loadEnabledList( enabledHairs, GameSystem.enabledHairs );
    loadEnabledList( enabledEyes, GameSystem.enabledEyes);
    loadEnabledList( enabledBeards, GameSystem.enabledBeards);
    loadEnabledList( enabledEars, GameSystem.enabledEars);
    loadEnabledList( enabledtShirts, GameSystem.enabledTShirts);
    loadEnabledList( enabledCoats, GameSystem.enabledCoats);
    loadEnabledList(enabledPants, GameSystem.enabledPants);
    loadEnabledList(enabledBoots, GameSystem.enabledBoots);
    loadEnabledList(enabledWeapons, GameSystem.enabledWeapons);
    loadEnabledList( playedCutScenes, GameSystem.playedCutScenes );
    loadEnabledList( playedHints, GameSystem.playedHints );
  }

  public void LoadMaterialsList()
  {
    loadMaterialList(hairs_main_mat, GameSystem.hairs_main_mat);
    loadMaterialList(hairs_wood_mat, GameSystem.hairs_wood_mat);
    loadMaterialList(hairs_leafs_mat, GameSystem.hairs_leafs_mat);
    loadMaterialList(hairs_acc_mat, GameSystem.hairs_acc_mat);
    loadMaterialList(eyes_1_mat, GameSystem.eyes_1_mat);
    loadMaterialList(eyes_2_mat, GameSystem.eyes_2_mat);
    loadMaterialList(eyes_3_mat, GameSystem.eyes_3_mat);
    loadMaterialList(glases_mat, GameSystem.glases_mat);
    loadMaterialList(glasses_lens_mat, GameSystem.glasses_lens_mat);
    loadMaterialList(beards_main_mat, GameSystem.beards_main_mat);
    loadMaterialList(beards_acc_mat, GameSystem.beards_acc_mat);
    loadMaterialList(ears_acc_mat, GameSystem.ears_acc_mat);
    loadMaterialList(horns_main_mat, GameSystem.horns_main_mat);
    loadMaterialList(tshirts_main_mat, GameSystem.tshirts_main_mat);
    loadMaterialList(tshirts_colar_mat, GameSystem.tshirts_colar_mat);
    loadMaterialList(coats_main_mat, GameSystem.coats_main_mat);
    loadMaterialList(coats_colars_mat, GameSystem.coats_colars_mat);
    loadMaterialList(coats_acc_mat, GameSystem.coats_acc_mat);
    loadMaterialList(neck_main_mat, GameSystem.neck_main_mat);
    loadMaterialList(pants_main_mat, GameSystem.pants_main_mat);
    loadMaterialList(body_leafs_mat, GameSystem.body_leafs_mat);
    loadMaterialList(skin, GameSystem.skin);
    loadMaterialList(hat_material, GameSystem.hat_material);
    loadMaterialList(hats_jewel_mat, GameSystem.hats_jewel_mat);
    loadMaterialList(boots_main_mat, GameSystem.boots_main_mat);
    loadMaterialList(arms_main_mat, GameSystem.arms_main_mat);
  }

  public void SaveMaterialsList()
  {
    saveMaterialList(hairs_main_mat, GameSystem.hairs_main_mat);
    saveMaterialList(hairs_wood_mat, GameSystem.hairs_wood_mat);
    saveMaterialList(hairs_leafs_mat, GameSystem.hairs_leafs_mat);
    saveMaterialList(hairs_acc_mat, GameSystem.hairs_acc_mat);
    saveMaterialList(eyes_1_mat, GameSystem.eyes_1_mat);
    saveMaterialList(eyes_2_mat, GameSystem.eyes_2_mat);
    saveMaterialList(eyes_3_mat, GameSystem.eyes_3_mat);
    saveMaterialList(glases_mat, GameSystem.glases_mat);
    saveMaterialList(glasses_lens_mat, GameSystem.glasses_lens_mat);
    saveMaterialList(beards_main_mat, GameSystem.beards_main_mat);
    saveMaterialList(beards_acc_mat, GameSystem.beards_acc_mat);
    saveMaterialList(ears_acc_mat, GameSystem.ears_acc_mat);
    saveMaterialList(horns_main_mat, GameSystem.horns_main_mat);
    saveMaterialList(tshirts_main_mat, GameSystem.tshirts_main_mat);
    saveMaterialList(tshirts_colar_mat, GameSystem.tshirts_colar_mat);
    saveMaterialList(coats_main_mat, GameSystem.coats_main_mat);
    saveMaterialList(coats_colars_mat, GameSystem.coats_colars_mat);
    saveMaterialList(coats_acc_mat, GameSystem.coats_acc_mat);
    saveMaterialList(neck_main_mat, GameSystem.neck_main_mat);
    saveMaterialList(pants_main_mat, GameSystem.pants_main_mat);
    saveMaterialList(body_leafs_mat, GameSystem.body_leafs_mat);
    saveMaterialList(skin, GameSystem.skin);
    saveMaterialList(hat_material, GameSystem.hat_material);
    saveMaterialList(hats_jewel_mat, GameSystem.hats_jewel_mat);
    saveMaterialList(boots_main_mat, GameSystem.boots_main_mat);
    saveMaterialList(arms_main_mat, GameSystem.arms_main_mat);
  }

  void loadEnabledList(bool[] gameDataList, List<bool> gameSystemList)
  {
    for (int i = 0; i < gameDataList.Length; i++)
    {
      gameSystemList[i] = gameDataList[i];
    }
  }

  void loadEnabledList(bool[] gameDataList, bool[] gameSystemList)
  {
    for (int i = 0; i < gameDataList.Length; i++)
    {
      gameSystemList[i] = gameDataList[i];
    }
  }

  void saveMaterialList( float[] gameDataList, float[] gameSystemList )
  {
    for  (int i = 0; i < gameSystemList.Length; i++)
    {
      gameDataList[i] = gameSystemList[i];
    }
  }

  void loadMaterialList(float[] gameDataList, float[] gameSystemList)
  {
    if( gameDataList == null )
    {
      gameDataList = new float[3];
      return;
    }

    if ( gameDataList.Length == 0 )
      return;
    for (int i = 0; i < gameDataList.Length; i++)
    {
      gameSystemList[i] = gameDataList[i];
    }
  }
}
