using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlatform : MonoBehaviour {

  public float destroyTime;        //Время разрушения, после того как платформа собралась
  public float startAppearanceTimeDelay;
  public float appearanceSpeed;
  public float decreaseScaleKoef;

  public Transform startAppearencePoint;
  public Transform endAppearancePoint;
  public Transform platform;

  Vector3 defaultScale;

  float destroyTimer;
  float startAppearaenceTimeDelayTimer;
  Collider collider;

  Vector3 moveDirection;
  float scaleInc;

  float moveTime;

  // Use this for initialization
  void Start ()
  {
    collider = platform.gameObject.GetComponent<Collider>();
    moveDirection = (endAppearancePoint.position - startAppearencePoint.position).normalized;
    float moveTime = (endAppearancePoint.position - startAppearencePoint.position).magnitude / appearanceSpeed;
    defaultScale = platform.localScale;
    scaleInc = (defaultScale.x - defaultScale.x * decreaseScaleKoef) / moveTime;
    platform.position = startAppearencePoint.position;
    platform.localScale = defaultScale * decreaseScaleKoef;
  }
	
	// Update is called once per frame
	void Update ()
  {
    //Задержка перед началом появления платформы
		if(startAppearaenceTimeDelayTimer < startAppearanceTimeDelay )
    {
      startAppearaenceTimeDelayTimer += Time.deltaTime;
      collider.isTrigger = true;
    }
    //Платформа появилась
    else
    {
      if( Vector3.Distance( platform.position, endAppearancePoint.position) > appearanceSpeed * Time.deltaTime )
      {
        collider.isTrigger = true;
        moveTime += Time.deltaTime;
        platform.Translate(moveDirection * appearanceSpeed * Time.deltaTime);
        if( platform.localScale.x < defaultScale.x )
        {
          platform.localScale += Vector3.one * scaleInc * Time.deltaTime;
        }
        else
        {
          platform.localScale = defaultScale;
        }
      }
      //Платформан на месте, начинаем отсчет до разрушения
      else
      {
        collider.isTrigger = false;
        
        platform.localScale = defaultScale;
        platform.position = endAppearancePoint.position;
        
        if( destroyTimer < destroyTime ) 
        {
          destroyTimer += Time.deltaTime;
        }
        else
        {
          destroyTimer = 0.0f;
          startAppearaenceTimeDelayTimer = 0.0f;
          collider.isTrigger = true;
          platform.position = startAppearencePoint.position;
          platform.localScale = defaultScale * decreaseScaleKoef;
        }
      }
    }
	}

  private void OnDrawGizmos()
  {
    if (startAppearencePoint == null || startAppearencePoint == null || platform == null)
      return;

    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(startAppearencePoint.position, platform.localScale);
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(endAppearancePoint.position, platform.localScale);
  }
}
