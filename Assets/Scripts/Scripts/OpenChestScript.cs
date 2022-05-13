using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestScript : MonoBehaviour {

  public int coinsCount;
  public GameObject coinsInChestObjet;
  public Transform[] chestCover;
  public AudioSource chestOpenSound;
  public AudioSource chestCoinsSound;


  //Угол открытия  сундука и скорость открытия
  public float openAngle;
  public float openSpeed;

  //Промежуток между вылетом монеток ( монетки вылетают из сундука одна за одной )
  public float coinFlyInterval;
  float coinFlyTimer;

  //Расстояние, которое пролетает монета вверх пока не начнет лететь к игроку
  public float vertCoinHeight;

  //Начальная скорость полета и прирост скорости полета монеты
  public float startCoinSpeed;
  public float coinAcc;
  public float getCoinDistance;

  float currOpenAngle = 0.0f;

  bool shouldCoinsFly = false;
  bool isOpened = false;
  bool isHitted = false;
  bool isFirstCoinTaken = false;

  GameObject coin;

  List<GameObject> coinsList;
  List<float> currDistance;
  List<float> currCoinsSpeed;
  List<bool> isStartedFly;
  List<bool> isCoinTaken;
  int coinStartFlyIndex;
  public Vector2 rangeX;
  public Vector2 rangeZ;

	// Use this for initialization
	void Start ()
  {
    coin = Resources.Load("coin 1") as GameObject;
    coinsList = new List<GameObject>();
    currDistance = new List<float>();
    currCoinsSpeed = new List<float>();
    isStartedFly = new List<bool>();
    isCoinTaken = new List<bool>();
    coinStartFlyIndex = 0;
    for ( int i = 0; i < coinsCount; i++ )
    {
      coinsList.Add( Instantiate( coin, transform.position, Quaternion.identity ) );
      coinsList[i].GetComponent<BoxCollider>().enabled = false;
      coinsList[i].SetActive( false );
      coinsList[i].transform.localScale *= 0.5f;
      currDistance.Add( 0.0f );
      currCoinsSpeed.Add( startCoinSpeed );
      isStartedFly.Add( false );
      isCoinTaken.Add( false );
      coinFlyTimer = coinFlyInterval;
    }
    //SceneGeneralObjects.instance.playerTr = SceneGeneralObjects.instance.SceneGeneralObjects.instance.playerTrTr//FindObjectOfType<CharacterControllerScript>().transform;
  }

	// Update is called once per frame
	void Update ()
  {
    if ( isOpened == true )
      return;

    if (!shouldCoinsFly)
      return;

    //Пришло ли время запускать следующую монету
    if (coinFlyTimer < coinFlyInterval)
    {
      coinFlyTimer += Time.deltaTime;
    }
    else
    {
      isStartedFly[coinStartFlyIndex] = true;
      if( coinStartFlyIndex != coinsList.Count - 1 )
        coinStartFlyIndex++;
      coinFlyTimer = 0.0f;
    }

    bool isAllTaken = true;
    for ( int i = 0; i < coinsList.Count; i++ )
    {
      if( isStartedFly[i]  )
      {
        if (isCoinTaken[i])
          continue;
        coinsList[i].SetActive( true );
        if( currDistance[i] < vertCoinHeight )
        {
          coinsList[i].transform.position += ( coinsList[i].transform.up + new Vector3(Random.Range(rangeX.x, rangeX.y), 0.0f, Random.Range(rangeZ.x, rangeX.y)) ) * currCoinsSpeed[i] * Time.deltaTime;
          currDistance[i] += currCoinsSpeed[i] * Time.deltaTime;
          currCoinsSpeed[i] += coinAcc;            
        }
        else
        {
          if( Vector3.Distance( coinsList[i].transform.position, SceneGeneralObjects.instance.playerTr.position) < getCoinDistance )
          {
            if( !isFirstCoinTaken )
            {
              chestCoinsSound.Play();
              isFirstCoinTaken = true;
            }
            GameSystem.collectedCoinsOnLevel++;
            EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
            coinsList[i].SetActive(false);
            isCoinTaken[i] = true;
          }
          else
          {
            coinsList[i].transform.position += ( SceneGeneralObjects.instance.playerTr.position - coinsList[i].transform.position ).normalized * currCoinsSpeed[i] * Time.deltaTime;
            currCoinsSpeed[i] += coinAcc;
          }
        }
      }
      if (!isCoinTaken[i])
        isAllTaken = false;
    }

    if( isAllTaken )
    {
      isOpened = true;
      coinsList.Clear();
      currCoinsSpeed.Clear();
      currDistance.Clear();
      isStartedFly.Clear();
      isCoinTaken.Clear();
      GetComponent<OutlineController>().enabled = false;
      GetComponent<Outline>().enabled = false;
    }
	}

  private void OnTriggerEnter( Collider other )
  {
    if(other.gameObject.tag =="Staff" )
    {
      if( StaffController.isStaffInHitt)
      {
        if( !isHitted )
        {
          StartCoroutine("openChest");
          // if (GameSystem.isSoundEnabled)
          chestOpenSound.Play();
          isHitted = true;
        }
      }
    }
  }

  IEnumerator openChest()
  {
    while( currOpenAngle < openAngle )
    {
      for( int i = 0; i < chestCover.Length; i++ )
      chestCover[i].rotation *= Quaternion.Euler(0.0f, openSpeed * Time.deltaTime, 0.0f);
      currOpenAngle += openSpeed * Time.deltaTime;
      yield return null;
    }
 
    shouldCoinsFly = true;
  }
}
