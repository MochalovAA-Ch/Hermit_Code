using System.Collections;
using System.Collections.Generic;


public enum Keys
{
  AETHER, WATER, FIRE, GROUND, AIR
}

public static class GameSystem  {
  public static int currentLevel;
  public static int availableLevel;
  public static int totalLevels;
  public static int language;    // 0 - русский, 1 - английский ( по умолчанию)
  public static bool isSoundEnabled;
  public static bool isMusicEnabled;
  public static bool isGoogleAdsInit;

  public static bool isHomyInThrow;
  public static int playedHintsCount;
  static GameSystem()
  {
    isGoogleAdsInit = false;
    isSoundEnabled = true;
    isMusicEnabled = true;
    withJoystick = true;
    language = 0;   //по умолчанию русский
    isTutorialComplete = false;
    totalLevels = 6;
    currentLevel = 1;   //По умолчанию, базовый уровень
    availableLevel = 1; //По умолчанию, когда не прошли туториал, доступен только базовый уровень
    playerLives = 3;
    collectedCoinsOnLevel = 0;
    cameraSensetive = 0.1f;
    playerKeys = 0;
    playerCanBeHitted = true;
    playerCanRestoreLives = true;
    invulnerableTime = 4.0f;
    currentUnvulnerableTime = 0.0f;
    restoreLivesTime = 3.0f;
    currentRestoreLivesTime = 0.0f;
    coinsOnLevel = 0;
    hairs = 0;
    eyes = 0;
    beards = 0;
    ears = 0;
    tshirts = 0;
    coats = 0;
    pants = 0;
    boots = 0;
    weapons = 0;
    isDataLoaded = false;
    isFreeCam = false;

    shouldHomeLevelEndLevelMusicPlay = true;

    currentHintId = 0;
    QuestIconClicked = 0;

    hairsPrices = new List<int>();
    eyesPrices = new List<int>();
    beardsPrices = new List<int>();
    earsPrices = new List<int>();
    hornsPrices = new List<int>();
    tShirtsPrices = new List<int>();
    coatsPrices = new List<int>();
    pantsPrices = new List<int>();
    necksPrices = new List<int>();
    bootsPrices = new List<int>();
    weaponsPrices = new List<int>();
    setClothesPrices(hairsPrices,
      0,   100, 100, 100, 100,
      100, 100, 100, 100, 100,          
      100, 100, 100, 100, 100,
      100, 100, 100, 100, 100,
      100, 100, 100, 100
      );
    setClothesPrices(eyesPrices,
      0, 100, 100, 100, 100
      );
    setClothesPrices(beardsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100, 100, 100
      );
    setClothesPrices(earsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100, 100, 100,
      100, 100, 100, 100, 100,
      100, 100, 100, 100, 100,
      100
      );
    setClothesPrices(hornsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100, 100, 100,
      100, 100
      );
    setClothesPrices(tShirtsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100
      );
    setClothesPrices(coatsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100, 100
      );
    setClothesPrices(necksPrices,
      0, 100, 100, 100
      );
    setClothesPrices(pantsPrices,
      0, 100, 100, 100, 100
      );
    setClothesPrices(bootsPrices,
      0, 100, 100, 100, 100,
      100, 100, 100, 100, 100
      );
    setClothesPrices(weaponsPrices,
     0, 100, 100
     );
    setClothesPrices(weaponsPrices,
     0, 100, 100, 100, 100
     );

    playedCutScenes = new bool[] { false };
    playedHintsCount = 100;

    enabledHairs = new List<bool>();
    enabledEyes = new List<bool>();
    enabledBeards = new List<bool>();
    enabledEars = new List<bool>();
    enabledHorns = new List<bool>();
    enabledTShirts = new List<bool>();
    enabledCoats = new List<bool>();
    enabledNecks = new List<bool>();
    enabledPants = new List<bool>();
    enabledBoots = new List<bool>();
    enabledWeapons = new List<bool>();
    enabledArms = new List<bool>();

    createEnabledClothesList(enabledHairs, 24);
    createEnabledClothesList(enabledEyes, 5);
    createEnabledClothesList(enabledBeards, 10);
    createEnabledClothesList(enabledEars, 21);
    createEnabledClothesList(enabledHorns, 12);
    createEnabledClothesList(enabledTShirts, 8);
    createEnabledClothesList(enabledCoats, 9);
    createEnabledClothesList(enabledNecks, 4);
    createEnabledClothesList(enabledPants, 5);
    createEnabledClothesList(enabledBoots, 10);
    createEnabledClothesList(enabledWeapons, 3);
    createEnabledClothesList(enabledArms, 5);

    hairs_main_mat = new float[]{ 154.0f, 154.0f, 154.0f };
    hairs_wood_mat = new float[]{ 90.0f, 72.0f, 34.0f };
    hairs_leafs_mat = new float[] { 71.0f, 125.0f, 34.0f };
    hairs_acc_mat = new float[] { 188.0f, 100.0f, 163.0f };

    eyes_1_mat = new float[] { 255.0f, 255.0f, 255.0f };
    eyes_2_mat = new float[] { 114.0f, 162.0f, 209.0f };
    eyes_3_mat = new float[] { 0.0f, 0.0f, 0.0f };

    glases_mat = new float[] { 0.0f, 0.0f, 0.0f };
    glasses_lens_mat = new float[] { 139.0f, 209.0f, 171.0f };

    beards_main_mat = new float[] { 154.0f, 154.0f, 154.0f };
    beards_acc_mat = new float[] { 188.0f, 100.0f, 163.0f };

    ears_acc_mat = new float[] { 255.0f, 252.0f, 182.0f };

    horns_main_mat = new float[] { 118.0f, 64.0f, 33.0f };

    tshirts_main_mat = new float[] { 128.0f, 0.0f, 255.0f };
    tshirts_colar_mat = new float[] { 192.0f, 108.0f, 0.0f };

    coats_main_mat = new float[] { 139.0f, 209.0f, 171.0f };
    coats_colars_mat = new float[] { 0.0f, 64.0f, 0.0f };
    coats_acc_mat = new float[] { 0.0f, 0.0f, 0.0f };

    neck_main_mat = new float[] { 239.0f, 134.0f, 0.0f };

    pants_main_mat = new float[] { 147.0f, 125.0f, 125.0f };

    hats_jewel_mat = new float[] { 215.0f, 214.0f, 45.0f };
    hat_material = new float[] { 55.0f, 56.0f, 180.0f };
    leg_acc_mat = new float[] { 0.0f, 0.0f, 0.0f };
    body_leafs_mat = new float[] { 71.0f, 125.0f, 34.0f };
    boots_main_mat = new float[] { 80.0f, 71.0f, 69.0f };

    skin = new float[] { 180.0f, 170.0f, 94.0f };

    arms_main_mat = new float[] { 139.0f, 209.0f, 171.0f };

    isHomyAvaible = true;

    playedHints = new List<bool>(playedHintsCount);
    for( int i = 0; i < playedHintsCount; i++ )
    {
      playedHints.Add(false);
    }
  }

