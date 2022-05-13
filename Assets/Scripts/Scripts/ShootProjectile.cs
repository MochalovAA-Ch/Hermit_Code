using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {

  public GameObject projectile;               //Снаряд
  public float shootTimer;                    //Периодичность выстрелов
  public float startSpeed;                    //Начальная скорость снаряда
  public float distance;                      //Дистанция выстрела
  public bool useGravitySim = false;          //Используем ли симуляцию силы притяжения
  public float gravityForce;                  //Сила притяжения, действующая на объект

  Transform trProjectile;
  Rigidbody rb;
  float currPath;                             //Сколько прошли на данный момент
  float currTime;                             //Таймер следующего выстрела
  float speed;                                //Текущая скорость

  Vector3 gravity;
  Vector3 startPosition;
  // Use this for initialization
  void Start()
  {
    trProjectile = projectile.GetComponent<Transform>();
    //rb = projectile.GetComponent<Rigidbody>();
    gravity = Vector3.zero;
    startPosition = trProjectile.position;
    speed = startSpeed;
  }

  // Update is called once per frame
  void Update()
  {
    //Если объект неактивен
    if (!projectile.activeInHierarchy)
    {
      if (currTime < shootTimer)
      {
        currTime += Time.deltaTime;
      }
      else
      {
        //StartCoroutine("shootProjectile");
        projectile.SetActive(true);
      }
    }
    else
    {
      MoveProjectile();
    }
  }
  void MoveProjectile()
  {
    if (currPath + speed * Time.deltaTime >= distance )
    {
      currTime = 0.0f;
      currPath = 0.0f;
      trProjectile.position = startPosition;
      projectile.SetActive(false);
      return;
    }
    
    trProjectile.position += trProjectile.forward * speed * Time.deltaTime;
    currPath += speed * Time.deltaTime;
  }
}
