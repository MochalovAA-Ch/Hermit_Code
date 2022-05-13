using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;


public class GameUIController : MonoBehaviour {

  public static GameUIController instance;
  //public static 
  public GoogleAdsScript googleAds;
  public Text healthText;
  public Text coinsText;
  public Text DialogText;
  public Text QuestDialogText;
  public Text QuestGiverNameText;
  public Text FloatingText;
  public Text QuestNameText;
  public Text QuestShortDescText;
  public Text QuestDescriptionText;
  public Text HintText;

  public Image hitButtonImage;
  public Image interractBtnImage;
  public Sprite hitSprite;
  public Sprite TalkSprite;
  public Sprite InterractSprite;
  public Image musicImage;
  public Sprite musicOffSprite;
  public Sprite musicOnSprite;
  public Image soundsImage;
  public Sprite soundsOffSprite;
  public Sprite soundsOnSprite;

  public GameObject GameOverMenu;
  public GameObject GameMenu;
  public GameObject Options;
  public GameObject QuestInformation;
  public GameObject DialogCanvas;
  public GameObject QuestDialogCanvas;
  public GameObject GameUICanvas;
  public GameObject OpenMenuBtn;
  public GameObject HintCanvas;
  public GameObject SettingMap;
  public GameObject HomyThrowBtn;
  public GameObject WardrobeCanvas;

  public Toggle isDefaultCam;
  public Toggle isFreeCam;
  public Slider camSensetiveSlider;

  public GameObject pauseImage;
  public GameObject playImage;

  public Button ReviveButton;
  public Button[] MainMenuButtons;
  public Button[] RestartButtons;

  public GameObject gameUiKeyArea;

  public List<GameObject> keysAreaList;

  public List<Image> keyAetherImagesList;
  public List<Image> keyWaterImagesList;
  public List<Image> keyFireImagesList;
  public List<Image> keyGroundImagesList;
  public List<Image> keyAirImagesList;

  public AudioMixerSnapshot SndOnMscOn;
  public AudioMixerSnapshot SndOnMscOff;
  public AudioMixerSnapshot SndOffMscOn;
  public AudioMixerSnapshot SndOffMscOff;


  public Image questInfoIcon;
  Image interractSpriteImage;

  bool isMenuOpened;

