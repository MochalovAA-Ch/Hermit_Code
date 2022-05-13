using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBossScript : IBossBehavior
{
  /*
  Механика босса:
  Бежит за игроком, на определенной дистанции начинает бить
  После удара небольшой отдых, продолжает бежать за игроком
  После трех ударов начинает отдыхать, в это время его можно ударить
    
  После того как босса ударили три раза, он считается побежденным
    */
  enum WolfBossState
  {
    CHASING_PLAYER, //Погоня за игроком
    SHORT_REST,     //Короткий отдых после того как босс ударил игрока
    LONG_REST,      //
    AFTER_HIT_REST,
    HINT,
    HIT,
    HIT_BY_PLAYER
  }

  WolfBossState bossState = WolfBossState.CHASING_PLAYER;

  public float chaseSpeed;
  public float hitDistance;
  CharacterController charController;
  Vector3 moveDirection;
  Transform meshTr;
  Animator animator;
  // Start is called before the first frame update
  void Start()
  {
    charController = GetComponent<CharacterController>();
    meshTr = transform.GetChild(0);
    animator = meshTr.GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    if( Input.GetKeyDown( KeyCode.U) )
    {
      StopAction();
    }

    switch ( bossState )
    {
      case WolfBossState.HINT:
      {
        Hint();
        break;
      }
      case WolfBossState.CHASING_PLAYER:
      {
        ChasePlayer();
        break;
      }
      case WolfBossState.SHORT_REST:
      {
        ShortRest();
        break;
      }
      case WolfBossState.LONG_REST:
      {
        LongRest();
        break;
      }
      case WolfBossState.AFTER_HIT_REST:
      {
        AfterHitRest();
        break;
      }
      case WolfBossState.HIT:
      {
        Hit();
        break;
      }
      case WolfBossState.HIT_BY_PLAYER:
      {
        HitByPlayer();
        break;
      }
    }
  }

  void ChasePlayer()
  {
    GameUtils.Get_XZ_DirectionToPlayer( out moveDirection, transform );
    GameUtils.SetRotationToDirection( meshTr, moveDirection );

    GameUtils.SetAnimState( animator, "ShouldWalk");

    if ( moveDirection.magnitude < hitDistance )
    {
      bossState = WolfBossState.HIT;
      return;
    }

    moveDirection.y = -30.0f;

    charController.Move(moveDirection.normalized * chaseSpeed * Time.deltaTime);

  }


  float shortResTime;
  float shortResTimer;
  void ShortRest()
  {
    if( shortResTimer < shortResTime )
    {
      shortResTimer += Time.deltaTime;
    }
    else
    {
      shortResTimer = 0.0f;
      bossState = WolfBossState.CHASING_PLAYER;
      return;
    }
  }

  float longResTime;
  float longResTimer;
  void LongRest()
  {
    if ( longResTimer < longResTime)
    {
      longResTimer += Time.deltaTime;
    }
    else
    {
      longResTimer = 0.0f;
      bossState = WolfBossState.CHASING_PLAYER;
      return;
    }
  }

  void AfterHitRest()
  {

  }


  float hitTime = 1.0f;
  
  float hitTimer;
  int hitCounts = 3;
  int currentHitCounts = 0;
  bool isWasHit = false;
  void Hit()
  {
    GameUtils.SetAnimState(animator, "ShoulHit");
    if ( hitTimer < hitTime )
    {
      hitTimer += Time.deltaTime;
      if( hitTimer > 0.15 && hitTimer < 0.75 )
      {
        GameUtils.Get_XZ_DirectionToPlayer( out moveDirection, transform );
        if ( moveDirection.magnitude < hitDistance )
        {
          if( !isWasHit )
          {
            GameSystem.playerLives--;
            EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
            EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
            isWasHit = true;
          }
        }
      }
    }
    else
    {
      isWasHit = false;
      hitTimer = 0.0f;
      if( currentHitCounts != hitCounts - 1 )
      {
        currentHitCounts++;
        bossState = WolfBossState.SHORT_REST;
      }
      else
      {
        currentHitCounts = 0;
        bossState = WolfBossState.LONG_REST;
        return;
      }
    }
  }

  void HitByPlayer()
  {

  }

  void Hint()
  {
  }

  public override void StartAction()
  {
    base.StartAction();
  }

  public override void StopAction()
  {
    base.StartAction();
    isDefeated = true;
  }
}
