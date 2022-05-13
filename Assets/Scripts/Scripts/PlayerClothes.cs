using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerClothes : MonoBehaviour {

  public Material coreMaterial;

  public List<GameObject> hairsList;          //0
  public List<GameObject> beardsList;         //1
  public List<GameObject> eyesList;           //2
  public List<GameObject> earsList;           //3
  public List<GameObject> hornsList;          //4
  public List<GameObject> tShirtsList;        //5
  public List<GameObject> coatsList;          //6
  public List<GameObject> necksList;          //7
  public List<GameObject> pantsList;          //8
  public List<GameObject> bootsList;          //9
  public List<GameObject> weaponsList;        //10
  public List<GameObject> armsList;           //11

  Dictionary<int, List<string>> hairsDict;
  Dictionary<int, List<string>> beardsDict;
  Dictionary<int, List<string>> eyesDict;
  Dictionary<int, List<string>> earsDict;
  Dictionary<int, List<string>> hornsDict;
  Dictionary<int, List<string>> tShirtsDict;
  Dictionary<int, List<string>> coatsDict;
  Dictionary<int, List<string>> neckDict;
  Dictionary<int, List<string>> pantsDict;
  Dictionary<int, List<string>> bootsDict;
  Dictionary<int, List<string>> weaponsDict;
  Dictionary<int, List<string>> armsDict;

  int categoryIndex = 0;
  int clothIndex = 0;

  List<Material> materialsList;

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.PUT_ON_CURRENT_CLOTH_SET, PutCurrentClothesSet );
  }

  private void OnDisable()
  {
    EventsManager.StopListening(EventsIds.PUT_ON_CURRENT_CLOTH_SET, PutCurrentClothesSet );
  }

  /*Здесь описывается структура списков вещей, логика работы с ними
   * Список выглядит следующим образом Dictionary<int, List<string>> hairsDict;
   * В списке строк хранятся имена вещей, которые входят в данный сет, например
   * список волос, в который входят волосы и два отдельных аксесуара, выглядит так
   * harsCategoryList = { 0: [ "hairs1", "accesoaries1", "accesoaries2" ]  , 1: [ ...] } ;
   * Каждый сет в категории соответсвует одной картинке в гардеробе
   * Для отображения в 3d есть список копий главного героя со всеми вещами, в зависимости от набора в нем включены те или иные предметы 
   * Для отображения интерфейса магазина по группам ( голова, торс, ноги ) БУДУТ использоваться свои функции отображения
   * хоть это и будет частичны дублированием кода,  так будет проще понимать код и делать некоторые пункты уникальными, если понадобится
   * например, функции ShowHeadSection и ShowBodySection будут иметь разную логику, потому что в HeadSection 5 категорий, а в BodySection 2.
    */




  // Use this for initialization
  void Start ()
  {
    materialsList = new List<Material>();

    hairsDict = new Dictionary<int, List<string>>();
    hairsDict.Add(0, new List<string>(new string[] { "" }));
    hairsDict.Add(1, new List<string>(new string[] { "hair4" }));
    hairsDict.Add(2, new List<string>(new string[] { "hair5", "hair6", "hair7" }));
    hairsDict.Add(3, new List<string>(new string[] { "hair2", "hair8" }));
    hairsDict.Add(4, new List<string>(new string[] { "hair", "hair2" }));
    hairsDict.Add(5, new List<string>(new string[] { "hair", "hair4" }));
    hairsDict.Add(6, new List<string>(new string[] { "hair3" }));
    hairsDict.Add(7, new List<string>(new string[] { "hair5" }));
    hairsDict.Add(8, new List<string>(new string[] { "hair9" }));
    hairsDict.Add(9, new List<string>(new string[] { "hair10", "hair11" }));
    hairsDict.Add(10, new List<string>(new string[] { "hair12", "hair13" }));
    hairsDict.Add(11, new List<string>(new string[] { "hair14" }));
    hairsDict.Add(12, new List<string>(new string[] { "hair15" }));
    hairsDict.Add(13, new List<string>(new string[] { "hair16" }));
    hairsDict.Add(14, new List<string>(new string[] { "hair17" }));
    hairsDict.Add(15, new List<string>(new string[] { "hair18" }));
    hairsDict.Add(16, new List<string>(new string[] { "hat1" }));
    hairsDict.Add(17, new List<string>(new string[] { "hat2" }));
    hairsDict.Add(18, new List<string>(new string[] { "hat3" }));
    hairsDict.Add(19, new List<string>(new string[] { "hat4" }));
    hairsDict.Add(20, new List<string>(new string[] { "hat5" }));
    hairsDict.Add(21, new List<string>(new string[] { "hat6" }));
    hairsDict.Add(22, new List<string>(new string[] { "hat7" }));
    hairsDict.Add(23, new List<string>(new string[] { "hat8", "hat_diamond" }));

    eyesDict = new Dictionary<int, List<string>>();
    eyesDict.Add(0, new List<string>(new string[] { "eye.L", "eye.R", "eye2.L", "eye2.R", "eye3.L", "eye3.R" }));
    eyesDict.Add(1, new List<string>(new string[] { "eye.L", "eye.R", "eye2.L", "eye2.R", "eye3.L", "eye3.R", "glasses1" }));
    eyesDict.Add(2, new List<string>(new string[] { "eye.L", "eye.R", "eye2.L", "eye2.R", "eye3.L", "eye3.R", "glasses2" }));
    eyesDict.Add(3, new List<string>(new string[] { "eye.L", "eye.R", "eye2.L", "eye2.R", "eye3.L", "eye3.R", "glasses3" }));
    eyesDict.Add(4, new List<string>(new string[] { "eye.L", "eye.R", "eye2.L", "eye2.R", "eye3.L", "eye3.R", "glasses4" }));

    beardsDict = new Dictionary<int, List<string>>();
    beardsDict.Add(0, new List<string>(new string[] { "beard" }));
    beardsDict.Add(1, new List<string>(new string[] { "" }));
    beardsDict.Add(2, new List<string>(new string[] { "beard", "beard_thing" }));
    beardsDict.Add(3, new List<string>(new string[] { "beard2" }));
    beardsDict.Add(4, new List<string>(new string[] { "beard3" }));
    beardsDict.Add(5, new List<string>(new string[] { "beard4" }));
    beardsDict.Add(6, new List<string>(new string[] { "Mustache" }));
    beardsDict.Add(7, new List<string>(new string[] { "Mustache2" }));
    beardsDict.Add(8, new List<string>(new string[] { "beard2", "Mustache2" }));
    beardsDict.Add(9, new List<string>(new string[] { "Mustache", "beard4" }));

    earsDict = new Dictionary<int, List<string>>();
    earsDict.Add(0, new List<string>(new string[] { "ear.L", "ear.R" }));
    earsDict.Add(1, new List<string>(new string[] { "ear.L", "ear.R", "ear_ring1.R", "ear_ring1.L" }));
    earsDict.Add(2, new List<string>(new string[] { "ear.L", "ear.R", "ear_ring3.L", "ear_ring1.L" }));
    earsDict.Add(3, new List<string>(new string[] { "ear.L", "ear.R", "ear_ring3.R", "ear_ring1.R" }));
    earsDict.Add(4, new List<string>(new string[] { "ear2.L", "ear2.R" }));
    earsDict.Add(5, new List<string>(new string[] { "ear2.L", "ear2.R", "ear_ring4.L" }));
    earsDict.Add(6, new List<string>(new string[] { "ear2.L", "ear2.R", "ear_ring4.R" }));
    earsDict.Add(7, new List<string>(new string[] { "ear2.L", "ear2.R", "ear_ring4.R", "ear_ring4.L" }));
    earsDict.Add(8, new List<string>(new string[] { "ear3.L", "ear3.R" }));
    earsDict.Add(9, new List<string>(new string[] { "ear3.L", "ear3.R", "ear_ring2.L", "ear_ring2.R" }));
    earsDict.Add(10, new List<string>(new string[] { "ear4.L", "ear4.R" }));
    earsDict.Add(11, new List<string>(new string[] { "ear4.L", "ear4.R", "ear_ring4.R" }));
    earsDict.Add(12, new List<string>(new string[] { "ear5.L", "ear5.R" }));
    earsDict.Add(13, new List<string>(new string[] { "ear5.L", "ear5.R", "ear_ring5.R" }));
    earsDict.Add(14, new List<string>(new string[] { "ear6.L", "ear6.R" }));
    earsDict.Add(15, new List<string>(new string[] { "ear6.L", "ear6.R", "ear_ring4.L" }));
    earsDict.Add(16, new List<string>(new string[] { "ear7.L", "ear7.R"}));
    earsDict.Add(17, new List<string>(new string[] { "ear7.L", "ear7.R", "ear_ring7.R", "ear_ring6.L" }));
    earsDict.Add(18, new List<string>(new string[] { "ear7.L", "ear7.R", "ear_ring7.L", "ear_ring6.L" }));
    earsDict.Add(19, new List<string>(new string[] { "ear8.L", "ear8.R" }));
    earsDict.Add(20, new List<string>(new string[] { "ear9.L", "ear9.R" }));

    hornsDict = new Dictionary<int, List<string>>();
    hornsDict.Add(0, new List<string>(new string[] { "" }));
    hornsDict.Add(1, new List<string>(new string[] { "horns2" }));
    hornsDict.Add(2, new List<string>(new string[] { "horns3" }));
    hornsDict.Add(3, new List<string>(new string[] { "horns4" }));
    hornsDict.Add(4, new List<string>(new string[] { "horns5" }));
    hornsDict.Add(5, new List<string>(new string[] { "horns6" }));
    hornsDict.Add(6, new List<string>(new string[] { "horns7" }));
    hornsDict.Add(7, new List<string>(new string[] { "horns8" }));
    hornsDict.Add(8, new List<string>(new string[] { "horns9" }));
    hornsDict.Add(9, new List<string>(new string[] { "horns10" }));
    hornsDict.Add(10, new List<string>(new string[] { "horns11" }));
    hornsDict.Add(11, new List<string>(new string[] { "horns1" }));

    neckDict = new Dictionary<int, List<string>>();
    neckDict.Add(0, new List<string>(new string[] { "" }));
    neckDict.Add(1, new List<string>(new string[] { "neck" }));
    neckDict.Add(2, new List<string>(new string[] { "neck2" }));
    neckDict.Add(3, new List<string>(new string[] { "neck3" }));

    tShirtsDict = new Dictionary<int, List<string>>();
    tShirtsDict.Add(0, new List<string>(new string[] { "T-shirt" }));
    tShirtsDict.Add(1, new List<string>(new string[] { "T-shirt2", "Buttons2" }));
    tShirtsDict.Add(2, new List<string>(new string[] { "T-shirt3" }));
    tShirtsDict.Add(3, new List<string>(new string[] { "T-shirt3", "Buttons" }));
    tShirtsDict.Add(4, new List<string>(new string[] { "T-shirt4" }));
    tShirtsDict.Add(5, new List<string>(new string[] { "leaf2" }));
    tShirtsDict.Add(6, new List<string>(new string[] { "T-shirt_collar" }));
    tShirtsDict.Add(7, new List<string>(new string[] { "" }));

    coatsDict = new Dictionary<int, List<string>>();
    coatsDict.Add(0, new List<string>(new string[] { "coat" }));
    coatsDict.Add(1, new List<string>(new string[] { "" }));
    coatsDict.Add(2, new List<string>(new string[] { "coat2" }));
    coatsDict.Add(3, new List<string>(new string[] { "coat2", "coat_spikes" }));
    coatsDict.Add(4, new List<string>(new string[] { "coat", "coat_collar", "coat_spikes" }));
    coatsDict.Add(5, new List<string>(new string[] { "coat3" }));
    coatsDict.Add(6, new List<string>(new string[] { "coat4" }));
    coatsDict.Add(7, new List<string>(new string[] { "coat4", "coat4_collar" }));
    coatsDict.Add(8, new List<string>(new string[] { "coat", "coat_collar" }));

    pantsDict = new Dictionary<int, List<string>>();
    pantsDict.Add(0, new List<string>(new string[] { "shorts" }));
    pantsDict.Add(1, new List<string>(new string[] { "skirt" }));
    pantsDict.Add(2, new List<string>(new string[] { "skirt2" }));
    pantsDict.Add(3, new List<string>(new string[] { "" }));
    pantsDict.Add(4, new List<string>(new string[] { "leaf" }));

    bootsDict = new Dictionary<int, List<string>>();
    bootsDict.Add(0, new List<string>(new string[] { "leg2.L", "leg2.R" })); //голые ноги в цвет кожи
    bootsDict.Add(1, new List<string>(new string[] { "leg2.L", "leg2.R", "leg2_1.L", "leg2_1.R" }));
    bootsDict.Add(2, new List<string>(new string[] { "leg2.L", "leg2.R", "leg2_2.L", "leg2_2.R" }));
    bootsDict.Add(3, new List<string>(new string[] { "leg3.L", "leg3.R" }));
    bootsDict.Add(4, new List<string>(new string[] { "leg4.L", "leg4.R" })); //голые ноги в цвет кожи
    bootsDict.Add(5, new List<string>(new string[] { "leg4.L", "leg4.R", "leg4_1.L", "leg4_1.R" }));
    bootsDict.Add(6, new List<string>(new string[] { "leg5.L", "leg5.R" }));
    bootsDict.Add(7, new List<string>(new string[] { "leg5.L", "leg5.R", "leg_spikes6.L", "leg_spikes6.R" }));
    bootsDict.Add(8, new List<string>(new string[] { "leg5.L", "leg5.R", "leg_spikes2.L", "leg_spikes2.R" }));
    bootsDict.Add(9, new List<string>(new string[] { "leg2.L", "leg2.R", "leg_spikes4.R", "leg_spikes.L", "leg_spikes3.L", "leg_spikes5.L", "leg_spikes4.L", "leg_spikes.R", "leg_spikes3.R", "leg_spikes5.R" }));
    
    weaponsDict = new Dictionary<int, List<string>>();
    weaponsDict.Add(0, new List<string>(new string[] { "staff" }));
    weaponsDict.Add(1, new List<string>(new string[] { "staff2" }));
    weaponsDict.Add(2, new List<string>(new string[] { "staff3" }));

    armsDict = new Dictionary<int, List<string>>();
    armsDict.Add(0, new List<string>(new string[] { "" }));
    armsDict.Add(1, new List<string>(new string[] { "sleeve1.L", "sleeve1.R" }));
    armsDict.Add(2, new List<string>(new string[] { "sleeve2.L", "sleeve2.R" }));
    armsDict.Add(3, new List<string>(new string[] { "sleeve3.L", "sleeve3.R" }));
    armsDict.Add(4, new List<string>(new string[] { "sleeve4.L", "sleeve4.R" }));

    //EventsManager.TriggerEvent(EventsIds.PUT_ON_CURRENT_CLOTH_SET);
    PutCurrentClothesSet();
  }
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  public void ShowCloth()
  {

  }

  public void ChangeCloth()
  {
    switch ( WadrobeScript.currentCategoryIndex )
    {
      case 0:
      {
        PutOnClothes(hairsList, hairsDict);
          //ShowClothsColors()
        break;
      }

      case 1:
      {
        PutOnClothes(beardsList, beardsDict);
        break;
          
      }

      case 2:
      {
        PutOnClothes(eyesList, eyesDict);
        break;
      }

      case 3:
      {
        PutOnClothes(earsList, earsDict);
        break;
      }

      case 4:
      {
        PutOnClothes( hornsList, hornsDict );
        break;
      }

      case 5:
      {
        PutOnClothes( tShirtsList, tShirtsDict );
        break;
      }
      case 6:
      {
        PutOnClothes( coatsList, coatsDict );
        break;
      }
      case 7:
      {
        PutOnClothes( necksList, neckDict );
        break;
      }
      case 8:
      {
        PutOnClothes( pantsList, pantsDict);
        break;
      }
      case 9:
      {
        PutOnClothes(bootsList, bootsDict);
        break;
      }
      case 10:
      {
        PutOnClothes(weaponsList, weaponsDict);
        break;
      }
      case 11:
      {
        PutOnClothes(armsList, armsDict);
        break;
      }
    }
  }

  //Одеваем вещи из категории, которые выбраны в магазине на данный момент
  //И формируем список уникальных материалов
  void PutOnClothes(List<GameObject> clothesList, Dictionary<int, List<string>> clothesSet)
  {
    materialsList.Clear();
    for (int i = 0; i < clothesList.Count; i++)
    {
      clothesList[i].SetActive(false);
      for (int j = 0; j < clothesSet[WadrobeScript.currentClothIndex].Count; j++)
      {
        if (clothesList[i].name == clothesSet[WadrobeScript.currentClothIndex][j])
        {
          clothesList[i].SetActive(true);
          MeshRenderer meshRend = clothesList[i].GetComponent<MeshRenderer>();//materialsList.Add();
          if( meshRend == null )
          {
            SkinnedMeshRenderer skinedMeshRend = clothesList[i].GetComponent<SkinnedMeshRenderer>();
            materialsList.Add(skinedMeshRend.sharedMaterial);
          }
          else
          {
            materialsList.Add(meshRend.sharedMaterial);
          }

          //Берем цвета дочерних элементов ( справедливо для стафа )
          if( clothesList[i].transform.childCount != 0 )
          {
            for ( int k = 0; k < clothesList[i].transform.childCount; k++ )
            {
              MeshRenderer tmeshRend = clothesList[i].transform.GetChild(k).GetComponent<MeshRenderer>();//materialsList.Add();
              if (tmeshRend == null)
              {
                SkinnedMeshRenderer tskinedMeshRend = clothesList[i].transform.GetChild(k).GetComponent<SkinnedMeshRenderer>();
                if(tskinedMeshRend != null )
                  materialsList.Add(tskinedMeshRend.sharedMaterial);
              }
              else
              {
                materialsList.Add(tmeshRend.sharedMaterial);
              }
            }
          }
        }
      }
    }
  }

  public List<Material> GetMaterials()
  {
    return materialsList;
  }

  void PutOnClothes(List<GameObject> clothesList, Dictionary<int, List<string>> clothesSet, int index )
  {
    //Если индекс -1 значить нужно снять все вещи в наборе
    if( index == -1 )
    {
      for (int i = 0; i < clothesList.Count; i++)
      {
        clothesList[i].SetActive(false);
      }
      return;
    }
    

    for (int i = 0; i < clothesList.Count; i++)
    {
      bool shouldEnabled = false;
      for (int j = 0; j < clothesSet[index].Count; j++)
      {

        if (clothesList[i].name == clothesSet[index][j])
        {
          shouldEnabled = true;
        }
      }
      clothesList[i].SetActive(shouldEnabled);
    }


    //Снимаем рукава у футболки если надето пальто
    if (clothesList == coatsList)
    {
      if ( index != 1)
      {
        for (int i = 0; i < tShirtsList.Count; i++)
        {
          if (tShirtsList[i].name.Contains("sleeve"))
          {
            tShirtsList[i].SetActive(false);
          }
        }
      }
    }
  }

  public int GetClothSetsCount( int index )
  {
    //war
    int count = 0;
    switch ( index )
    {
      case 0:
      {
        count = hairsDict.Count;
        break;
      }
      case 1:
      {
        count = beardsDict.Count;
        break;
      }
      case 2:
      {
        count = eyesDict.Count;
        break;
      }
      case 3:
      {
        count = earsDict.Count;
        break;
      }
      case 4:
      {
        count = hornsDict.Count;
        break;
      }
      case 5:
      {
        count = tShirtsDict.Count;
        break;
      }
      case 6:
      {
        count = coatsDict.Count;
        break;
      }
      case 7:
      {
        count = neckDict.Count;
        break;
      }
      case 8:
      {
        count = pantsDict.Count;
        break;
      }
      case 9:
      {
        count = bootsDict.Count;
        break;
      }
      case 10:
      {
        count = weaponsDict.Count;
        break;
      }
      case 11:
      {
        count = armsDict.Count;
        break;
      }
    }
    return count;
  }

  public void PutCurrentClothesSet()
  {
    Debug.Log( "PutCurrentClothesSet");
    PutOnClothes( hairsList, hairsDict, GameSystem.hairs );
    PutOnClothes( eyesList, eyesDict, GameSystem.eyes );
    PutOnClothes( earsList, earsDict, GameSystem.ears );
    PutOnClothes( beardsList, beardsDict, GameSystem.beards );
    PutOnClothes( hornsList, hornsDict, GameSystem.horns );
    PutOnClothes( tShirtsList, tShirtsDict, GameSystem.tshirts );
    PutOnClothes( coatsList, coatsDict, GameSystem.coats);
    PutOnClothes( necksList, neckDict, GameSystem.necks);
    PutOnClothes( pantsList, pantsDict, GameSystem.pants);
    PutOnClothes( bootsList, bootsDict, GameSystem.boots);
    PutOnClothes( weaponsList, weaponsDict, GameSystem.weapons);
    PutOnClothes( armsList, armsDict, GameSystem.weapons);
        EventsManager.TriggerEvent(EventsIds.CHANGE_STAFF_COLLIDER);
  }
}
