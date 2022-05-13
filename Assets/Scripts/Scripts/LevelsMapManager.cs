using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMapManager : MonoBehaviour {

  //Отсчет номера сцены уровня начинается с 2 ( 0 - главное меню, 1 - базовый уровень )
  public GameObject settingMap;
  public int settingNum;
  public Button PlayBtn;
  //public List<Button> levelsBtn;
  public Image levelImage;
  public List<Sprite> levelsList;
  public Text levelText;
  int levelNum;
  int avaibleLevel;
  int currentSelecedLevel;

	// Use this for initialization
	void Start ()
  {
   /* for( int i = 0; i <  levelsBtn.Count; i++ )
    {
      if (settingNum * levelsBtn.Count + i + 2 > GameSystem.availableLevel)
        levelsBtn[i].interactable = false;
      else
        levelsBtn[i].interactable = true;
    }*/
	}

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.SHOW_NEXT_LEVEL, ShowNextLvl);
    EventsManager.StartListening(EventsIds.SHOW_PREV_LEVEL, ShowPrevLvl);
    EventsManager.StartListening(EventsIds.PLAY_LEVEL, PlayLevel);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.SHOW_NEXT_LEVEL, ShowNextLvl);
    EventsManager.StopListening(EventsIds.SHOW_PREV_LEVEL, ShowPrevLvl);
    EventsManager.StopListening(EventsIds.PLAY_LEVEL, PlayLevel);
  }

  // Update is called once per frame
  void Update ()
  {
		
	}

  bool isPlayed = false;
  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      if( GameSystem.currentLevel == 1 )
      {
        if( !isPlayed  && GameSystem.shouldHomeLevelEndLevelMusicPlay )
        {
          InterfaceSoundsManager.instance.PlayLevelComplete();
          isPlayed = true;
          GameSystem.shouldHomeLevelEndLevelMusicPlay = false;
        }
      }
      else
      {
        if (!isPlayed)
        InterfaceSoundsManager.instance.PlayLevelComplete();
        isPlayed = true;
      }
      ShowLevelMap();
    }
  }

  public void ShowLevelMap()
  {
    HermitSoundManager.instance.StopSound();
    GameUIController.instance.ShowLevelMap();
    CharacterControllerScript.instance.EnterStopState();
    avaibleLevel = ( GameSystem.availableLevel - 2) > levelsList.Count ? levelsList.Count -1 : ( GameSystem.availableLevel - 2);
    Color color = levelImage.color;
    color.a = 1.0f;
    levelImage.color = color;
    levelImage.sprite = levelsList[avaibleLevel];
    currentSelecedLevel = avaibleLevel;
    levelText.text = (avaibleLevel + 1).ToString() + "/" + (levelsList.Count).ToString();
  }

  public void ShowNextLvl()
  {
    if( currentSelecedLevel == levelsList.Count - 1)
    {
      currentSelecedLevel = 0;
    }
    else
    {
      currentSelecedLevel++;
    }

    Color color = levelImage.color;
    if (currentSelecedLevel > avaibleLevel)
    {
      color.a = 0.1f;
      PlayBtn.interactable = false;
    }
    else
    {
      color.a = 1.0f;
      PlayBtn.interactable = true;
    }
    levelImage.color = color;
    levelImage.sprite = levelsList[currentSelecedLevel];
    levelText.text = (currentSelecedLevel + 1).ToString() + "/" + (levelsList.Count).ToString();
  }

  public void ShowPrevLvl()
  {
    if (currentSelecedLevel == 0)
    {
      currentSelecedLevel = levelsList.Count - 1;
    }
    else
    {
      currentSelecedLevel--;
    }
  
    Color color = levelImage.color;
    if (currentSelecedLevel > avaibleLevel)
    {
      color.a = 0.1f;
      PlayBtn.interactable = false;
    }
    else
    {
      color.a = 1.0f;
      PlayBtn.interactable = true;
    }
    levelImage.color = color;
    levelImage.sprite = levelsList[currentSelecedLevel];
    levelText.text = (currentSelecedLevel + 1).ToString() + "/" + (levelsList.Count).ToString();
  }

  public void PlayLevel()
  {
    GameSystem.currentLevel = currentSelecedLevel + 2;
    SceneLoader.instance.LoadLevel(GameSystem.currentLevel);
  }
}
