using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodTimeLiving : MonoBehaviour {

  public GameObject targetObject;

  public float LiveTime;
  public float RespawnTime;

  float currLifeTime;
  float currRespTime;

  public bool isAlive;

	// Use this for initialization
	void Start ()
  {
    currLifeTime = 0.0f;
  }

  // Update is called once per frame
  void Update()
  {
    if ( isAlive )
    {
      Live();
    }
    else
    {
      Respawn();
    }
	}

  //Объект начинает существовать
  public void StartLive()
  {
    currLifeTime = 0.0f;
    isAlive = true;
    targetObject.SetActive(true);
  }

  public void Live()
  {
    currLifeTime += Time.deltaTime;
    if (currLifeTime > LiveTime)
      EndLive();
  }

  public void Respawn()
  {
    currRespTime += Time.deltaTime;
    if (currRespTime > RespawnTime)
      StartLive();
  }

  //Объект заканчивает существовать
  public void EndLive()
  {
    currRespTime = 0.0f;
    isAlive = false;
    targetObject.SetActive(false);
  }
}
