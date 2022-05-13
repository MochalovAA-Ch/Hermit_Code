using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfScript : MonoBehaviour, IEnemy {


  public int hitPoints;
  public HealthBar healthBar;
  public Animator animController;
  public float speed;
  public float restTimer;
  public float hitDistance;
  public float hitCooldown;
  public float agroDistance;
  CharacterController charController;
  Vector3 startPosition;
  Vector3 moveDirection;
  public Transform meshTr;
  Rigidbody rb;
  ParticleSystem diePartSystem;
  //Если включен, волк бежит за игрком вне зависимости от того, где игрок
  public bool shouldAlwayFollow;
  bool isNoticePlayer;

  bool canHit;
  bool isHitting;
  float currRestTimer;
  float hitTimer = 0.3f;
  float currHitTimer;
  float hitCurrCooldown;

  bool isPlayerInZone;

  public WolfSounds wolfSounds;

  enum  EnemyStates { IDLE, CHASING_PLAYER, MOVE_TO_START, HITED, DYING };

  EnemyStates currentState;

  // Use this for initialization
  void Start()
  {
    diePartSystem = GameObject.Find("takeSmthAnim").GetComponent<ParticleSystem>();
    charController = GetComponent<CharacterController>();
    startPosition = transform.position;
    canHit = true;
    isHitting = false;
    hitCurrCooldown = hitCooldown;
    currentState = EnemyStates.IDLE;
    //meshTr = gameObject.GetComponentInChildren<Transform>();
    healthBar.CreateHealthBarsImages(hitPoints);
  }

  float distanceToPlayer;
  float distanceToPlayerFromSpawn;
  // Update is called once per frame
  void Update()
  {
    distanceToPlayer = Vector3.Distance(transform.position, SceneGeneralObjects.instance.playerTr.position );
    distanceToPlayerFromSpawn = Vector3.Distance(startPosition, SceneGeneralObjects.instance.playerTr.position);
    if (distanceToPlayerFromSpawn <= agroDistance )
      isPlayerInZone = true;
    else
      isPlayerInZone = false;
    switch ( currentState )
    {

      case EnemyStates.IDLE:
      {
        Idle_Update();
        break;
      }

      case EnemyStates.CHASING_PLAYER:
      {
        ChaseForPlayer_Update();
        break;
      }

      case EnemyStates.MOVE_TO_START:
      {
        MoveToStart_Update();
        break;
      }

      case EnemyStates.HITED:
        {
          Hited_Update();
          break;
        }
    }
  }

  void RotateToPlayer()
  {
    moveDirection = SceneGeneralObjects.instance.playerTr.position - transform.position;
    moveDirection = moveDirection.normalized;
    moveDirection.y = 0.0f;
    SetRotationToDirection();
    moveDirection = Vector3.zero;
  }

  void SetRotationToDirection()
  {
    float angle = -Vector3.SignedAngle(moveDirection, Vector3.forward, Vector3.up);
    meshTr.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
  }

  public float distanceXZ = 0.5f;
  public float distanceY = 0.0f;

  void SetMoveDirectionToPlayer()
  {
    moveDirection = SceneGeneralObjects.instance.playerTr.position - charController.transform.position;// + charController.center;
    if( GameUtils.getDistanceByXZ( moveDirection ) < 1.5f )
    {
      moveDirection.y = 0.0f;
      SetRotationToDirection();
      moveDirection.y = -30.8f;
      moveDirection.x = 0;
      moveDirection.z = 0;
      charController.Move(moveDirection * speed * Time.deltaTime);
    }
    else
    {
      moveDirection += moveDirection.normalized;
      moveDirection = moveDirection.normalized;
      moveDirection.y = 0.0f;
      SetRotationToDirection();
      moveDirection.y = -30.8f;
      charController.Move(moveDirection * speed * Time.deltaTime);
    }
  }

  bool isOnTopPlayer()
  {
    if (GameUtils.getDistanceByXZ( SceneGeneralObjects.instance.playerTr.position, charController.transform.position ) < distanceXZ)
    {
      if ( SceneGeneralObjects.instance.playerTr.position.y + distanceY < charController.transform.position.y )
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    else
    {
      return false;
    }
  }

  void Idle_Update()
  {
    wolfSounds.WolfGroan( distanceToPlayer );
    if( isPlayerInZone )
      currentState = EnemyStates.CHASING_PLAYER;
  }

  void MoveToStart_Update()
  {
    wolfSounds.WolfGroan( distanceToPlayer );
    isHitting = false;
    if (currRestTimer < restTimer)
    {
      currRestTimer += Time.deltaTime;
    }
    moveDirection = (startPosition - transform.position).normalized;
    float a = Vector3.Distance(startPosition, transform.position);
    float b = (moveDirection * speed * Time.deltaTime).magnitude;
    if ( Vector3.Distance(startPosition, transform.position) > (moveDirection * speed * Time.deltaTime).magnitude + 2.0f)
    {
      //Debug.Log("Двигаемся в стартовую точку");
      GameUtils.SetAnimState(animController, "shouldRun");
      moveDirection = (startPosition - transform.position).normalized;
      SetRotationToDirection();
      moveDirection.y = -30.8f;
      charController.Move(moveDirection * speed * Time.deltaTime);
    }
    else
    {
      currentState = EnemyStates.IDLE;
      GameUtils.SetAnimState(animController, "shouldIdle");
      transform.position = startPosition;
    }
  }

  float currBiteRemain;
  void ChaseForPlayer_Update()
  {
    if ( !shouldAlwayFollow )
    {
      if (!isPlayerInZone)
      {
        currentState = EnemyStates.MOVE_TO_START;
        return;
      }
    }

    //Бежим за игроком
    if (!isHitting && distanceToPlayer > hitDistance)
    {
      GameUtils.SetAnimState(animController, "shouldRun");
      
      //Тут вставить звук таймер для укуса
      if (currBiteRemain < hitTimer)
      {
        currBiteRemain += Time.deltaTime;
      }
      else
      {
        wolfSounds.WolfGroan(distanceToPlayer);
      }
    }
    //Кусаем игрока
    else
    {
      isHitting = true;
      //Если игрок был уязвим, убавляем жизни
      if (GameSystem.playerCanBeHitted)
      {
        Debug.Log("Hit");
        wolfSounds.WolfBite(distanceToPlayer);
        //Начинаем проигрывать анимацию
        animController.Play("Hit");

        //В середине анимации делаем укус
        if (currHitTimer < hitTimer)
        {
          currHitTimer += Time.deltaTime;
        }
        else
        {
          canHit = false;
          currHitTimer = 0.0f;
          currRestTimer = 0.0f;
          isHitting = false;
          currBiteRemain = 0.0f;
          if (distanceToPlayer < hitDistance )
          {    
            CharacterControllerScript.knockbackVector = (SceneGeneralObjects.instance.playerTr.position - transform.position).normalized;
            GameSystem.playerLives--;
            EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
            EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
          }
        }
      }
    }
    SetMoveDirectionToPlayer();
  }

  float hitedTimer;
  float hitedTime = 0.3f;
  void Hited_Update()
  {
    if( hitedTimer > hitedTime )
    {
      //Здесь враг умирает
      if( hitPoints <= 0 )
      {
        diePartSystem.gameObject.transform.position = transform.position;
        diePartSystem.Play();
        Destroy(gameObject);
      }
      else
      {
        currentState = EnemyStates.CHASING_PLAYER;
      }
    }
    else
    {
      hitedTimer += Time.deltaTime;
      charController.Move(-meshTr.forward * speed * Time.deltaTime);
    }
  }

  //Получаем удар от игрока
  public void GetHit()
  {
    healthBar.DecreaseHelth();
    hitedTimer = 0.0f;
    hitPoints--;
    wolfSounds.WolfHited( distanceToPlayer );
    currentState = EnemyStates.HITED;
  }

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "MinusLivesArea")
    {
      gameObject.SetActive(false);
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, agroDistance);
  }
}


