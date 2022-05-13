using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour, IEnemy {

  public EnemyZoneScipt enemyZoneScipt;
  public int hitPoints;
  public HealthBar healthBar;

  public List<Transform> wayPoints;
  public int targetPointIndex;
  public float defaultSpeed;
  public float chaseSpeed;
  public bool isReverse = false;
  public Transform patrolMeshTr;
  public float hitTimer;
  public float hitDistance;
  public float restTimer;
  public float biteOffset;
  public float gravity;

  public CharacterController charTr;

  public CrocSounds crocSounds;

  Vector3 moveDirection;
  Vector3 size;
  Vector3 targetPosition;
  Vector3 playerExitPosition;
  Vector3 gravityVec;
  Vector3 fallFromPlayerVec = Vector3.zero;

  Transform playerTr;
  ParticleSystem diePartSystem;
  float speed;
  float currentHitTimer;
  float currentRestTimer;
  float playerTrWidth;
  float swampSurface;
  float startHitTimer = 0.3f;
  float currStartHitTimer = 0.3f;
  float hitedTimer = 0.0f;
  float hitedTime = 0.3f;

  bool shouldMoveForward;
  bool isChasing;
  bool isHitted;
  bool isReturnedToWayPoint;
  bool isPlayerInTrigger;
  bool isRestedFromChase;
  bool isReachedPlayerExitPoint;
  bool startHit = false;

  bool isFallFromTopPlayer = false;

  public Animator animator;

  enum EnemyStates { IDLE, PATROL, CHASING, RESTING, RETURN_TO_PATROL, HITTED, MOVE_EXIT_POINT }

  EnemyStates currentState;

  void SetRotationToDirection()
  {
    float angle = Vector3.Angle(moveDirection, Vector3.forward);
    if (moveDirection.x < 0)
      angle = -angle;
    patrolMeshTr.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
  }



  // Use this for initialization
  void Start()
  {
    diePartSystem = GameObject.Find("takeSmthAnim").GetComponent<ParticleSystem>();
    healthBar.CreateHealthBarsImages(hitPoints);
    currentState = EnemyStates.PATROL;
    targetPosition = wayPoints[targetPointIndex].position;
    moveDirection = (targetPosition - patrolMeshTr.position).normalized;
    SetRotationToDirection();
    shouldMoveForward = true;
    isChasing = false;
    isReturnedToWayPoint = true;
    speed = defaultSpeed;
    currentHitTimer = 0.0f;
    isPlayerInTrigger = true;
    isRestedFromChase = true;
    isReachedPlayerExitPoint = true;
    playerTr = FindObjectOfType<CharacterControllerScript>().transform;
    charTr.transform.position = targetPosition;// new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
    playerTrWidth = playerTr.gameObject.GetComponent<CharacterController>().radius;
    swampSurface = charTr.transform.position.y;
  }

  float distanceToPlayer;

  // Update is called once per frame
  void Update()
  {
    distanceToPlayer = Vector3.Distance(patrolMeshTr.position, SceneGeneralObjects.instance.playerTr.position);
    if (!charTr.gameObject.activeInHierarchy)
      return;

    switch (currentState)
    {

      case EnemyStates.PATROL:
      {
        Patrol_Update();
        break;
      }

      case EnemyStates.CHASING:
      {
        Chasing_Update();
        break;
      }

      case EnemyStates.RESTING:
      {
        RestFromChase_Update();
        break;
      }

      case EnemyStates.HITTED:
      {
        Hited_Update();
        break;
      }
    }
  }




  void Patrol_Update()
  {
    crocSounds.CrocGroan(distanceToPlayer);
    if ( enemyZoneScipt.isPlayerInZone )
    {
      currentState = EnemyStates.CHASING;
      speed = chaseSpeed;
      isHitted = false;
      return;
    }

    if (!isReverse)
    {
      
      if (getDistanceByXZ(charTr.transform.position, targetPosition) < speed * Time.deltaTime)
      {
        if (targetPointIndex == wayPoints.Count - 1)
        {
          targetPointIndex = 0;
          targetPosition = wayPoints[targetPointIndex].position;
        }
        else
        {
          targetPointIndex++;
          targetPosition = wayPoints[targetPointIndex].position;
        }
      }
    }
    else
    {
      if (shouldMoveForward)
      {
        //Debug.Log(getDistanceByXZ(charTr.transform.position, targetPosition));
        if (getDistanceByXZ(charTr.transform.position, targetPosition) < speed * Time.deltaTime)
        {
          if (targetPointIndex == wayPoints.Count - 1)
          {
            shouldMoveForward = false;
            targetPointIndex = wayPoints.Count - 2;
            targetPosition = wayPoints[targetPointIndex].position;
          }
          else
          {
            targetPointIndex++;
            targetPosition = wayPoints[targetPointIndex].position;
          }
           //moveDirection = (targetPosition - charTr.transform.position).normalized;
          //SetRotationToDirection();
        }
        else
        {
          //Debug.Log(moveDirection.magnitude);
        }

      }
      else
      {
        if (getDistanceByXZ(charTr.transform.position, targetPosition) < speed * Time.deltaTime)
        {
          if (targetPointIndex == 0)
          {
            shouldMoveForward = true;
            targetPointIndex++;
            targetPosition = wayPoints[targetPointIndex].position;
          }
          else
          {
            targetPointIndex--;
            targetPosition = wayPoints[targetPointIndex].position;
          }
          //moveDirection = (targetPosition - charTr.transform.position);//.normalized;
          //moveDirection.y = 0;
         // moveDirection = moveDirection.normalized;
          //SetRotationToDirection();
        }
      }
    }
    moveDirection = (targetPosition - charTr.transform.position);
    moveDirection.y = 0.0f;
    SetRotationToDirection();
    charTr.Move((moveDirection.normalized * speed + gravityVec) * Time.deltaTime);
  }


  void Chasing_Update()
  {
    if ( !enemyZoneScipt.isPlayerInZone )
    {
      currentState = EnemyStates.RESTING;
      currentRestTimer = 0.0f;
      return;
    }

    if( isHitted )
    {
      if ( currentHitTimer < hitTimer )
      {
        currentHitTimer += Time.deltaTime;
      }
      else
      {
        isHitted = false;
      }
    }
    float dist = Vector3.Distance(playerTr.position, charTr.transform.position + patrolMeshTr.forward * (playerTrWidth + biteOffset));
    if ( dist < hitDistance )
    {
      if (!isHitted)
      {
        currentHitTimer = 0.0f;
        //Тут анимация удара
        animator.Play("Bite");
        currentHitTimer = 0.0f;
        isHitted = true;
        crocSounds.CrocBite(distanceToPlayer);

        //Если игрок был уязвим, убавляем жизни
        if (GameSystem.playerCanBeHitted)
        {

          CharacterControllerScript.knockbackVector = (playerTr.position - charTr.transform.position).normalized;
          GameSystem.playerLives--;
          EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
          EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
        }
      }
    }
    else
    {
      if (charTr.transform.position.y > swampSurface + gravity * Time.deltaTime)
      {
        gravityVec.y = gravity;
      }
      else
      {
        gravityVec = Vector3.zero;
      }
    }
    if (isOnTopPlayer())
    {

    }
    else
    {
      SetMoveDirectionToPlayer();
    }
    if( crocSounds.audioSource.clip == crocSounds.crocBite )
    {
      if( !crocSounds.audioSource.isPlaying )
      {
        crocSounds.CrocGroan( distanceToPlayer );
      }
    }
    charTr.Move(moveDirection * speed * Time.deltaTime);
  }

  void RestFromChase_Update()
  {
    if (currentRestTimer < restTimer)
    {
      currentRestTimer += Time.deltaTime;
    }
    else
    {
      currentRestTimer = 0.0f;
      if (enemyZoneScipt.isPlayerInZone)
        currentState = EnemyStates.CHASING;
      else
      {
        float minDistance = Vector3.Distance(wayPoints[0].position, charTr.transform.position);
        int minIndex = 0;
        for (int i = 1; i < wayPoints.Count; i++)
        {
          if (Vector3.Distance(wayPoints[i].position, charTr.transform.position) < minDistance)
          {
            minDistance = Vector3.Distance(wayPoints[i].position, charTr.transform.position);
            minIndex = i;
          }
        }
        targetPointIndex = minIndex;
        targetPosition = wayPoints[targetPointIndex].position;
        isChasing = false;
        currentState = EnemyStates.PATROL;
      }
    }
  }


  void Hited_Update()
  {
    if (hitedTimer > hitedTime)
    {
      //Здесь враг умирает
      if (hitPoints <= 0)
      {
        diePartSystem.gameObject.transform.position = transform.position;
        diePartSystem.Play();
        Destroy(gameObject);
      }
      else
      {
        if (enemyZoneScipt.isPlayerInZone)
        {
          currentState = EnemyStates.CHASING;
        }
        else
        {
          currentState = EnemyStates.RESTING;
        }
      }
    }
    else
    {
      hitedTimer += Time.deltaTime;
      charTr.Move(-patrolMeshTr.forward * speed * Time.deltaTime);
    }
  }

  private void OnDrawGizmos()
  {
    if (wayPoints.Count < 3)
      return;

    Gizmos.color = Color.green;
    Gizmos.DrawWireCube( wayPoints[0].position, new Vector3(1, 1, 1));

    Gizmos.color = Color.blue;
    for( int i = 1; i < wayPoints.Count-1; i++ )
    {
      Gizmos.DrawWireCube(wayPoints[i].position, new Vector3(1, 1, 1));
    }

    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(wayPoints[wayPoints.Count-1].position, new Vector3(1, 1, 1));
  }

  private void OnTriggerEnter(Collider other)
  {
    /*if ( other.tag == "Player" )
    {
      isPlayerInTrigger = true;
      isChasing = true;
      isRestedFromChase = true;
      speed = chaseSpeed;
    }*/
  }

  private void OnTriggerExit(Collider other)
  {
   /* if ( other.tag == "Player" )
    {
      isPlayerInTrigger = false;
      isRestedFromChase = false;
      playerExitPosition = playerTr.position;
      playerExitPosition.y = 0.0f;
      isReachedPlayerExitPoint = false;
      //speed = defaultSpeed;
      //Запускаем таймер "отдыха" крокодила после погони

      //isChasing = false;
     /* speed = defaultSpeed;
      isChasing = false;
      playerTr = null;

      float minDistance = minDistance = Vector3.Distance(wayPoints[0].position, patrolMeshTr.position);
      int minIndex = 0;
      for (int i = 1; i < wayPoints.Count; i++)
      {
        if (Vector3.Distance(wayPoints[i].position, patrolMeshTr.position) < minDistance)
        {
          minDistance = Vector3.Distance(wayPoints[i].position, patrolMeshTr.position);
          minIndex = i;
        }
      }

      Debug.Log( "Возвращаемся в вейпоинт" );
      targetPointIndex = minIndex;
      moveDirection = (wayPoints[targetPointIndex].position - patrolMeshTr.position).normalized;
      moveDirection.y = 0.0f;
    }*/
  }

  float getDistanceByXZ( Vector3 a, Vector3 b )
  {
    return (Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.z - a.z) * (b.z - a.z)));
  }

  public float distanceXZ;
  public float distanceY;

  bool isOnTopPlayer()
  {
    if (GameUtils.getDistanceByXZ(playerTr.position, charTr.transform.position) < distanceXZ)
    {
      if (playerTr.position.y + distanceY < charTr.transform.position.y)
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

  public void GetHit()
  {
    healthBar.DecreaseHelth();
    hitedTimer = 0.0f;
    hitPoints--;
    crocSounds.CrocHited(distanceToPlayer);

    currentState = EnemyStates.HITTED;
  }


  void SetMoveDirectionToPlayer()
  {
      moveDirection = playerTr.position - charTr.transform.position;// + charController.center;
      moveDirection = moveDirection.normalized;
      moveDirection.y = 0.0f;
      SetRotationToDirection();
      moveDirection.y = -30.8f;
  }
}