  public static bool isTutorialComplete;
  public static bool isHomyAvaible;
  public static bool isAddShowed;
  public static bool isAdClosed;
  public static bool diedInGameZone;

  //Количество жизней игрока
  public static int playerLives;

  public static bool isDataLoaded;

  //Количество собранных ключей
  public static int playerKeys;

  public static int QuestIconClicked;

  //Можем ли ударить игрока
  public static bool playerCanBeHitted;

  //Можем ли восстанавлвать жизни
  public static bool playerCanRestoreLives;

  //Время до возможности следующего удара
  public static float invulnerableTime;

  //Текущее время в неуязвимости
  public static float currentUnvulnerableTime;

  //Время восстановления одной жизни
  public static float restoreLivesTime;

  public static float cameraSensetive;
  public static bool isFreeCam;
  public static bool withJoystick;

  public static List<bool> enabledHairs;
  public static List<bool> enabledEyes;
  public static List<bool> enabledBeards;
  public static List<bool> enabledEars;
  public static List<bool> enabledHorns;
  public static List<bool> enabledTShirts;
  public static List<bool> enabledCoats;
  public static List<bool> enabledNecks;
  public static List<bool> enabledPants;
  public static List<bool> enabledBoots;
  public static List<bool> enabledWeapons;
  public static List<bool> enabledArms;

  public static List<int> hairsPrices;
  public static List<int> eyesPrices;
  public static List<int> beardsPrices;
  public static List<int> earsPrices;
  public static List<int> hornsPrices;
  public static List<int> tShirtsPrices;
  public static List<int> coatsPrices;
  public static List<int> pantsPrices;
  public static List<int> necksPrices;
  public static List<int> bootsPrices;
  public static List<int> weaponsPrices;
  public static List<int> armsPrices;

  public static int cutScenesCount = 1;

  //Текущее время восстановления жизни
  public static float currentRestoreLivesTime;

  public static int coinsOnLevel;
  public static int collectedCoinsOnLevel;
  //Сохраняемые данные 
  public static int totalCoins;

  //Одежда персонажа
  public static int hairs;
  public static int eyes;
  public static int beards;
  public static int ears;
  public static int horns;
  public static int tshirts;
  public static int coats;
  public static int necks;
  public static int pants;
  public static int boots;
  public static int weapons;
  public static int arms;

  //Стандартные цвета материала
  public static float[] beards_acc_mat;
  public static float[] beards_main_mat;

  public static float[] boots_main_mat;

  public static float[] body_leafs_mat;

  public static float[] coats_acc_mat;
  public static float[] coats_colars_mat;
  public static float[] coats_main_mat;

  public static float[] ears_acc_mat;

  public static float[] eyes_1_mat;
  public static float[] eyes_2_mat;
  public static float[] eyes_3_mat;
  public static float[] glases_mat;
  public static float[] glasses_lens_mat;

  public static float[] hairs_main_mat;
  public static float[] hairs_wood_mat;
  public static float[] hairs_leafs_mat;
  public static float[] hairs_acc_mat;

  public static float[] hat_material;
  public static float[] hats_jewel_mat;

  public static float[] horns_main_mat;

  public static float[] leg_acc_mat;

  public static float[] neck_main_mat;

  public static float[] pants_main_mat;

  public static float[] skin;

  public static float[] tshirts_main_mat;
  public static float[] tshirts_colar_mat;

  public static float[] arms_main_mat;


  public static bool[] playedCutScenes;
  public static List<bool> playedHints;

  public static Keys collectedKeyIndex;

  public static int currentHintId;

  public static bool shouldHomeLevelEndLevelMusicPlay;

  static void setEnabledClothesList( List<bool> enabledList, params int[] values)
  {
    for ( int i = 0; i < values.Length; i++ )
    {
      enabledList[i] =  values[i] == 0 ? false : true;
    }
  }

  static void createEnabledClothesList(List<bool> enabledList, int count )
  {
    for ( var i = 0; i < count; i++ )
    {
      if( i == 0 )
      {
        enabledList.Add(true);
      }
      else
      {
        enabledList.Add(false);
      }
    }
  }

  static void setClothesPrices( List<int> prices, params int[] values )
  {
    for (int i = 0; i < values.Length; i++)
    {
      prices.Add( values[i] );
    }
  }
}
