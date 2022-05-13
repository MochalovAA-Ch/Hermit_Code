using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlatform2: MonoBehaviour  {

  public GameObject targetObject;
  public GameObject targetFallObject;

  public float LiveTime;
  public float RespawnTime;

  public float FallingTime;
  float FallingTimer;

  float currLifeTime;
  float currRespTime;

  public bool isAlive;
  //Платформа, которая разршушается под нами
  bool isStepped;

  Collider coll;

  public Animator anim;
	// Use this for initialization
	void Start ()
  {
    coll = targetObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
  {
		if( isStepped )
    {
      //Объект живет
      if( isAlive )
      {
        anim.SetBool("Destr", true );
        anim.SetBool("Idle", false);
        if (currLifeTime < LiveTime )
        {
          currLifeTime += Time.deltaTime;
        }
        //Объект начинает падать
        else
        {
          coll.enabled = true;
          anim.SetBool("Destr", false);
          anim.SetBool("Idle", false);
          isAlive = false;
          targetObject.SetActive(false);
          targetFallObject.SetActive(true);
        }
      }
      else
      {
        //Объект падает
        if(FallingTimer < FallingTime )
        {
          FallingTimer += Time.deltaTime;
        }
        else
        {
          targetFallObject.SetActive(false);
          
          if( currRespTime < RespawnTime )
          {
            currRespTime += Time.deltaTime;
          }
          else
          {
            anim.SetBool("Destr", false);
            anim.SetBool("Idle", true);
            coll.enabled = true;
            targetObject.SetActive(true);
            isAlive = true;
            isStepped = false;
            currLifeTime = 0.0f;
            currRespTime = 0.0f;
            FallingTimer = 0.0f;
          }
        }
      }
    }
	}

 

  private void OnTriggerEnter(Collider other)
  {
    isStepped = true;
  }
}
