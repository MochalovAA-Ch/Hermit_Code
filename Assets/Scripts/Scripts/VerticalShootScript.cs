using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalShootScript : MonoBehaviour {

  public GameObject projectile;
  Transform trProjectile;
  Rigidbody rb;
  public float shootTimer;
  public float startSpeed;
  public float gravityForce;

  //Сколько прошли на данный момент
  float currPath;
  //Таймер следующего выстрела
  float currTime;
  //Текущая скорость
  float speed;

	// Use this for initialization
	void Start ()
  {
    trProjectile = projectile.GetComponent<Transform>();
    rb = projectile.GetComponent<Rigidbody>();
	}


	
	// Update is called once per frame
	void Update ()
  {
    //Если объект неактивен
    if (!projectile.activeInHierarchy)
    {
      if ( currTime < shootTimer )
      {
        currTime += Time.deltaTime;
      }
      else
      {
        StartCoroutine("shootProjectile");
      }
    }
    else
    {

    }
      
	}

  IEnumerator shootProjectile()
  {
    speed = startSpeed;
    projectile.SetActive(true);
    //Выстрел вверх
    do
    {
      projectile.transform.Translate(Vector3.up * speed * Time.deltaTime);
      speed -= gravityForce * Time.deltaTime;
      yield return null;
    } while (projectile.transform.localPosition.y > 0 );
    projectile.SetActive(false);
    projectile.transform.position = transform.position;
    currTime = 0.0f;
  }
  

}
