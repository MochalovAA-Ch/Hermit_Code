using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour {

  public Button StartGame;
  public Toggle DefaultCameraToggle;
  public Toggle FreeCameraToggle;
  public GameObject mainMenu;
  public GameObject optionsMenu;
  public Slider camSensetiveSlider;
  GameObject currentOpenedMenu;
  public Image musicImage;
  public Sprite musicOffSprite;
  public Sprite musicOnSprite;
  public Image soundsImage;
  public Sprite soundsOffSprite;
  public Sprite soundsOnSprite;

  public Material hairs_main_mat;
  public Material hairs_wood_mat;
  public Material hairs_leafs_mat;
  public Material hairs_acc_mat;

  public Material eyes_1_mat;
  public Material eyes_2_mat;
  public Material eyes_3_mat;
  public Material glases_mat;
  public Material glasses_lens_mat;

  public Material beards_main_mat;
  public Material beards_acc_mat;

  public Material ears_acc_mat;

  public Material horns_main_mat;

  public Material tshirts_main_mat;
  public Material tshirts_colar_mat;

  public Material coats_main_mat;
  public Material coats_colars_mat;
  public Material coats_acc_mat;

  public Material neck_main_mat;

  public Material pants_main_mat;
  public Material body_leafs_mat;
  public Material skin;
  public Material leg_acc_mat;

  public Material hat_material;
  public Material hats_jewel_mat;
  public Material boots_main_mat;

  public Material arms_main_mat;

  private void Start()
  {
    if ( !GameSystem.isDataLoaded )
    {
      SaveDataManager.LoadPlayer();
      SetMaterialsColors();
    }
    StartGame.onClick.AddListener(ContinueGame);
    if ( !LangResources.isCreated )
    {
      LangResources.isCreated = true;
      LangResources.InitLangResoursec();
    }

    if( GameSystem.isFreeCam )
    {
      FreeCameraToggle.isOn = true;
    }
    else
    {
      DefaultCameraToggle.isOn = true;
    }

    SetSoundImage();
  }

  public void ContinueGame()
  {
    SceneLoader.instance.LoadLevel(GameSystem.availableLevel);
    GameSystem.currentLevel = GameSystem.availableLevel;
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void SetDefaultCam()
  {
    GameSystem.isFreeCam = false;
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void SetFreeCam()
  {
    GameSystem.isFreeCam = true;
    MainMenuSoundManager.instance.ButtonClickSound();
  }


  public void SetControlType( bool moveWithJoystick )
  {
    GameSystem.withJoystick = moveWithJoystick;
  }

  public void BackToMenu()
  {
    currentOpenedMenu.SetActive(false);
    mainMenu.SetActive(true);
    SaveDataManager.SaveGameData();
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void OpenOptionsMenu()
  {
    mainMenu.SetActive(false);
    optionsMenu.SetActive(true);
    currentOpenedMenu = optionsMenu;
    camSensetiveSlider.value = GameSystem.cameraSensetive;
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void ChangeCameraSmoothnes()
  {
    GameSystem.cameraSensetive = camSensetiveSlider.value;
    CameraLook.smoothness = camSensetiveSlider.value;
  }

  public void SetRusLang()
  {
    GameSystem.language = 0;
    EventsManager.TriggerEvent(EventsIds.LANG_CHANGE);
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void SetEngLang()
  {
    GameSystem.language = 1;
    EventsManager.TriggerEvent(EventsIds.LANG_CHANGE);
    MainMenuSoundManager.instance.ButtonClickSound();
  }

  public void ToggleMuteMusic()
  {
    GameSystem.isMusicEnabled = !GameSystem.isMusicEnabled;
    MainMenuSoundManager.instance.ButtonClickSound();
    SetMusicImage();
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
    MainMenuSoundManager.instance.ButtonClickSound();
    SetSoundImage();
  }

  public void SetSoundImage()
  {
    if (GameSystem.isSoundEnabled)
      soundsImage.sprite = soundsOnSprite;
    else
      soundsImage.sprite = soundsOffSprite;
  }

  public void Exit()
  {
    MainMenuSoundManager.instance.ButtonClickSound();
    Application.Quit();
  }

  void SetMaterialsColors()
  {
    SetMaterialColor(hairs_main_mat, GameSystem.hairs_main_mat);
    SetMaterialColor(hairs_wood_mat, GameSystem.hairs_wood_mat);
    SetMaterialColor(hairs_leafs_mat, GameSystem.hairs_leafs_mat);
    SetMaterialColor(hairs_acc_mat, GameSystem.hairs_acc_mat);
    SetMaterialColor(eyes_1_mat, GameSystem.eyes_1_mat);
    SetMaterialColor(eyes_2_mat, GameSystem.eyes_2_mat);
    SetMaterialColor(eyes_3_mat, GameSystem.eyes_3_mat);
    SetMaterialColor(glases_mat, GameSystem.glases_mat);
    SetMaterialColor(glasses_lens_mat, GameSystem.glasses_lens_mat);
    SetMaterialColor(beards_main_mat, GameSystem.beards_main_mat);
    SetMaterialColor(beards_acc_mat, GameSystem.beards_acc_mat);
    SetMaterialColor(ears_acc_mat, GameSystem.ears_acc_mat);
    SetMaterialColor(tshirts_main_mat, GameSystem.tshirts_main_mat);
    SetMaterialColor(tshirts_colar_mat, GameSystem.tshirts_colar_mat);
    SetMaterialColor(coats_main_mat, GameSystem.coats_main_mat);
    SetMaterialColor(coats_colars_mat, GameSystem.coats_colars_mat);
    SetMaterialColor(coats_acc_mat, GameSystem.coats_acc_mat);
    SetMaterialColor(neck_main_mat, GameSystem.neck_main_mat);
    SetMaterialColor(arms_main_mat, GameSystem.arms_main_mat);
    SetMaterialColor(boots_main_mat, GameSystem.boots_main_mat);
    SetMaterialColor( leg_acc_mat, GameSystem.leg_acc_mat );
    SetMaterialColor( horns_main_mat, GameSystem.horns_main_mat);
    SetMaterialColor(hat_material, GameSystem.hat_material);
    SetMaterialColor(hats_jewel_mat, GameSystem.hats_jewel_mat);
    SetMaterialColor(skin, GameSystem.skin);
  }

  void SetMaterialColor( Material mat, float[] colorRgb )
  {
    //mat.color = 
    // mat.SetColor("_Color", Color.green/*new Color(colorRgb[0], colorRgb[1], colorRgb[2], 255.0f)*/);
    mat.SetColor("_Color", new Color( colorRgb[0] / 255.0f, colorRgb[1]/255.0f,colorRgb[2] / 255.0f, 1 ) );
  }
}
