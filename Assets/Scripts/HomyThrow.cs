using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomyThrow : MonoBehaviour
{
  public static HomyThrow instance;
  AudioSource audioSource;
  public AudioClip launchHomy;
  //public Rigidbody rb;
  public LayerMask layer;
  public Transform cameraTr;
  [SerializeField]
  public float defLaunchForce;
  [SerializeField]
  public float returnSpeed;
  [SerializeField]
  public float deltaY;
  [SerializeField]
  public float deltaX;
  [SerializeField]
  public float minV0x;
  public float turnSpeed;
  public float speed;
  public float deltaReturnX;

  public Transform player;
  Transform playerModel;
  public Animator animContr;
  public Transform homyModel;

  public ParticleSystem partSystem;

  ParticleSystem takeCoinPart;

  Vector3 direction;
  Vector3 verticalVelocity;
  Vector3 motionVector;
  Vector3 startPosition;

  public enum HomyThrowState { FolowPlayer, Launched, ReturnToPlayer, EndLaunchUpdate, ReflectFromSurface, FlyToNoticedObject };

  public SphereCollider homyColl;

  public HomyThrowState homyThrowState;

  Vector3 playerVelocity;

  float V0;
  float V0y;
  float V0x;
  float flyTime;
  float currentFlyTime;
  [SerializeField]
  public float angle;

  float maxFlyTime = 5.0f;
  
  private void Start()
  {
    instance = this;
    audioSource = GetComponent<AudioSource>();
    takeCoinPart = GameObject.Find("takeSmthAnimHomy").GetComponent<ParticleSystem>();
  }

  public bool isFly = false;
  private void Update()
  {
    if( playerModel == null )
    {
      playerModel = SceneGeneralObjects.instance.playerTr.GetChild(0);
    }

    if( Input.GetKeyDown( KeyCode.H ) )
    {
      isFly = false;
    }

    switch ( homyThrowState )
    {
      case HomyThrowState.Launched:
      {
        Fly_Update();
        break;
      }
      case HomyThrowState.ReturnToPlayer:
      {
        ReturnToPlayer_Update();
        break;
      }

      case HomyThrowState.EndLaunchUpdate:
      {
        EndLaunchUpdate();
        break;
      }

      case HomyThrowState.ReflectFromSurface:
      {
        ReflectFromSurface_Update();
        break;
      }

      case HomyThrowState.FlyToNoticedObject:
      {
        FlyToNoticedObject_Update();
        break;
      }
    }
  }

  float endLaunchTime = 0.5f;
  float endLaunchTimer;
  public void EndLaunchUpdate()
  {
    if( endLaunchTimer < endLaunchTime )
    {
      transform.position += direction * speed * Time.deltaTime;
      endLaunchTimer += Time.deltaTime;
    }
    else
    {
      homyThrowState = HomyThrowState.ReturnToPlayer;
    }
  }

  Vector3 launchPoint;
  public void LaunchHomy( )
  {
    PlayLaunchSound();
    //TODO:
    //Прежде чем кидать homy, проверяем не улетит ли он в стену или куда нибудь еще
    direction = playerModel.forward;
    transform.position += direction * speed * Time.deltaTime;
    transform.position = player.position;
    transform.rotation = playerModel.rotation;
    launchPoint = transform.position;
    CalculateFlyParams();
    //CalculateFlyParams2();
    homyThrowState = HomyThrowState.Launched;
    isFly = true;
    GameSystem.isHomyInThrow = true;
  }

  float playerV0x, playerV0y;
  //Здесь расчитываем все переменныее для полета хоми, в зависимости от стартовой скорости хермита
  //Возможно лучше сделать расстояние которое пролетел хоми, чем по таймеру, потому что 
  //при таком подходе можно увеличить/уменьшить скорость без затрагивания дистанции полета
  void CalculateFlyParams()
  {
    flyRotationVector = Vector3.up;
    playerVelocity = CharacterControllerScript.instance.charController.velocity;
    playerV0x = Mathf.Sqrt(playerVelocity.x * playerVelocity.x + playerVelocity.z * playerVelocity.z);
    playerV0y = playerVelocity.y;

    //6 случаев :
    //1.Стоим и кидаем
    //2.Бежим и кидаем
    //3.Прыгаем вверх и кидаем
    //4.Бежим и прыгаем вверх и кидаем
    //5.Падаем вниз и кидаем
    //6.Падаем вниз и вперед и кидаем
    deltaReturnX = 30.0f;
    //1.Стоим и кидаем
    if ( ( playerV0x < 0.1f && playerV0x > -0.1f ) && ( playerV0y < 0.1f && playerV0y > -0.1f ) )
    {
      V0y = 0.0f;
      deltaY = 0.0f;
      deltaX = 30.0f;
      V0x = minV0x * 4 * speed;
      flyTime = (V0x - minV0x*3) / deltaX;
    }
    //2.Бежим и кидаем
    else if( ( playerV0x >= 0.1f ) && ( playerV0y < 0.1f && playerV0y > -0.1f ) )
    {
      if( V0x < minV0x)
      {
        V0x = minV0x * 4 * speed;
      }
      else
      {
        V0x = minV0x * 4 * speed + playerV0x * speed;
      }
      V0y = 0.0f;
      deltaY = 0.0f;
      deltaX = 30.0f;
      flyTime = (V0x - minV0x*3) / deltaX;
    }
    //3.Прыгаем вверх и кидаем
    else if ((playerV0x < 0.1f) && (playerV0y >= 0.1f ) )
    {
      if ( playerV0y < 2.0f )
      {
        V0y = 7.5f * speed;
        deltaY = 30.0f;
      }
      else
      {
        if (playerV0y < 15.0f)
        {
          playerV0y = 15.0f;
          V0y = playerV0y * speed;
          deltaY = 30.0f;
        }
        else
        {
          V0y = playerV0y * speed * ( 0.80f);
          deltaY = 30 + playerV0y-15.0f;
        }
      }
      V0x = minV0x * speed;
      deltaX = 0.0f;
      flyTime = maxFlyTime;
    }
    //4.Бежим и прыгаем вверх и кидаем
    else if ( ( playerV0x >= 0.1f ) && ( playerV0y >= 0.1f ) )
    {
      angle = 80.0f;
      if (playerV0y < 2.0f)
      {
        V0y = 7.5f * speed;
        deltaY = 30.0f;
      }
      else
      {
        if (playerV0y < 15.0f)
        {
          playerV0y = 15.0f;
          V0y = playerV0y * speed;
          deltaY = 30.0f;
        }
        else
        {
          V0y = playerV0y * speed * (0.80f);
          deltaY = 30 + playerV0y - 15.0f;
        }
      }

      if (V0x < minV0x)
      {
        V0x = minV0x * speed;
      }
      else
      {
        V0x = playerV0x * speed;
      }
      deltaX = 0.0f;
      flyTime = maxFlyTime;
    }
    //5.Падаем вниз и кидаем
    else if ((playerV0x < 0.1f) && (playerV0y < 0.1f ) )
    {
      V0y = playerV0y * 0.2f;
      V0x = minV0x * 2 * speed;
      deltaY = 30.0f;
      deltaX = 0.0f;
      flyTime = maxFlyTime;
    }
    //6.Падаем вниз и вперед и кидаем
    else if ( playerV0x > 0.1f && playerV0y < 0.1f )
    {
      V0y = playerV0y * 0.2f;
      if (playerV0x < minV0x)
        V0x = minV0x * 2 * speed;
      else
        V0x = playerV0x * 2 * speed;
      deltaY = 30.0f;
      deltaX = 0.0f;
      flyTime = maxFlyTime;
    }
    verticalVelocity.y = V0y;
    currentFlyTime = 0.0f;
  }

  public void ReturnToPlayer()
  {
    homyThrowState = HomyThrowState.ReturnToPlayer;
  }

  void ReturnToPlayer_Update()
  {
    direction = player.position - transform.position;

    if (direction.magnitude <  returnSpeed * Time.deltaTime )
    {
      homyThrowState = HomyThrowState.FolowPlayer;
    }
    else
    {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, returnSpeed * Time.deltaTime, layer, QueryTriggerInteraction.Collide))
      {
        if (hit.collider.isTrigger)
        {
          if (hit.transform.tag == "Coin")
          {
            GameSystem.collectedCoinsOnLevel++;
            EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
            takeCoinPart.transform.position = hit.transform.position;
            takeCoinPart.Play();
            Destroy(hit.transform.gameObject);
          }
        }
      }
      direction =  player.position - transform.position;
      if (V0x < returnSpeed)
      {
        V0x += deltaReturnX * Time.deltaTime;
      }
      transform.position += direction.normalized * V0x * Time.deltaTime;
      transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }
  }

  float flyToObjectSpeed = 30.0f;
  float flyTOObjDeltaX = 10.0f;
  void FlyToNoticedObject_Update()
  {
    direction = HomyInteractibaleBase.nearstHomyInteractible.transform.position - transform.position;
    motionVector = direction.normalized * V0x;
    if ( direction.magnitude < 2 )
    {
      HomyInteractibaleBase.nearstHomyInteractible.Interract();
      GameSystem.isHomyInThrow = false;
      homyThrowState = HomyThrowState.ReflectFromSurface;
      direction = Vector3.up;
      deltaY = 30.0f;
      verticalVelocity.y = V0x/3;
      V0x = 0;
      isFly = false;
      partSystem.gameObject.transform.position = HomyInteractibaleBase.nearstHomyInteractible.transform.position;
      partSystem.Play();
      return;
    }

    if (V0x > flyToObjectSpeed + flyTOObjDeltaX * Time.deltaTime )
    {
     // V0x -= flyTOObjDeltaX * Time.deltaTime;
    }

    transform.rotation = Quaternion.LookRotation(motionVector) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
    transform.position += direction.normalized * V0x * Time.deltaTime;
  }

  Vector3 flyRotationVector;
  float rotationAngle = 0.0f;
  void Fly_Update()
  {
    RaycastHit hit;
    if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, speed * Time.deltaTime, layer, QueryTriggerInteraction.Collide))
    {
      if( hit.collider.isTrigger )
      {
        if (hit.transform.tag == "Coin")
        {
          GameSystem.collectedCoinsOnLevel++;
          EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
          takeCoinPart.transform.position = hit.transform.position;
          takeCoinPart.Play();
          Destroy( hit.transform.gameObject );
        }
        else
        if (hit.transform.tag == "Key")
        {
          if (hit.transform.name.Contains("aether"))
            GameSystem.collectedKeyIndex = Keys.AETHER;
          else if (hit.transform.name.Contains("water"))
            GameSystem.collectedKeyIndex = Keys.WATER;
          else if (hit.transform.name.Contains("fire"))
            GameSystem.collectedKeyIndex = Keys.FIRE;
          else if (hit.transform.name.Contains("ground"))
            GameSystem.collectedKeyIndex = Keys.GROUND;
          else if (hit.transform.name.Contains("air"))
            GameSystem.collectedKeyIndex = Keys.AIR;

          GameSystem.playerKeys++;
          EventsManager.TriggerEvent(EventsIds.CHANGE_KEYS_COUNT);

          takeCoinPart.transform.position = hit.transform.position;
          takeCoinPart.Play();
          Destroy(hit.transform.gameObject);
        }
        else
        if (hit.transform.tag == "Live")
        {
          GameSystem.playerLives++;
          EventsManager.TriggerEvent(EventsIds.INCREASE_LIVES);
          takeCoinPart.transform.position = hit.transform.position;
          takeCoinPart.Play();
          Destroy(hit.transform.gameObject);
        }
        else 
        if( hit.transform.tag == "Enemy" )
        {
          
        }
      }
      else
      {
        
      }
    }
    else
    {

    }

    if( isFly )
    {
     if( currentFlyTime < flyTime )
      {
        motionVector = direction.normalized * V0x + verticalVelocity;
        if ( CheckCollision( motionVector ) )
        {
          GameSystem.isHomyInThrow = false;
          homyThrowState = HomyThrowState.ReflectFromSurface;
          direction = Vector3.up;
          float reflVel = Mathf.Sqrt(V0x * V0x + verticalVelocity.y * verticalVelocity.y);
          deltaY = 30.0f;
          verticalVelocity.y = reflVel/3;
          V0x = 0;
          return;
        }
        transform.position += motionVector * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(motionVector) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        V0x -= deltaX * Time.deltaTime;
        verticalVelocity.y -= deltaY * Time.deltaTime;
        currentFlyTime += Time.deltaTime;
      }
      else
      {
        Debug.Log("flyedDistance : " + GameUtils.getDistanceByXZ(launchPoint, transform.position));
        GameSystem.isHomyInThrow = false;
        homyThrowState = HomyThrowState.ReturnToPlayer;
      }
    }
    else
    {
      homyThrowState = HomyThrowState.EndLaunchUpdate;
      endLaunchTimer = 0.0f;
    }
  }

  float collisionVx;
  float collisionVy;
  void ReflectFromSurface_Update()
  {
    if( verticalVelocity.y < 0 )
    {
      verticalVelocity.y = 0.0f;
      homyThrowState = HomyThrowState.ReturnToPlayer;
      return;
    }
    direction = player.position - transform.position;
    motionVector = direction.normalized * V0x + Vector3.up * verticalVelocity.y;
    transform.position += motionVector*Time.deltaTime;
    transform.rotation = Quaternion.LookRotation(motionVector) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
    verticalVelocity.y -= deltaY * Time.deltaTime;
    V0x += ( deltaReturnX )* Time.deltaTime;
  }


  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log("Collision");
    if (collision.transform.tag == "Enemy")
    {
      IEnemy enemy = collision.transform.GetComponent<IEnemy>();
      enemy.GetHit();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Enemy" )
    {
      IEnemy enemy = other.GetComponent<IEnemy>();
      enemy.GetHit();
    }
  }

  RaycastHit collisionHit;
  bool  CheckCollision( Vector3 collisionDirVector)
  {
    if(Physics.SphereCast(transform.position, 0.5f, motionVector.normalized, out collisionHit, motionVector.magnitude * Time.deltaTime, layer, QueryTriggerInteraction.Ignore) )
    {
      partSystem.gameObject.transform.position = collisionHit.point;
      partSystem.Play();

      DestroyObjectScript destObj = collisionHit.transform.GetComponent<DestroyObjectScript>();
      if (destObj)
      {
        destObj.DestroyObject();
        endLaunchTimer = 0.0f;
        homyThrowState = HomyThrowState.EndLaunchUpdate;
        return true;
      }

      HomyInteractibleObject interObject = collisionHit.transform.GetComponent<HomyInteractibleObject>();
      if (interObject)
      {
        interObject.Interract();
        return true;
      }

      if (collisionHit.transform.tag == "Enemy")
      {
        IEnemy enemy = collisionHit.transform.GetComponent<IEnemy>();
        enemy.GetHit();
        endLaunchTimer = 0.0f;
        homyThrowState = HomyThrowState.EndLaunchUpdate;
        return true;
      }
      else
      {
        endLaunchTimer = 0.0f;
        homyThrowState = HomyThrowState.EndLaunchUpdate;
        return true;
      }


      return true;
    }
    return false;
  }

  void PlayLaunchSound()
  {
    return;
    audioSource.clip = launchHomy;
    audioSource.Play();
  }

  public void SetNoticedObjectState()
  {
    isFly = false;
    homyThrowState = HomyThrowState.FlyToNoticedObject;
  }

  void Launch()
  {
    //StartCoroutine(ParabolicMovementCorutine());
  }
}
