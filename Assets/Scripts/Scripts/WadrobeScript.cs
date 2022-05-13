using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WadrobeScript : MonoBehaviour {
  public InputController playerInput;
  public GameObject GameCamera;
  public GameObject WardrobeCanvasCamera;
  public GameObject gameUiCanvas;
  public GameObject wardrobeUiCanvas;
  public List<GameObject> subCategories;
  public Text clothIndexText;
  public PlayerClothes playerClothes_Wardrobe;

  public static int currentClothIndex;
  public static int currentCategoryIndex;
  public Button buyBtn;
  public Button equipBtn;
  public Text priceText;
  public Text coinsText;
  public Text clothSetIndexText;
  public Text gameUiCoinsText;
  public Image priceImage;
  public GameObject clothColorBtnTemplate;
  public Transform Maneken; 

  public GameObject colorPicker;
  public GameObject categoriesBtnContainer;

  public Image SelSubCatImg;
  //public GameObject colorPicker;
  public ColorPicker colorPickerScript;

  //С какими вещами мы зашли в магазин
  int startWadrobeHairIndex;
  int startWadrobeEyesIndex;
  int startWadrobeBeardsIndex;
  int startWadrobeEarsIndex;
  int startWadrobeHornsIndex;
  int startWadrobetShirtsIndex;
  int startWadrobeCoatsIndex;
  int startWadrobeNecksIndex;
  int startWadrobePantsIndex;
  int startWadrobeBootsIndex;
  int startWadrobeWeaponsIndex;
  int startWadrobeArmsIndex;

  int currentWadrobeHairIndex;
  int currentWadrobeEyesIndex;
  int currentWadrobeBeardsIndex;
  int currentWadrobeEarsIndex;
  int currentWadrobeHornsIndex;
  int currentWadrobetShirtsIndex;
  int currentWadrobeCoatsIndex;
  int currentWadrobeNecksIndex;
  int currentWadrobePantsIndex;
  int currentWadrobeBootsIndex;
  int currentWadrobeWeaponsIndex;
  int currentWadrobeArmsIndex;

  public GameObject skinColorBtn;

  // Use this for initialization
  void Start()
  {

  }
	
	// Update is called once per frame
	void Update ()
  {
    //CheckIfPlayerNear();
    if( Input.GetKeyDown( KeyCode.C ) )
    {
      EnterInWardrobe();
    }

    if( Input.GetKeyDown( KeyCode.X ) )
    {
      ExitFromWadrobe();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      EnterInWardrobe();//SwitchToWardrobe(true);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
     // SwitchToWardrobe(false);
    }
  }

  void EnterInWardrobe()
  {
    uniqueMaterials = new List<Material>();
    SwitchToWardrobe(true);
    CharacterControllerScript.instance.isInWardrobe = true;
   // playerClothes = CharacterControllerScript.instance.gameObject.GetComponent
    //player.position = wardrobePlayerPoint.position;
    //playerTr.rotation = Quaternion.Euler(0.0f, WardrobeCanvasCamera.transform.rotation.eulerAngles.y - 180.0f, 0.0f);

    coinsText.text = GameSystem.totalCoins.ToString();

    startWadrobeHairIndex = GameSystem.hairs;
    currentWadrobeHairIndex = startWadrobeHairIndex;

    startWadrobeEyesIndex = GameSystem.eyes;
    currentWadrobeEyesIndex = startWadrobeEyesIndex;

    startWadrobeBeardsIndex = GameSystem.beards;
    currentWadrobeBeardsIndex = startWadrobeBeardsIndex;

    startWadrobeEarsIndex = GameSystem.ears;
    currentWadrobeEarsIndex = startWadrobeEarsIndex;

    startWadrobeHornsIndex = GameSystem.horns;
    currentWadrobeHornsIndex = startWadrobeHornsIndex;

    startWadrobetShirtsIndex = GameSystem.tshirts;
    currentWadrobetShirtsIndex = startWadrobetShirtsIndex;

    startWadrobeCoatsIndex = GameSystem.coats;
    currentWadrobeCoatsIndex = startWadrobeCoatsIndex;

    startWadrobeNecksIndex = GameSystem.necks;
    currentWadrobeNecksIndex = startWadrobeNecksIndex;

    startWadrobePantsIndex = GameSystem.pants;
    currentWadrobePantsIndex = startWadrobePantsIndex;

    startWadrobeBootsIndex = GameSystem.boots;
    currentWadrobeBootsIndex = startWadrobeBootsIndex;

    startWadrobeWeaponsIndex = GameSystem.weapons;
    currentWadrobeWeaponsIndex = startWadrobeWeaponsIndex;

    startWadrobeArmsIndex = GameSystem.arms;
    currentWadrobeWeaponsIndex = startWadrobeWeaponsIndex;

    //Maneken.position = WardrobeInventoryCamera.transform.position + new Vector3(0.0f, -1.0f, -4.0f);
    //Maneken.rotation = WardrobeInventoryCamera.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);

    ShowSubCategoriesButtons(0);
    //ShowSubCategory(0);
    ShowSkinColor_OnImage();
  }

  public void ExitFromWadrobe()
  {
    SwitchToWardrobe(false);
    gameUiCoinsText.text = GameSystem.totalCoins.ToString();
    CloseColorPicker();
    //Проверяем, на какой вещи остановились в каждой категории, и если вещь доступна, то надеваем ее
    //Закоментированная логика подходит для случаев, если мы хотим одеть те вещи, на просмотрах которых мы остановились в магазине
    //Сейчас работает логика явного надевания вещи с помощью кнопки надеть
    /*if (GameSystem.enabledHairs[currentWadrobeHairIndex])
      GameSystem.hairs = currentWadrobeHairIndex;
    else
      GameSystem.hairs = startWadrobeHairIndex;

    if (GameSystem.enabledEyes[currentWadrobeEyesIndex])
      GameSystem.eyes = currentWadrobeEyesIndex;
    else
      GameSystem.eyes = startWadrobeEyesIndex;

    if (GameSystem.enabledBeards[currentWadrobeBeardsIndex])
      GameSystem.beards = currentWadrobeBeardsIndex;
    else
      GameSystem.beards = startWadrobeBeardsIndex;

    if (GameSystem.enabledEars[currentWadrobeEarsIndex])
      GameSystem.ears = currentWadrobeEarsIndex;
    else
      GameSystem.ears = startWadrobeEarsIndex;

    if (GameSystem.enabledTShirts[currentWadrobetShirtsIndex])
      GameSystem.tshirts = currentWadrobetShirtsIndex;
    else
      GameSystem.ears = startWadrobeEarsIndex;

    if (GameSystem.enabledCoats[currentWadrobeCoatsIndex])
      GameSystem.coats = currentWadrobeCoatsIndex;
    else
      GameSystem.coats = startWadrobeCoatsIndex;

    if (GameSystem.enabledPants[currentWadrobePantsIndex])
      GameSystem.pants = currentWadrobePantsIndex;
    else
      GameSystem.pants = startWadrobePantsIndex;

    if (GameSystem.enabledBoots[currentWadrobeBootsIndex])
      GameSystem.boots = currentWadrobeBootsIndex;
    else
      GameSystem.boots = startWadrobeBootsIndex;

    if (GameSystem.enabledWeapons[currentWadrobeWeaponsIndex])
      GameSystem.weapons = currentWadrobeWeaponsIndex;
    else
      GameSystem.weapons = startWadrobeWeaponsIndex;*/

    playerClothes_Wardrobe.PutCurrentClothesSet();
    EventsManager.TriggerEvent( EventsIds.PUT_ON_CURRENT_CLOTH_SET );
    SaveDataManager.SaveGameData();
    CharacterControllerScript.instance.isInWardrobe = false;
  }

  void SwitchToWardrobe( bool isWardrobe )
  {
    GameCamera.SetActive(!isWardrobe);
    WardrobeCanvasCamera.SetActive(isWardrobe);
    gameUiCanvas.SetActive(!isWardrobe);
    wardrobeUiCanvas.SetActive(isWardrobe);
  }

  public void TestToggle( bool value )
  {

  }

  int clothSetsCount = 0;
  int currentCategory = 0;
  Transform parentTr;
  public void ShowSubCategoriesButtons( int index )
  {
    RectTransform selCatImg = categoriesBtnContainer.transform.GetChild(0).GetComponent<RectTransform>();
    RectTransform btnRt = categoriesBtnContainer.transform.GetChild(index + 1).GetComponent<RectTransform>();
    selCatImg.anchoredPosition = btnRt.anchoredPosition;
    currentCategory = index;
    
    for ( var i = 0; i < subCategories.Count; i++ )
    {
      if( index == i )
      {
        subCategories[i].SetActive(true);
        parentTr = subCategories[i].transform;
        SelSubCatImg.transform.SetParent( subCategories[i].transform );
      }
      else
      {
        subCategories[i].SetActive(false);
      }
    }
    if (index == 0)
      ShowSubCategory(0);
    if( index == 1 )
    {
      ShowSubCategory( 5 );
    }
    if (index == 2)
    {
      ShowSubCategory(8);
    }
  }

  public void ShowSubCategory( int subCategoryIndex )
  {
    //Для оружия отключаем все кнопки
    if( subCategoryIndex == 10 )
    {
      for (var i = 0; i < subCategories.Count; i++)
      {
        subCategories[i].SetActive(false);
      }
      RectTransform selCatImg = categoriesBtnContainer.transform.GetChild(0).GetComponent<RectTransform>();
      RectTransform btnRt = categoriesBtnContainer.transform.GetChild(4).GetComponent<RectTransform>();
      selCatImg.anchoredPosition = btnRt.anchoredPosition;
      SelSubCatImg.gameObject.SetActive(false);
    }
    else
    {
      SelSubCatImg.gameObject.SetActive(true);
    }
    currentCategoryIndex = subCategoryIndex;
    clothSetsCount = playerClothes_Wardrobe.GetClothSetsCount(subCategoryIndex);
    int subCatBtnInGroup = 0;
    switch (subCategoryIndex)
    {
      case 0:
      {
        currentClothIndex = currentWadrobeHairIndex;
        subCatBtnInGroup = 0;
        break;
      }
      case 1:
      {
        currentClothIndex = currentWadrobeBeardsIndex;
        subCatBtnInGroup = 1;
        break;
      }
      case 2:
      {
        currentClothIndex = currentWadrobeEyesIndex;
        subCatBtnInGroup = 2;
        break;
      }
      case 3:
      {
        currentClothIndex = currentWadrobeEarsIndex;
        subCatBtnInGroup = 3;
        break;
      }
      case 4:
      {
        currentClothIndex = currentWadrobeHornsIndex;
        subCatBtnInGroup = 4;
        break;
      }
      case 5:
      {
        currentClothIndex = currentWadrobetShirtsIndex;
        subCatBtnInGroup = 0;
        break;
      }
      case 6:
      {
        currentClothIndex = currentWadrobeCoatsIndex;
        subCatBtnInGroup = 1;
        break;
      }
      case 7:
      {
        currentClothIndex = currentWadrobeNecksIndex;
        subCatBtnInGroup = 2;
        break;
      }
      case 8:
      {
        currentClothIndex = currentWadrobePantsIndex;
        subCatBtnInGroup = 0;
        break;
      }
      case 9:
      {
        currentClothIndex = currentWadrobeBootsIndex;
        subCatBtnInGroup = 1;
        break;
      }
      case 10:
      {
        currentClothIndex = currentWadrobeWeaponsIndex;
        break;
      }

      case 11:
      {
        currentClothIndex = currentWadrobeArmsIndex;
        break;
      }
    }

    RectTransform subCatRectTransform = parentTr.GetChild(subCatBtnInGroup).GetComponent<RectTransform>();
    SelSubCatImg.rectTransform.anchoredPosition = subCatRectTransform.anchoredPosition;

    currentClothIndex--;
    ShowNextCloth();

    clothSetIndexText.text = currentClothIndex+1+ "/" + clothSetsCount;
    ShowCloth();
    //Debug.Log(clothSetsCount);
  }

  void ShowCloth()
  {
    playerClothes_Wardrobe.ChangeCloth();
  }

  List<Material> clothesMaterials;
  List<Material> uniqueMaterials;
  List<GameObject> colorPickerBtnsList;
  void ShowClothsColors()
  {
    if (currentCategoryIndex == 10)
    {
      CloseColorPicker();
      return;
    }
     

    if (colorPickerBtnsList != null )
    {
      for( int i = 0; i < colorPickerBtnsList.Count; i++ )
      {
        Destroy(colorPickerBtnsList[i] );
      }
      colorPickerBtnsList.Clear();
    }
    else
    {
      colorPickerBtnsList = new List<GameObject>();
    }
    uniqueMaterials.Clear();
    clothesMaterials = playerClothes_Wardrobe.GetMaterials();
    bool hasMaterial = false;
    for(int i = 0; i < clothesMaterials.Count; i++ )
    {
      hasMaterial = false;
      for( int j = 0; j < uniqueMaterials.Count; j++ )
      {
        if ( clothesMaterials[i] == uniqueMaterials[j])
        {
          hasMaterial = true;
          break;
        }
      }
      if (!hasMaterial)
      {
        if( clothesMaterials[i].name != "skin" )
          uniqueMaterials.Add(clothesMaterials[i]);
      }
    }
   
    for ( int i = 0; i < uniqueMaterials.Count; i++ )
    {
      GameObject clothColorPicker = Instantiate( clothColorBtnTemplate, clothColorBtnTemplate.transform.parent );
      clothColorPicker.SetActive(true);
      clothColorPicker.GetComponent<RectTransform>().anchoredPosition = new Vector2(clothColorPicker.GetComponent<RectTransform>().anchoredPosition.x, clothColorPicker.GetComponent<RectTransform>().anchoredPosition.y - i * 90 );
      colorPickerBtnsList.Add(clothColorPicker);
      Image colorImg = clothColorPicker.transform.GetChild(0).GetComponent<Image>();
      colorImg.color = uniqueMaterials[i].color;
      int colorIndex = i;
      clothColorPicker.GetComponent<Button>().onClick.AddListener(delegate { ShowColorPicker(colorIndex); } );
    }
  }

  int uniqMaterialIndex;
  public void ShowColorPicker( int materialIdnex )
  {
    uniqMaterialIndex = materialIdnex;
    colorPicker.SetActive(true);
    colorPickerScript.CurrentColor = uniqueMaterials[uniqMaterialIndex].color;
    subCategories[currentCategory].SetActive(false);
    for( int i = 0; i < colorPickerBtnsList.Count; i++ )
    {
      colorPickerBtnsList[i].SetActive(false);
    }
  }

  public void CloseColorPicker()
  {
    colorPicker.SetActive(false);
    //Если в категории оружие, не показываем смену цветов
    if ( currentCategoryIndex == 10 || currentCategoryIndex == 20 )
      subCategories[currentCategory].SetActive(false);
    else
      subCategories[currentCategory].SetActive(true);

    if ( currentCategoryIndex != 20 )
    {
      for (int i = 0; i < colorPickerBtnsList.Count; i++)
      {
        //Если в категории оружие, не показываем смену цветов
        if (currentCategoryIndex != 10)
          colorPickerBtnsList[i].SetActive(true);
        else
          colorPickerBtnsList[i].SetActive(false);
      }
    }
    else
    {
      SetColorRgb(GameSystem.skin, playerClothes_Wardrobe.coreMaterial );
    }
    SaveColorOfMaterial();
  }

  public void ChangeColorOfMaterial( Color color)
  {
    //Transform img = EventSystem.current.currentSelectedGameObject.transform.GetChild(0);
    //Color color = img.GetComponent<Image>().color;
    if ( currentCategoryIndex == 20 )
    {
      Image skinImage = skinColorBtn.transform.GetChild(0).GetComponent<Image>();
      skinImage.color = color;
      playerClothes_Wardrobe.coreMaterial.color = color;
      return;
    }
      
    uniqueMaterials[uniqMaterialIndex].color = color;
    Image colorImg = colorPickerBtnsList[uniqMaterialIndex].transform.GetChild(0).GetComponent<Image>();
    colorImg.color = color;
  }

  void SaveColorOfMaterial()
  {
    if (uniqueMaterials.Count == 0)
      return;
    if (uniqueMaterials[uniqMaterialIndex].name == "beards_main_mat")
    {
      SetColorRgb(GameSystem.beards_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "beards_acc_mat")
    {
      SetColorRgb(GameSystem.beards_acc_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "body_leafs_mat")
    {
      SetColorRgb(GameSystem.body_leafs_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "boots_main_mat")
    {
      SetColorRgb(GameSystem.boots_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "coats_acc_mat" )
    {
      SetColorRgb(GameSystem.coats_acc_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "coats_colar_mat")
    {
      SetColorRgb(GameSystem.coats_colars_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "coats_main_mat")
    {
      SetColorRgb(GameSystem.coats_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "ear_acc_mat")
    {
      SetColorRgb(GameSystem.ears_acc_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "eye_mat1")
    {
      SetColorRgb(GameSystem.eyes_1_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "eye_mat2")
    {
      SetColorRgb(GameSystem.eyes_2_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "eye_mat3")
    {
      SetColorRgb(GameSystem.eyes_3_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "galsses_mat")
    {
      SetColorRgb(GameSystem.glases_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "glasses_lens_mat")
    {
      SetColorRgb(GameSystem.glasses_lens_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hairs_accessoaries_mat")
    {
      SetColorRgb(GameSystem.hairs_acc_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hairs_leafs_mat")
    {
      SetColorRgb(GameSystem.hairs_leafs_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hairs_main_mat")
    {
      SetColorRgb(GameSystem.hairs_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hairs_wood_mat")
    {
      SetColorRgb(GameSystem.hairs_wood_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hat_material")
    {
      SetColorRgb(GameSystem.hat_material, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "hats_jewel_mat")
    {
      SetColorRgb(GameSystem.hats_jewel_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "horn_material")
    {
      SetColorRgb(GameSystem.horns_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "leg_acc_mat")
    {
      SetColorRgb(GameSystem.leg_acc_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "neck_material")
    {
      SetColorRgb(GameSystem.neck_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "pants_main_mat")
    {
      SetColorRgb(GameSystem.pants_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "skin")
    {
      SetColorRgb(GameSystem.skin, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "tshirts_colar_mat")
    {
      SetColorRgb(GameSystem.tshirts_colar_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "tshirts_main_mat")
    {
      SetColorRgb(GameSystem.tshirts_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
    if (uniqueMaterials[uniqMaterialIndex].name == "arms_main_mat")
    {
      SetColorRgb(GameSystem.arms_main_mat, uniqueMaterials[uniqMaterialIndex]);
      return;
    }
  }

  void SetColorRgb( float[] rgbArray, Material material )
  {
    rgbArray[0] = material.color.r * 255.0f;
    rgbArray[1] = material.color.g * 255.0f;
    rgbArray[2] = material.color.b * 255.0f;
  }

  bool CheckIsClothEnabled(  List<bool> enabledClothArr, int clothIndex )
  {
    if ( enabledClothArr[clothIndex] )
    {
      priceImage.gameObject.SetActive(false);
      buyBtn.gameObject.SetActive(false);
      equipBtn.gameObject.SetActive(true);
      return true;
    }
    else
    {
      priceImage.gameObject.SetActive(true);
      buyBtn.gameObject.SetActive(true);
      equipBtn.gameObject.SetActive(false);
      switch ( currentCategoryIndex )
      {
        case 0:
        {
          priceText.text = GameSystem.hairsPrices[clothIndex].ToString();
          if (GameSystem.hairsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 1:
        {
          priceText.text = GameSystem.beardsPrices[clothIndex].ToString();
          if (GameSystem.beardsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 2:
        {
          priceText.text = GameSystem.eyesPrices[clothIndex].ToString();
          if (GameSystem.eyesPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }

        case 3:
        {
          priceText.text = GameSystem.earsPrices[clothIndex].ToString();
          if (GameSystem.earsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 4:
        {
          priceText.text = GameSystem.hornsPrices[clothIndex].ToString();
          if (GameSystem.hornsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 5:
        {
          priceText.text = GameSystem.tShirtsPrices[clothIndex].ToString();
          if (GameSystem.tShirtsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 6:
        {
          priceText.text = GameSystem.coatsPrices[clothIndex].ToString();
          if (GameSystem.coatsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 7:
        {
          priceText.text = GameSystem.necksPrices[clothIndex].ToString();
          if (GameSystem.necksPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 8:
        {
          priceText.text = GameSystem.pantsPrices[clothIndex].ToString();
          if (GameSystem.pantsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 9:
        {
          priceText.text = GameSystem.bootsPrices[clothIndex].ToString();
          if (GameSystem.bootsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
        case 10:
        {
          priceText.text = GameSystem.weaponsPrices[clothIndex].ToString();
          if (GameSystem.weaponsPrices[clothIndex] > GameSystem.totalCoins)
            buyBtn.interactable = false;
          else
            buyBtn.interactable = true;
          break;
        }
      }
      return false;
    }
  }

  public void ShowNextCloth()
  {
    currentClothIndex++;
    switch ( currentCategoryIndex )
    {
      case 0:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeHairIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledHairs, currentClothIndex);
        if (GameSystem.hairs == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 1:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeBeardsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledBeards, currentClothIndex);
        if (GameSystem.beards == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 2:
      {
        if ( currentClothIndex == clothSetsCount)
          currentClothIndex = 0;
        currentWadrobeEyesIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledEyes, currentClothIndex);
        if (GameSystem.eyes == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 3:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeEarsIndex = currentClothIndex;
        CheckIsClothEnabled( GameSystem.enabledEars, currentClothIndex );
        if (GameSystem.ears == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 4:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeHornsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledHorns, currentClothIndex);
        if (GameSystem.horns == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 5:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobetShirtsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledTShirts, currentClothIndex);
        if (GameSystem.tshirts == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 6:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeCoatsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledCoats, currentClothIndex);
        if (GameSystem.coats == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 7:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeNecksIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledNecks, currentClothIndex);
        if (GameSystem.necks == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 8:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobePantsIndex = currentClothIndex;
        CheckIsClothEnabled( GameSystem.enabledPants, currentClothIndex );
        if (GameSystem.pants == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 9:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeBootsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledBoots, currentClothIndex);
        if (GameSystem.boots == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
          ShowCloth();
        break;
      }
      case 10:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeWeaponsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledWeapons, currentClothIndex);
        if (GameSystem.weapons == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 11:
      {
        if (currentClothIndex == clothSetsCount)
          currentClothIndex = 0;

        currentWadrobeArmsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledArms, currentClothIndex);
        if (GameSystem.arms == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
    }
    ShowClothsColors();
    clothSetIndexText.text = currentClothIndex + 1 + "/" + clothSetsCount;
  }

  public void ShowPrevCloth()
  {
    currentClothIndex--;
    switch (currentCategoryIndex)
    {
      case 0:
        {
          if (currentClothIndex == -1)
            currentClothIndex = clothSetsCount - 1;

          currentWadrobeHairIndex = currentClothIndex;
          CheckIsClothEnabled(GameSystem.enabledHairs, currentClothIndex);
          if (GameSystem.hairs == currentClothIndex)
            equipBtn.gameObject.SetActive(false);
          ShowCloth();
          break;
        }
      case 1:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeBeardsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledBeards, currentClothIndex);
        if (GameSystem.beards == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 2:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;
        currentWadrobeEyesIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledEyes, currentClothIndex);
        if (GameSystem.eyes == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
          ShowCloth();
        break;
      }
      
      case 3:
        {
          if (currentClothIndex == -1)
            currentClothIndex = clothSetsCount - 1;

          currentWadrobeEarsIndex = currentClothIndex;
          CheckIsClothEnabled(GameSystem.enabledEars, currentClothIndex);
          if (GameSystem.ears == currentClothIndex)
            equipBtn.gameObject.SetActive(false);
          ShowCloth();
          break;
        }
      case 4:
        {
          if (currentClothIndex == -1)
            currentClothIndex = clothSetsCount - 1;

          currentWadrobeHornsIndex = currentClothIndex;
          CheckIsClothEnabled(GameSystem.enabledHorns, currentClothIndex);
          if (GameSystem.horns == currentClothIndex)
            equipBtn.gameObject.SetActive(false);
          ShowCloth();
          break;
        }
      case 5:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobetShirtsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledTShirts, currentClothIndex);
        if (GameSystem.tshirts == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 6:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeCoatsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledCoats, currentClothIndex);
        if (GameSystem.coats == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
          ShowCloth();
        break;
      }
      case 7:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeNecksIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledNecks, currentClothIndex);
        if (GameSystem.necks == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 8:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobePantsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledPants, currentClothIndex);
        if (GameSystem.pants == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 9:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeBootsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledBoots, currentClothIndex);
        if (GameSystem.boots == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 10:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeWeaponsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledWeapons, currentClothIndex);
        if (GameSystem.weapons == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
      case 11:
      {
        if (currentClothIndex == -1)
          currentClothIndex = clothSetsCount - 1;

        currentWadrobeArmsIndex = currentClothIndex;
        CheckIsClothEnabled(GameSystem.enabledArms, currentClothIndex);
        if (GameSystem.arms == currentClothIndex)
          equipBtn.gameObject.SetActive(false);
        ShowCloth();
        break;
      }
    }
    ShowClothsColors();
    clothSetIndexText.text = currentClothIndex + 1 + "/" + clothSetsCount;
  }

  public void BuyCloth()
  {
    buyBtn.gameObject.SetActive(false);
    equipBtn.gameObject.SetActive(true);
    priceImage.gameObject.SetActive(false);
    switch ( currentCategoryIndex )
    {
      case 0:
      {
        GameSystem.enabledHairs[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.hairsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 1:
      {
        GameSystem.enabledBeards[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.beardsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 2:
      {
        GameSystem.enabledEyes[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.eyesPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 3:
      {
        GameSystem.enabledEars[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.earsPrices[currentClothIndex];
        coinsText.text =GameSystem.totalCoins.ToString();
        break;
      }

      case 4:
      {
        GameSystem.enabledHorns[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.hornsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 5:
      {
        GameSystem.enabledTShirts[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.tShirtsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 6:
      {
        GameSystem.enabledCoats[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.coatsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }
      case 7:
      {
        GameSystem.enabledNecks[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.necksPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 8:
      {
        GameSystem.enabledPants[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.pantsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }

      case 9:
      {
        GameSystem.enabledBoots[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.bootsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }
      case 10:
      {
        GameSystem.enabledWeapons[currentClothIndex] = true;
        GameSystem.totalCoins -= GameSystem.weaponsPrices[currentClothIndex];
        coinsText.text = GameSystem.totalCoins.ToString();
        break;
      }
    }
    EquipCloth();
  }

  /*if (GameSystem.enabledHairs[currentWadrobeHairIndex])
      GameSystem.hairs = currentWadrobeHairIndex;
    else
      GameSystem.hairs = startWadrobeHairIndex;

    if (GameSystem.enabledEyes[currentWadrobeEyesIndex])
      GameSystem.eyes = currentWadrobeEyesIndex;
    else
      GameSystem.eyes = startWadrobeEyesIndex;

    if (GameSystem.enabledBeards[currentWadrobeBeardsIndex])
      GameSystem.beards = currentWadrobeBeardsIndex;
    else
      GameSystem.beards = startWadrobeBeardsIndex;

    if (GameSystem.enabledEars[currentWadrobeEarsIndex])
      GameSystem.ears = currentWadrobeEarsIndex;
    else
      GameSystem.ears = startWadrobeEarsIndex;

    if (GameSystem.enabledTShirts[currentWadrobetShirtsIndex])
      GameSystem.tshirts = currentWadrobetShirtsIndex;
    else
      GameSystem.ears = startWadrobeEarsIndex;

    if (GameSystem.enabledCoats[currentWadrobeCoatsIndex])
      GameSystem.coats = currentWadrobeCoatsIndex;
    else
      GameSystem.coats = startWadrobeCoatsIndex;

    if (GameSystem.enabledPants[currentWadrobePantsIndex])
      GameSystem.pants = currentWadrobePantsIndex;
    else
      GameSystem.pants = startWadrobePantsIndex;

    if (GameSystem.enabledBoots[currentWadrobeBootsIndex])
      GameSystem.boots = currentWadrobeBootsIndex;
    else
      GameSystem.boots = startWadrobeBootsIndex;

    if (GameSystem.enabledWeapons[currentWadrobeWeaponsIndex])
      GameSystem.weapons = currentWadrobeWeaponsIndex;
    else
      GameSystem.weapons = startWadrobeWeaponsIndex;*/

  public void EquipCloth()
  {
    switch (currentCategoryIndex)
    {
      case 0:
        {
          if (GameSystem.enabledHairs[currentWadrobeHairIndex])
            GameSystem.hairs = currentWadrobeHairIndex;
          break;
        }
      case 1:
        {
          if (GameSystem.enabledBeards[currentWadrobeBeardsIndex])
            GameSystem.beards = currentWadrobeBeardsIndex;
          break;
        }
      case 2:
        {
          if (GameSystem.enabledEyes[currentWadrobeEyesIndex])
            GameSystem.eyes = currentWadrobeEyesIndex;
          break;
        }
      case 3:
        {
          if (GameSystem.enabledEars[currentWadrobeEarsIndex])
            GameSystem.ears = currentWadrobeEarsIndex;
          break;
        }
      case 4:
        {
          if (GameSystem.enabledHorns[currentWadrobeEarsIndex])
            GameSystem.horns = currentWadrobeHornsIndex;
          break;
        }
      case 5:
        {
          if (GameSystem.enabledTShirts[currentWadrobetShirtsIndex])
            GameSystem.tshirts = currentWadrobetShirtsIndex;
          break;
        }
      case 6:
        {
          if (GameSystem.enabledCoats[currentWadrobeCoatsIndex])
            GameSystem.coats = currentWadrobeCoatsIndex;
          break;
        }
      case 7:
        {
          if (GameSystem.enabledNecks[currentWadrobeNecksIndex])
            GameSystem.necks = currentWadrobeNecksIndex;
          break;
        }
      case 8:
        {
          if (GameSystem.enabledPants[currentWadrobePantsIndex])
            GameSystem.pants = currentWadrobePantsIndex;
          break;
        }
      case 9:
        {
          if (GameSystem.enabledBoots[currentWadrobeBootsIndex])
            GameSystem.boots = currentWadrobeBootsIndex;
          break;
        }
      case 10:
        {
          if (GameSystem.enabledWeapons[currentWadrobeWeaponsIndex])
            GameSystem.weapons = currentWadrobeWeaponsIndex;
          break;
        }
    }
    equipBtn.gameObject.SetActive(false);
  }

  void ShowSkinColor_OnImage()
  {
    Image skinColorImage = skinColorBtn.transform.GetChild(0).GetComponent<Image>();
    skinColorImage.color = new Color(GameSystem.skin[0] / 255.0f, GameSystem.skin[1] / 255.0f, GameSystem.skin[2] / 255.0f);
  }

  public void ShowSkinColorPicker()
  {
    currentCategoryIndex = 20;
    uniqueMaterials.Clear();
    RectTransform selCatImg = categoriesBtnContainer.transform.GetChild(0).GetComponent<RectTransform>();
    RectTransform btnRt = skinColorBtn.GetComponent<RectTransform>();
    selCatImg.anchoredPosition = btnRt.anchoredPosition;
    colorPicker.SetActive(true);
    colorPickerScript.CurrentColor = playerClothes_Wardrobe.coreMaterial.color;//uniqueMaterials[uniqMaterialIndex].color;
    subCategories[currentCategory].SetActive(false);
    for (int i = 0; i < colorPickerBtnsList.Count; i++)
    {
      Destroy(colorPickerBtnsList[i]);
    }
    colorPickerBtnsList.Clear();
  }

}