  public WadrobeScript wadrobe;
  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.INCREASE_LIVES, SetHealthText);
    EventsManager.StartListening(EventsIds.DECREASE_LIVES, SetHealthText);
    EventsManager.StartListening(EventsIds.CHANGE_COINS_COUNT, SetCoinsText);
    EventsManager.StartListening(EventsIds.CHANGE_KEYS_COUNT, ShowCollectedkeys);
    EventsManager.StartListening(EventsIds.GAME_OVER, ShowGameOverMenu);
    EventsManager.StartListening(EventsIds.CLOSE_REWARD_VIDEO, ReviveForVideo);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.INCREASE_LIVES, SetHealthText);
    EventsManager.StopListening(EventsIds.DECREASE_LIVES, SetHealthText);
    EventsManager.StopListening(EventsIds.CHANGE_COINS_COUNT, SetCoinsText);
    EventsManager.StopListening(EventsIds.CHANGE_KEYS_COUNT, ShowCollectedkeys);
    EventsManager.StopListening(EventsIds.GAME_OVER, ShowGameOverMenu);
    EventsManager.StopListening(EventsIds.CLOSE_REWARD_VIDEO, ReviveForVideo);
  }

  // Use this for initialization
  void Start()
  {
    GameSystem.collectedCoinsOnLevel = 0;
    GameSystem.playerLives = 3;
    interractSpriteImage = interractBtnImage.transform.GetChild(0).GetComponent<Image>();
    if (GameSystem.currentLevel == 1 && GameSystem.isTutorialComplete)
    {
      for (int i = 0; i < keysAreaList.Count; i++)
      {
        keysAreaList[i].gameObject.SetActive(false);
      }
    }


    /* for( int i = 0; i < keyAetherImagesList.Count; i++ )
     {
       keyAetherImagesList[i].rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
     }

     for (int i = 0; i < keyWaterImagesList.Count; i++)
     {
       keyWaterImagesList[i].rectTransform.anchoredPosition = new Vector2(-100.0f, 0.0f);
     }

     for (int i = 0; i < keyFireImagesList.Count; i++)
     {
       keyFireImagesList[i].rectTransform.anchoredPosition = new Vector2(-200.0f, 0.0f);
     }

     for (int i = 0; i < keyGroundImagesList.Count; i++)
     {
       keyGroundImagesList[i].rectTransform.anchoredPosition = new Vector2(100.0f, 0.0f);
     }

     for (int i = 0; i < keyAirImagesList.Count; i++)
     {
       keyAirImagesList[i].rectTransform.anchoredPosition = new Vector2(200.0f, 0.0f);
     }*/






    instance = this;
    SetHealthText();
    SetCoinsText();
    SetSoundImage();
    SetMusicImage();
    Time.timeScale = 1.0f;
    for (int i = 0; i < MainMenuButtons.Length; i++)
    {
      MainMenuButtons[i].onClick.AddListener(delegate { InterfaceSoundsManager.instance.PlayBtnClip(); SceneLoader.instance.LoadLevel(0); });
    }

    for (int i = 0; i < RestartButtons.Length; i++)
    {
      RestartButtons[i].onClick.AddListener(RestartLevel);
    }

    if( GameSystem.isFreeCam )
    {
      isFreeCam.isOn = true;
    }
    else
    {
      isDefaultCam.isOn = false;
    }

    camSensetiveSlider.value = GameSystem.cameraSensetive;

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R))
    {
      ReviveForVideo();
    }

    //Если закрыли видео, значит был gameover
    if (GameSystem.isAdClosed)
    {
      ReviveForVideo();
      GameSystem.isAdClosed = false;
    }

  }


  public void GoToMenu()
  {
    SceneLoader.instance.LoadLevel(0);
  }
  //
  public void SetHealthText()
  {
    healthText.text =  GameSystem.playerLives.ToString();
  }

  public void SetCoinsText()
  {
    if( GameSystem.currentLevel != 1 )
    {
      coinsText.text = GameSystem.collectedCoinsOnLevel.ToString();
    }
    else
    {
      coinsText.text = GameSystem.totalCoins.ToString();
    }
  }

  public void ShowCollectedkeys()
  {
    gameUiKeyArea.SetActive(true);
    if( GameSystem.collectedKeyIndex == Keys.AETHER )
    {
      SetKeyImagesViewState(keyAetherImagesList, true);
    }
    else if ( GameSystem.collectedKeyIndex == Keys.WATER )
    {
      SetKeyImagesViewState(keyWaterImagesList, true);
    }

    else if (GameSystem.collectedKeyIndex == Keys.FIRE)
    {
      SetKeyImagesViewState(keyFireImagesList, true);
    }

    else if (GameSystem.collectedKeyIndex == Keys.GROUND)
    {
      SetKeyImagesViewState(keyGroundImagesList, true);
    }

    else if (GameSystem.collectedKeyIndex == Keys.AIR)
    {
      SetKeyImagesViewState(keyAirImagesList, true);
    }

    StartCoroutine(showKeysTemp());
  }

  float keyImgTimer;
  float keyImgTime = 1.0f;
  IEnumerator showKeysTemp( )
  {
    keyImgTimer = 0.0f;
    while ( keyImgTimer < keyImgTime )
    {
      keyImgTimer += Time.deltaTime;
      yield return null;
    }
    gameUiKeyArea.SetActive(false);
  }

  public void ShowGameOverMenu()
  {
    InterfaceSoundsManager.instance.PlayGameOver();
    OpenMenuBtn.SetActive(false);
    Time.timeScale = 0.0f;
    GameOverMenu.gameObject.SetActive(true);
    if( googleAds.rewardBasedVideo.IsLoaded() )
    {
      ReviveButton.interactable = true;
    }
    else
    {
      ReviveButton.interactable = false;
    }

  }

  public void RestartLevel()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    SceneLoader.instance.LoadLevel( GameSystem.currentLevel );
    GameSystem.playerLives = 3;
    GameSystem.collectedCoinsOnLevel = 0;
  }

  public void ShowGameMenu()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    if ( !isMenuOpened )
    {
      Time.timeScale = 0.0f;
      pauseImage.SetActive(false);
      playImage.SetActive(true);
      GameMenu.gameObject.SetActive(true);
      Options.SetActive(false);
      isMenuOpened = true;
    }
    else
    {
      Time.timeScale = 1.0f;
      Options.SetActive(false);
      pauseImage.SetActive(true);
      playImage.SetActive(false);
      GameMenu.gameObject.SetActive(false);
      isMenuOpened = false;
    }
  }

  public void CloseGameMenu()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    isMenuOpened = false;
    Time.timeScale = 1.0f;
    playImage.SetActive(false);
    pauseImage.SetActive(true);
    GameMenu.gameObject.SetActive(false);
  }

  public void GoHome()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    GameSystem.playerLives = 3;
    GameSystem.currentLevel = 1;
    SceneLoader.instance.LoadLevel(1);
  }

  public void ShowOptions()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    Options.SetActive(true);
    GameMenu.gameObject.SetActive(false);
  }

  public void CloseOptions()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    SaveDataManager.SaveGameData();
    Options.SetActive(false);
    GameMenu.gameObject.SetActive(true);
  }

  public void SetDefaultCam()
  {
    GameSystem.isFreeCam = false;
  }

  public void SetFreeCam()
  {
    GameSystem.isFreeCam = true;
  }
  
  public void ChangeCameraSmoothnes()
  {
    GameSystem.cameraSensetive = camSensetiveSlider.value;
    CameraLook.smoothness = camSensetiveSlider.value;
  }

  public void CloseQuestInformation()
  {
    Time.timeScale = 1.0f;
    QuestInformation.SetActive(false);
  }

  public void ShowQuestDialog(  string questGiverName)
  {
    InterfaceSoundsManager.instance.PlayShowHint();
    QuestGiverNameText.text = questGiverName;
    QuestDialogCanvas.SetActive(true);
    GameUICanvas.SetActive(false);
  }

  public void ShowNextQuestDialogReplic()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    EventsManager.TriggerEvent(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC);
  }

  public void ShowDialog()
  {
    DialogCanvas.SetActive(true);
  }

  public void SetDialogText( string text )
  {
    DialogText.text = text;
  }

  public void SetQuestGiverNameText( string text )
  {
    QuestGiverNameText.text = text;
  }

  public void SetQuestDialogText(string text)
  {
    QuestDialogText.text = text;
  }

  public void ShowFloatingText( string text )
  {
    floatTextTimer = 0.0f;
    floatY = 100 / floatTextTime;
    FloatingText.rectTransform.anchoredPosition = Vector2.zero;
    FloatingText.text = text;
    FloatingText.gameObject.SetActive(true);
    StartCoroutine(floatingText());
  }

  float floatTextTimer;
  float floatTextTime = 3.0f;
  float floatY;
  IEnumerator floatingText()
  {
    while ( floatTextTimer < floatTextTime )
    {
      FloatingText.rectTransform.anchoredPosition += new Vector2(0.0f, floatY * Time.deltaTime);
      floatTextTimer += Time.deltaTime;
      yield return null;
    }
    FloatingText.gameObject.SetActive(false);
  }


  public void CloseDialog()
  {
    DialogCanvas.SetActive(false);
  }

  public void CloseQuestDialog()
  {
    QuestDialogCanvas.SetActive(false);
    GameUICanvas.SetActive(true);
  }


  void SetKeyImagesViewState(  List<Image> keyImageList, bool isCollcted )
  {
    for( int i = 0; i < keyImageList.Count; i++ )
    {
      Color color = keyImageList[i].color;
      color.a = 1.0f;
      keyImageList[i].color = color;
    }
  }

  public void ShowQuestInformation( Quest quest )
  {
    Time.timeScale = 0.0f;
    int id = (int)quest.questId;
    QuestInformation.SetActive(true);
    QuestShortDescText.text = LangResources.GetQuestSummary( id ) + " (" + quest.currentObjectives + "/" + quest.objectiveCount + ")";
    QuestDescriptionText.text = LangResources.GetQuestDescription(id);
    QuestNameText.text = LangResources.GetQuestName(id);
    questInfoIcon.sprite = quest.questIcon;
  }

  public void ShowHint( string text )
  {
    InterfaceSoundsManager.instance.PlayShowHint();
    HintCanvas.SetActive(true);
    HintText.text = text;
    GameUICanvas.SetActive(false);
  }

  public void ShowNextHintReplic()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    EventsManager.TriggerEvent( EventsIds.SHOW_NEXT_HINT_REPLIC );
  }

  public void CloseHint()
  {
    //InterfaceSoundsManager.instance.PlayBtnClip();
    InterfaceSoundsManager.instance.PlayShowHint();
    HintCanvas.SetActive(false);
    GameUICanvas.SetActive(true);
  }

  public void Revive()
  {
    InterfaceSoundsManager.instance.PlayBtnClip();
    googleAds.ShowRewardVideo();
  }

  public void ReviveForVideo()
  {
    OpenMenuBtn.SetActive(true);
    GameSystem.playerLives = 3;
    SetHealthText();
    if( GameSystem.diedInGameZone)
    {
      GameController.instance.SetPlayerToCheckpoint(FindObjectOfType<CharacterControllerScript>().transform);
      GameSystem.diedInGameZone = false;
    }
    GameOverMenu.SetActive(false);
    Time.timeScale = 1.0f;
  }

  public void ShowNextLevel()
  {
    EventsManager.TriggerEvent(EventsIds.SHOW_NEXT_LEVEL);
  }

  public void ShowPrevLevel()
  {
    EventsManager.TriggerEvent(EventsIds.SHOW_PREV_LEVEL);
  }

  public void PlayLevel()
  {
    EventsManager.TriggerEvent(EventsIds.PLAY_LEVEL );
  }

  public void ExitLevelMap()
  {
    CharacterControllerScript.instance.ExitStopState();
    SettingMap.SetActive(false);
    GameUICanvas.SetActive(true);
  }

  public void ShowLevelMap()
  {
    SettingMap.SetActive(true);
    GameUICanvas.SetActive(false);
  }

  public void HideHomyThrow()
  {
    HomyThrowBtn.SetActive(false);
  }

  public void ShowHomyThrow()
  {
    HomyThrowBtn.SetActive(true);
  }

  public void SetDefaultIcon()
  {
    hitButtonImage.rectTransform.sizeDelta = new Vector2(75.28f, hitButtonImage.rectTransform.sizeDelta.y);
    hitButtonImage.sprite = hitSprite;
  }

  public void SetTalkIcon()
  {
    interractSpriteImage.sprite = TalkSprite;
  }

  public void SetInterractIcon()
  {
    interractSpriteImage.sprite = InterractSprite;
  }


  public void ShowInterractIcon()
  {
    interractBtnImage.gameObject.SetActive(true);
  }

  public void HideInterractIcon()
  {
    interractBtnImage.gameObject.SetActive(false);
  }

  public void ShowWadrobe()
  {
    GameUICanvas.SetActive(false);
    WardrobeCanvas.SetActive(true);
    //wadrobe.ShouldOpenWadrobe = true;
  }

  public void CloseWadrobe()
  {
    GameUICanvas.SetActive(true);
    WardrobeCanvas.SetActive(false);
  }

  public void ToggleMuteMusic()
  {
    GameSystem.isMusicEnabled = !GameSystem.isMusicEnabled;
    SetMusicImage();
    ChangeAudioSnapshot();
  }

  public void SetMusicImage()
  {
    if (GameSystem.isMusicEnabled)
      musicImage.sprite = musicOnSprite;
    else
      musicImage.sprite = musicOffSprite;
  }

  public void ToggleMuteSounds()
  {
    GameSystem.isSoundEnabled = !GameSystem.isSoundEnabled;
    SetSoundImage();
    ChangeAudioSnapshot();
  }

  public void SetSoundImage()
  {
    if (GameSystem.isSoundEnabled)
      soundsImage.sprite = soundsOnSprite;
    else
      soundsImage.sprite = soundsOffSprite;
  }

  public void ChangeAudioSnapshot()
  {
    if (GameSystem.isMusicEnabled && GameSystem.isSoundEnabled)
      SndOnMscOn.TransitionTo(0);
    else if (!GameSystem.isMusicEnabled && GameSystem.isSoundEnabled)
      SndOnMscOff.TransitionTo(0);
    else if (GameSystem.isMusicEnabled && !GameSystem.isSoundEnabled)
      SndOffMscOn.TransitionTo(0);
    else 
      SndOffMscOff.TransitionTo(0);
  }
}
