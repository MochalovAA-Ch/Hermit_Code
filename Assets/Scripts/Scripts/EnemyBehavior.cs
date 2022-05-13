using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {


  //Максимальная дистанция погони от стартовой точки
  public float maxMoveDistance;
  //Скорость движения врага
  public float moveSpeed;
  //Дистанция для замечания игрока
  public float agroDistance;
  //Таймер удара
  public float hitTimer;

  //Компоненты объектов ------------------------
  CharacterController charContr;
  GameUIController gameUI;
  Transform playerTransform;
  Transform tr;
  //--------------------------------------------

  //Направление к игроку
  Vector3 playerDirection;

  //Позиция точки появления на сцене ( для возращаение в ту же точку после погони ) 
  Vector3 spawnPoint;

  //Направление к точки появления
  Vector3 spawnDirection;

  //Гонимся ли за игроком
  bool canChasingPlayer = true;
  //Можем ли ударить игрока
  bool canHitPlayer = true;

  //Дистанция от стартовой точки до текущего положения
  float distanceFromSpawnPoint;

  //Таймер времени с момента удара
  float timeFromLastHit;

  // Use this for initialization

  private void Awake()
  {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    gameUI = GameObject.FindObjectOfType<GameUIController>();
    spawnPoint = transform.position;
    tr = GetComponent<Transform>();
    charContr = GetComponent<CharacterController>();
  }

  void Start ()
  {
    
  }
  
  // Update is called once per frame
  void Update ()
  {
    //Вектор от врага до игрока
    playerDirection = playerTransform.position - tr.position;
    //Как только коснулись земли, считаем дистанцию
    if (  charContr.isGrounded )
      distanceFromSpawnPoint = Mathf.Abs( ( spawnPoint - tr.position ).magnitude );
    Vector3 move = new Vector3(0.0f, -9.8f, 0.0f);

    //Если сагрились на игрока, бежим за ним
    if ( playerDirection.magnitude <= agroDistance &&  distanceFromSpawnPoint <= maxMoveDistance && canChasingPlayer )
    {
      move = new Vector3( playerDirection.normalized.x, -9.8f, playerDirection.normalized.z );
    }
    //Иначе возращаемся в исходное положение, и не гонимся за игроком пока не достигнем его
    else
    {
      canChasingPlayer = false;
      spawnDirection = spawnPoint - tr.position;
      //Если расстояние от исходной точки меньше определенного значения, снова можем гнаться за игроком
      if (spawnDirection.magnitude <= 0.2f)
        canChasingPlayer = true;
      move = new Vector3( spawnDirection.normalized.x, -9.8f, spawnDirection.normalized.z );
    }
    
    if ( !canHitPlayer )
    {
      timeFromLastHit += Time.deltaTime;
      if(  timeFromLastHit >= hitTimer )
      {
        canHitPlayer = true;
      }
    }

    charContr.Move(move * moveSpeed * Time.deltaTime);
  }

  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if ( hit.gameObject.tag == "Player" && canHitPlayer && GameSystem.playerCanBeHitted )
    {
      GameSystem.playerLives--;
      EventsManager.TriggerEvent( EventsIds.DECREASE_LIVES );
      canHitPlayer = false;
      timeFromLastHit = 0.0f;
    }
  }
}