#define MOVE_WITH_JOYSTICK_0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControllerScript : MonoBehaviour
{
  //Длинна максимального двойного прыжка 24,5 юнита

  public LayerMask layer;
  public static CharacterControllerScript instance;
  public InputController input;
  public bool isGroundedAnimPlayed = true;       //Проигралась ли анимация падения
  public bool isInWardrobe = false;

  Vector3 prevPosition;
  public Vector3 playerOuterVelocity;
  public Vector3 jumpStartPoint;

  //Transform staff;

  public Animator playerAnimator;  //Аниматор персонажа ( используется для переключения анимации )
  public Material playerMaterial;         //Материал персонажа ( используется для отображения различных эффектов )
  public Transform cameraTransform;
  public JoystickController joystick;
  public Vector3 charContBottomOffset;    //Смещение низа капсулы charController относительно центра
  public Transform charTrPublic;
  ParticleSystem hitParticlesAnim;
  ParticleSystem jump_jump2_anim;
  public ParticleSystem runPartSystem;
  ParticleSystem takeSmthPartSystem;
  ParticleSystem swimParticles;
  public ThirdPersonOrbitCam cameraScript;

  MeshRenderer[] playerMeshRenderers;
  SkinnedMeshRenderer[] playerSkinnedMeshRenderer;

  public float walkSpeed;                 //Текущая скорость движения
  float defaultWalkSpeed = 8;            //Скорость движения по умолчанию                    
  public float turnSpeed;                 //Скорость поворота
  public float customSlopeLimit;          //Предел угла наклона поверхности, на которых мы можем стоять
  public float jumpHeight;                //Максимальная высота на которую может прыгнуть персонаж
  public float jumpSpeed;                 //Скорость прыжка( с какой скоростью достигнем макимальной дистанции прыжка)
  public float gravityForce = 9.8f;       //Сила притяжения
  public float slideAcceleration;
  public float climbSpeed = 10.0f;
  public float climbJumpOffset = 10.0f;
  public float knockbackSpeed;
  public float moveObjectsSpeed;
  public float flowerJumpHeight;
  public float keyboardTurnSpeed;
  public float pushForce;
  public Transform carryPoint;
  public Image hitEffectImage;
  public bool isTelekenesisState;

  bool isWalkOnDirt;

  Vector3 moveDirection;                  //Направление  движения персонажа
  Vector3 verticalVelocity;               //Вериткальная скорость
  Vector3 hitNormal;                      //Нормаль к поверхности, с которой соприкосаемся
  Vector3 climbVector;                    //Вектор движения по скалам
  Vector3 moveOffsetVector;               //Вектор влияния внешних сил на игрока
  Vector3 platformOffsetVector;           //Вектор смещения при нахождении на двигающейся платформе
  Vector3 hitchRayCastOffset;             //Смещение относительно позиции CharController, из которой мы проверям возможность зацепиться  за выступ
  Vector3 hitchSphereCastOffset;
  Vector3 hitchPrevPos;
  Vector3 hitchCurrPos;
  ControllerColliderHit hitTest;
  Color hitEffectCol;
  Color deltaHitEffectCol;

  public static Vector3 knockbackVector;  //Вектор движения при отталкивании
  FloatingPlatform movingPlatform;
  public CharacterController charController;     //Контроллер персонажа
  Transform hitchTransform;
  GameObject carryable;
  ControllerColliderHit charContrHit;

  movingObjectsScript movingObject;
  Vector2 cameraTransformForwardXZ;

  HermitSoundManager hermitSounds;

  RaycastHit climbHit;
  RaycastHit climbCheckHit;

  float movingAxis = 0.0f;                //Кнопки W S ( от -1 до 1 )
  float swimOnSurfaceOffset = 1.0f;

  bool canHit = true;                     //Флаг возомжности удара
  bool isKnockback = false;
  bool isSwimming = false;
  bool isSwimOnSurface = false;
  bool shouldSlide = false;
  bool isCarrySomething = false;

  Vector2 keyboardInputAxis;

  float swimSurfaceY = 0.0f;
  public static bool isClimbing = false;  //Флаг состояния карабканья
  public static bool isJumpingFromClimb = false;
  bool shouldMoveObject = false;

  Vector3 motion;                         //Вектор движения
  float angleRot = 0;
  float camAngle = 0;

  List<GameObject> takingCoinsList;
  List<float> takingCoinsSpeed;

  List<GameObject> takinObjectsList;
  List<float> takinObjectsSpeed;

  Quest quest;
  Hint hint;

  float collisionTime;
  float collisionTimer = 0.1f;
  public bool wasCollision = false;

  List<Collider> hitColliders;

  public Transform pickUp;
  Vector3 pickUpVector;

  PickableObject pickableObject;

  float fallJumpTime = 0.2f;
  float fallJumpTimer = 0.0f;

  public struct PlayerAnimStates
  {
    public int IDLE;
    public int RUN;
    public int WALK;
    public int GROUND;
    public int JUMP_FIRST;
    public int JUMP_SECOND;
    public int FALL;
    public int HIT;
    public int CLIMB;
    public int SWIM;
    public int HIT2;
    public int HIT3;
    public int HIT4;
    public int HITCH;
    public int PICK_UP;
    public int ROLL_UP;
    public int STATES_COUNT;

    public void Init()
    {
      IDLE = 0;
      RUN = 1;
      WALK = 2;
      GROUND = 3;
      JUMP_FIRST = 4;
      JUMP_SECOND = 5;
      FALL = 6;
      HIT = 7;
      CLIMB = 8;
      SWIM = 9;
      HIT2 = 10;
      HIT3 = 11;
      HIT4 = 12;
      HITCH = 13;
      PICK_UP = 14;
      ROLL_UP = 15;
      STATES_COUNT = 16;
    }
  };

  PlayerAnimStates playerAnimStates;
  int currentAnimState;
  public void SetPlayerAnimState( int stateId, float speed )
  {
    playerAnimator.speed = speed;
    if (currentAnimState == stateId )
    {
      return;
    }

    playerAnimator.SetBool(currentAnimState, false);
    playerAnimator.SetBool(stateId, true);
    currentAnimState = stateId;
    /*for (int i = 0; i < playerAnimator.parameterCount; i++)
    {
      AnimatorControllerParameter param = playerAnimator.parameters[i];
      if (i == (int)state)
      {
        //Debug.Log( param.)

        //playerAnimator.SetBool(param.name, true);
      }
      else
      {
        
        //playerAnimator.SetBool(param.name, false);
      }
    }*/
  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.DECREASE_LIVES, StartUnvulnerableState);
    //EventsManager.StartListening(EventsIds.KNOCKBACK, KnockBack);
  }

  private void OnDisable()
  {

  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.DECREASE_LIVES, StartUnvulnerableState);
    //EventsManager.StopListening(EventsIds.KNOCKBACK, KnockBack);
  }

  void Awake()
  {
    Application.targetFrameRate = 60;
  }

  // Use this for initialization
  void Start()
  {
    InitAnimParameters();
    currentAnimState = playerAnimStates.IDLE;
    prevPosition = transform.position;
    swimParticles = GameObject.Find("swim_part").GetComponent<ParticleSystem>(); ;
    //runPartSystem = GameObject.Find("RunParticleSystem").GetComponent<ParticleSystem>();
    hitParticlesAnim = GameObject.Find("hitParticlesAnim").GetComponent<ParticleSystem>();
    runMainModule = runPartSystem.main;
    jump_jump2_anim = GameObject.Find("jump_jump2_effect").GetComponent<ParticleSystem>();
    takeSmthPartSystem = GameObject.Find("takeSmthAnim").GetComponent<ParticleSystem>();
    //staff = GameObject.FindGameObjectWithTag("Staff").GetComponent<Transform>();
    hitColliders = new List<Collider>();
    hermitSounds = GetComponent<HermitSoundManager>();
    Time.timeScale = 1.0f;
    GameSystem.playerKeys = 0;
    GameSystem.diedInGameZone = false;
    input = GetComponent<InputController>();
    ChangeState( PlayerStates.MOVING );
    instance = this;
    playerAnimator = charTrPublic.GetComponent<Animator>();
    playerMeshRenderers = charTrPublic.GetComponentsInChildren<MeshRenderer>();
    playerSkinnedMeshRenderer = charTrPublic.GetComponentsInChildren<SkinnedMeshRenderer>();
    takingCoinsList = new List<GameObject>();
    takingCoinsSpeed = new List<float>();
    takinObjectsList = new List<GameObject>();
    takinObjectsSpeed = new List<float>();
    //При старте сцены устанавливаем ссылки на необходимые компоненты
    charController = GetComponent<CharacterController>();
    verticalVelocity.y = -gravityForce;
    moveDirection = charTrPublic.forward;
    pickUpVector = charTrPublic.forward;

    hitchRayCastOffset.y = charController.center.y + charController.bounds.size.y / 4;
    hitchSphereCastOffset.y = hitchRayCastOffset.y + 0.5f;

    GameSystem.invulnerableTime = 3.0f;
    GameSystem.currentUnvulnerableTime = 0.0f;
    GameSystem.playerCanBeHitted = true;

    defaultWalkSpeed = walkSpeed;
    hitEffectCol = hitEffectImage.color;
    deltaHitEffectCol = new Color(0.0f, 0.0f, 0.0f, hitEffectCol.a / (hitImageEffectTime / Time.deltaTime));
    EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);

    // GameSystem.coinsOnLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
    // GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<DestroyObjectScript>().Length * 5;
    //GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<OpenChestScript>().Length * 25;

    GameSystem.coinsOnLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
    GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<OpenChestScript>().Length * 25;
    GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<DestroyObjectScript>().Length * 5;
    //Debug.Log(GameSystem.coinsOnLevel);

#if MOVE_WITH_JOYSTICK
    turnSpeed = 1000;
#else
    turnSpeed = 300;
#endif
  }

  enum PlayerStates
  {
    MOVING,
    JUMPING,
    FALLING,
    CLIMBING,
    SWIMMING,
    HITTING,
    JUMP_FROM_CLIMB,
    HITCH_ON_CLIMB,
    DOUBLE_JUMP,
    KNOCKBACK,
    SLIDING,
    JUMP_ON_PLAYER,
    DIALOG,
    WARDROBE,
    HINT,
    STOP,
    PICK_UP,
    ROLL_OVER,
    HOMY_AIM,
    INACTIVE,
    FLY_WINGS
  }

  PlayerStates playerState, prevState;

  private void FixedUpdate()
  {
    //pushinPointRb.MovePosition(charController.transform.position + charTrPublic.forward * 2);
  }

  public float staffHitTimer;
  float currStaffHitTime;

  // Update is called once per frame
  bool homyThrow_Input = false;
  void Update()
  {
    //DrawHitchSphere();

    if (Input.GetKeyDown(KeyCode.H))
    {
      GameSystem.coinsOnLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
      GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<OpenChestScript>().Length * 25;
      GameSystem.coinsOnLevel += GameObject.FindObjectsOfType<DestroyObjectScript>().Length * 5;
      Debug.Log(GameSystem.coinsOnLevel);
    }

    if (Input.GetKeyDown(KeyCode.U))
    {
      GameSystem.totalCoins += 10000;
      EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
    }

    rollOverCurrentCd += Time.deltaTime;

    if (Input.GetKeyDown(KeyCode.V))
    {
      //RollOver();
      //rollOverTimer = 0.0f;
      //timer
      //playerState = PlayerStates.ROLL_OVER;
      /*pickUpVector = charTrPublic.forward;
      //moveVector = charTrPublic.forward;
      playerState = PlayerStates.PICK_UP;
      //Debug.Log(GameSystem.playerKeys);*/
    }

    if (Input.GetKeyDown(KeyCode.O))
    {
      GameSystem.playerKeys = 5;
    }

    if (Input.GetKeyDown(KeyCode.M))
    {
      Debug.Log("TotalCoins" + GameSystem.coinsOnLevel);
    }

    if (Input.GetKeyDown(KeyCode.B))
    {
      GameSystem.playerLives += 5;
      EventsManager.TriggerEvent(EventsIds.INCREASE_LIVES);
    }

    if (Input.GetKeyDown(KeyCode.T))
    {
      transform.position = FindObjectOfType<EndLevelStand>().transform.position + Vector3.up * 6;
    }

    if (isInWardrobe)
    {
      ChangeState(PlayerStates.WARDROBE);
    }

    if (Input.GetKeyDown(KeyCode.J))
    {
      GameSystem.withJoystick = true;
    }

    if (Input.GetKeyDown(KeyCode.K))
    {
      GameSystem.withJoystick = false;
    }

    switch (playerState)
    {
      case PlayerStates.MOVING:
        {
          Move_Update();
          break;
        }
      case PlayerStates.FALLING:
        {
          Fall_Update();
          break;
        }
      case PlayerStates.JUMPING:
        {
          Jump_Update();
          break;
        }
      case PlayerStates.DOUBLE_JUMP:
        {
          DoubleJump_Update();
          break;
        }
      case PlayerStates.SLIDING:
        {
          Sliding_Update();
          break;
        }
      case PlayerStates.SWIMMING:
        {
          Swim_Update();
          break;
        }
      case PlayerStates.HITCH_ON_CLIMB:
        {
          Hitch_Update();
          break;
        }
      case PlayerStates.CLIMBING:
        {
          Climb_Update();
          break;
        }
      case PlayerStates.DIALOG:
        {
          Dialog_Update();
          break;
        }
      case PlayerStates.WARDROBE:
        {
          Wardrobe_Update();
          break;
        }

      case PlayerStates.HINT:
        {
          Hint_Update();
          break;
        }

      case PlayerStates.STOP:
        {
          Stop_Update();
          break;
        }

      case PlayerStates.PICK_UP:
        {

          PickUp_Update();
          break;
        }
      case PlayerStates.ROLL_OVER:
        {

          RollOver_Update();
          break;
        }
      case PlayerStates.HOMY_AIM:
        {
          HomyThrow_Update();
          break;
        }
      case PlayerStates.FLY_WINGS:
        {
          Fly_Wings_Update();
          break;
        }
    }

    playerOuterVelocity = transform.position - prevPosition;
    prevPosition = transform.position;

    /* if (Input.GetKeyDown(KeyCode.U))
       Time.timeScale = 0.1f;
     if (Input.GetKeyDown(KeyCode.I))
       Time.timeScale = 1.0f;*/

    /*if (input.Info.hitInput)
    {
      if (quest != null)
      {
        if (quest.status == QuestStatus.Completed)
        {

        }
        else if (quest.status == QuestStatus.Done)
        {
          CompleteQuestDialog();
        }
        else
        {
          StartQuestDialog();
        }
        input.Info.hitInput = false;
      }

      if (hint != null)
      {
        hint.ShowHint();
        playerState = PlayerStates.HINT;
        input.Info.hitInput = false;
      }
    }*/

    if (quest != null)
    {
      if (CheckObjectForInterraction(quest.transform))
      {
        CheckQuest();
        GameUIController.instance.SetTalkIcon();
      }
    }
    else if (hint != null)
    {
      if (CheckObjectForInterraction(hint.transform))
      {
        CheckHint();
        hint.outline.enabled = true;
        GameUIController.instance.SetInterractIcon();
      }
      else
      {
        if( hint.outline != null )
          hint.outline.enabled = false;
      }
    }

    //Анимируем сбор монет
    TakeCoinAnim();
    TakeObjectAnim();

    //если время с момента последней колизии прошло немного, то считаем что колизия была
    if (collisionTime < collisionTimer)
    {
      wasCollision = true;
      collisionTime += Time.deltaTime;
    }
    //иначе  говорим что колизии небыло
    else
    {
      wasCollision = false;
    }
    //  Debug.Log(hitColliders.Count);

    hitColliders.Clear();
  }

  float slideAngle = 0.0f;
  bool CheckForSliding()
  {
    /* if( !wasCollision )
     {
       return false;
     }*/

    slideAngle = Vector3.Angle(Vector3.up, hitNormal);
    return (Vector3.Angle(Vector3.up, hitNormal) >= customSlopeLimit);
  }

  public void StartUnvulnerableState()
  {
    if (GameSystem.playerCanBeHitted)
    {
      hermitSounds.HitedSound();
      StartCoroutine("unvurnelableState");
      StartCoroutine("hitImageEffect");
    }
  }

  IEnumerator unvurnelableState()
  {
    GameSystem.currentUnvulnerableTime = 0.0f;
    GameSystem.playerCanBeHitted = false;
    GameSystem.playerCanRestoreLives = false;
    while (GameSystem.currentUnvulnerableTime < GameSystem.invulnerableTime)
    {
      GameSystem.currentUnvulnerableTime += Time.deltaTime;
      ChangeColor();
      yield return null;
    }

    for (int i = 0; i < playerMeshRenderers.Length; i++)
    {
      playerMeshRenderers[i].enabled = true;
    }
    for (int i = 0; i < playerSkinnedMeshRenderer.Length; i++)
    {
      playerSkinnedMeshRenderer[i].enabled = true;
    }
    MeshRendStateEnabled = false;
    GameSystem.playerCanBeHitted = true;
    /*GameSystem.playerCanRestoreLives = true;
    if ( GameSystem.playerLives < 3 && GameSystem.playerLives > 0 )
    {
      StartCoroutine("restoreLives");
    }*/
  }

  float deltaMoveOffsetFromClimb = 2.0f;
  IEnumerator JumpFromClimb2()
  {
    while (moveOffsetVector.magnitude > 0)
    {
      if (moveOffsetVector.magnitude - deltaMoveOffsetFromClimb * Time.deltaTime < 0)
      {
        moveOffsetVector = Vector3.zero;
        yield break;
      }
      moveOffsetVector -= moveOffsetVector * deltaMoveOffsetFromClimb * Time.deltaTime;
      yield return null;
    }

    moveOffsetVector = Vector3.zero;
  }

  //Спрыгиваем некоторое время со скалы, отдельная логика
  float jumpFromClimbTimer = 0.5f;
  float currJumpFromClimbTimer = 0.0f;
  IEnumerator JumpFromClimb()
  {
    if (isJumpingFromClimb)
      yield return null;

    EventsManager.TriggerEvent(EventsIds.JUMP_FROM_CLIMB);
    ChangeState(PlayerStates.JUMP_FROM_CLIMB);
    isJumpingFromClimb = true;
    currJumpFromClimbTimer = 0.0f;
    while (currJumpFromClimbTimer < jumpFromClimbTimer)
    {
      verticalVelocity.y -= gravityForce * Time.deltaTime;
      currJumpFromClimbTimer += Time.deltaTime;
      yield return null;
    }
    isClimbing = false;
    canJump = false;
    canDoubleJump = true;
    isJumpingFromClimb = false;
    EventsManager.TriggerEvent(EventsIds.STOP_SMOOTH_CAM_BEHIND_PLAYER);
  }

  IEnumerator restoreLives()
  {
    GameSystem.currentRestoreLivesTime = 0.0f;
    if (!GameSystem.playerCanRestoreLives)
    {
      yield return null;
    }

    while (GameSystem.playerLives != 3 && GameSystem.playerCanRestoreLives)
    {
      if (GameSystem.currentRestoreLivesTime < GameSystem.restoreLivesTime)
      {
        GameSystem.currentRestoreLivesTime += Time.deltaTime;
      }
      else
      {
        GameSystem.currentRestoreLivesTime = 0;
        GameSystem.playerLives++;
        EventsManager.TriggerEvent(EventsIds.INCREASE_LIVES);
      }
      yield return null;
    }
  }

  public void StaffHit()
  {
    if (!StaffController.isStaffInHitt)
    {
      EventsManager.TriggerEvent(EventsIds.STAFF_HIT);
      playerAnimator.Play("Hit");
      StaffSounds.instance.StaffSwipe();
    }
  }

  public void OnHitEnded()
  {
    playerAnimator.SetBool("isHit", false);
  }

  private void OnTriggerEnter(Collider other)
  {

    if (other.tag == "GameZone")
    {
      if (GameSystem.currentLevel != 1)
      {
        GameSystem.playerLives--;
        if (GameSystem.playerLives <= 0)
          GameSystem.diedInGameZone = true;
        EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
        ChangeState(PlayerStates.MOVING);
      }
      GameController.instance.SetPlayerToCheckpoint(transform);
      return;
    }

    if (other.tag == "FinishTrigger")
    {
      SceneLoader.instance.LoadLevel(1);
      return;
    }

    if (other.tag == "Live")
    {
      hermitSounds.GetLive();
      other.gameObject.GetComponent<Collider>().enabled = false;
      takinObjectsList.Add(other.gameObject);
      takinObjectsSpeed.Add(objectTakeSpeed);
      return;
    }

    if (other.tag == "MinusLivesArea")
    {
      if (GameSystem.playerCanBeHitted)
      {
        CharacterControllerScript.knockbackVector = charTrPublic.forward;
        GameSystem.playerLives--;
        EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
        EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
        //KnockBack();
      }
      return;
    }

    if (other.tag == "Coin")
    {
      hermitSounds.GetCoin();
      other.gameObject.GetComponent<Collider>().enabled = false;
      takingCoinsList.Add(other.gameObject);
      takingCoinsSpeed.Add(coinTakeSpeed);
      return;
    }

    if (other.tag == "Key")
    {
      hermitSounds.GetKey();
      other.gameObject.GetComponent<Collider>().enabled = false;
      takinObjectsList.Add(other.gameObject);
      takinObjectsSpeed.Add(objectTakeSpeed);
    }

    if (other.tag == "DirtArea")
    {
      walkSpeed = defaultWalkSpeed / 2;
      isWalkOnDirt = true;
      return;
    }

    if (other.tag == "ClimbWall")
    {
      if (!isJumpingFromClimb)
      {
        isClimbing = true;
        ChangeState( PlayerStates.CLIMBING );
        verticalVelocity.y = 0.0f;
        moveOffsetVector = Vector3.zero;
        charTrPublic.rotation = other.transform.rotation * Quaternion.Euler(0.0f, -90.0f, 0.0f);
        ThirdPersonOrbitCam.instance.SetDefaultCameraState();
        SetClimbPosition(other);
        EventsManager.TriggerEvent(EventsIds.JUMP_ON_CLIMB);
      }
    }

    if (other.tag == "JumpPlatform")
    {
      Animator anim = other.GetComponent<Animator>();
      anim.Play("jump_flower");
      verticalVelocity.y = flowerJumpHeight;
      canJump = false;
      canDoubleJump = true;
      StopRunParticles();
      ThirdPersonOrbitCam.instance.SetJumpState();
      ChangeState( PlayerStates.JUMPING );
      PlayPartSystem(jump_jump2_anim, transform.position);
      hermitSounds.FlowerJump();
    }

    if (other.tag == "MovingObject")
    {
      transform.parent = other.transform;
    }

    if (other.tag == "MovableObject")
    {

    }

    if (other.tag == "SwimArea")
    {
      ChangeState( PlayerStates.SWIMMING );
      swimParticles.gameObject.SetActive(true);
      ThirdPersonOrbitCam.instance.SetDefaultCameraState();
      isSwimming = true;
      Collider swimTrigger = other.GetComponent<Collider>();
      verticalVelocity.y = 0;
      swimSurfaceY = other.transform.position.y + swimTrigger.bounds.size.y * 0.5f;
      if (transform.position.y - swimOnSurfaceOffset < swimSurfaceY)
      {
        isSwimOnSurface = false;
      }
      else
      {
        transform.position = new Vector3(transform.position.x, swimSurfaceY - swimOnSurfaceOffset,
        transform.position.z);
        isSwimOnSurface = true;
      }
    }

    if (other.tag == "Quest")
    {
      GameUIController.instance.SetTalkIcon();
      quest = other.GetComponent<Quest>();
    }

    if (other.tag == "Hint")
    {
      hint = other.GetComponent<Hint>();
      if (hint.isTriggerHint && !hint.isClosed)
      {
        if (!hint.IsAlreadyPlayed())
        {
          hint.ShowHint();
          ChangeState(PlayerStates.HINT);
          hermitSounds.StopSound();
        }
      }
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.tag == "MinusLivesArea")
    {
      if (GameSystem.playerCanBeHitted)
      {
        GameSystem.playerLives--;
        EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
        EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
      }
    }

    if (other.tag == "SwimArea")
    {
      isSwimming = true;
      moveOffsetVector = other.transform.forward * 5;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "DirtArea")
    {
      walkSpeed = defaultWalkSpeed;
      isWalkOnDirt = false;
      return;
    }

    if (other.tag == "ClimbWall")
    {
      if (!isJumpingFromClimb)
      {
        isClimbing = false;
        verticalVelocity.y = -gravityForce * Time.deltaTime;
        EventsManager.TriggerEvent(EventsIds.STOP_SMOOTH_CAM_BEHIND_PLAYER);
      }
    }

    if (other.tag == "MovingPlatform")
    {
      movingPlatform = null;
      transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      platformOffsetVector = Vector3.zero;
    }

    if (other.tag == "JumpPlatform")
    {
      jumpHeight = 15.0f;
    }

    if (other.tag == "MovingObject")
    {
      transform.parent = null;
      transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    if (other.tag == "SwimArea")
    {
      isSwimming = false;
      moveOffsetVector = Vector3.zero;
    }

    if (other.tag == "Quest")
    {
      quest = null;
      GameUIController.instance.HideInterractIcon();
    }

    if (other.tag == "Hint")
    {
      if(hint != null )
      {
        hint.outline.enabled = false;
        GameUIController.instance.HideInterractIcon();
        hint = null;
      }
    }
  }

  private void OnControllerColliderHit(ControllerColliderHit hit)
  {
    hitColliders.Add(hit.collider);
    //if (playerState == PlayerStates.SLIDING)
    // Debug.Log("slideStateCollision " + Time.time);
    //Debug.Log("contrHit " + Time.time);
    collisionTime = 0.0f;

    if (hit.gameObject.tag == "MovingObject")
    {

    }

    if (hit.gameObject.tag == "Enemy")
    {
      CharacterController enemyCharCnt = hit.gameObject.GetComponent<CharacterController>();

      if (charController.transform.position.y - charController.height * 0.5f > enemyCharCnt.transform.position.y + enemyCharCnt.height * 0.5f - 0.2f)
      {
        PlayPartSystem(hitParticlesAnim, hit.point);
        IEnemy enemy = hit.gameObject.GetComponent<IEnemy>();
        enemy.GetHit();
        canDoubleJump = true;
        ChangeState( PlayerStates.JUMPING );
        PlayPartSystem(jump_jump2_anim, charTrPublic.position - Vector3.up);
        verticalVelocity.y = jumpHeight;
      }
    }

    hitNormal = hit.normal;
    slideAngle = Vector3.Angle(Vector3.up, hitNormal);
  }

  private float changeColorTime = 0.2f;
  private float currentChangeTime = 0.0f;

  Color colOrig = Color.white;

  Color colRed = Color.red;

  bool MeshRendStateEnabled = false;
  //Меняет цвет материала игрока
  private void ChangeColor()
  {
    if (currentChangeTime < changeColorTime)
    {
      currentChangeTime += Time.deltaTime;
    }
    else
    {
      for (int i = 0; i < playerMeshRenderers.Length; i++)
      {
        playerMeshRenderers[i].enabled = MeshRendStateEnabled;
      }
      for (int i = 0; i < playerSkinnedMeshRenderer.Length; i++)
      {
        playerSkinnedMeshRenderer[i].enabled = MeshRendStateEnabled;
      }
      currentChangeTime = 0.0f;
      MeshRendStateEnabled = !MeshRendStateEnabled;
    }
  }

  //Проверяем, можем ли мы зацепится за  выступ
  bool isStartedHitch = false;
  bool checkHitch()
  {
    if (Physics.Raycast(charController.transform.position + hitchRayCastOffset, charTrPublic.forward, out climbHit, 1.0f))
    {
      //Не рассматриваем триггеры
      if (climbHit.collider.isTrigger || climbHit.collider.tag == "PushingPoint")
      {
        isStartedHitch = false;
        return false;
      }

      //if (!Physics.SphereCast(charController.transform.position + hitchSphereCastOffset - charTrPublic.forward, 0.2f, charTrPublic.forward, out climbCheckHit, 1.0f, layer ))
      if (!Physics.CheckCapsule(charController.transform.position + hitchSphereCastOffset, charController.transform.position + hitchSphereCastOffset + charTrPublic.forward, 0.2f, layer))
      {
        //Можем зацепится
        hitchTransform = climbHit.collider.transform;
        Vector3 hitVectorXY = -new Vector3(climbHit.normal.x, 0.0f, climbHit.normal.z);
        float angle = Vector3.SignedAngle(hitVectorXY, charTrPublic.forward, Vector3.up);
        verticalVelocity.y = 0.0f;
        ChangeState( PlayerStates.HITCH_ON_CLIMB );
        ThirdPersonOrbitCam.instance.SetDefaultCameraState();
        if (!isStartedHitch)
        {
          hitchCurrPos = hitchTransform.position;
          hitchPrevPos = hitchTransform.position;
          isStartedHitch = true;
        }
        Debug.Log(climbCheckHit.collider);
        return true;
      }
    }
    isStartedHitch = false;
    return false;
  }

  void KnockBack()
  {
    return;
    currKnockbackTimer = 0.0f;
    StartCoroutine("KnockBackCoroutine");
  }

  float currKnockbackTimer = 0.0f;
  float knockbackTime = 0.3f;
  IEnumerator KnockBackCoroutine()
  {
    if (isKnockback)
      yield break;

    isKnockback = true;
    currKnockbackTimer = 0.0f;
    while (currKnockbackTimer < knockbackTime)
    {
      currKnockbackTimer += Time.deltaTime;
      yield return null;
    }
    isKnockback = false;
  }

  void OnParticleCollision(GameObject other)
  {
    //Debug.Log("ParticleCollision");
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(charTrPublic.position + hitchSphereCastOffset + charTrPublic.forward * 1.0f, 0.1f);
  }

  //Устанавливаем начальную позицию для лазанья
  void SetClimbPosition(Collider other)
  {
    BoxCollider boxCollider = other.gameObject.GetComponent<BoxCollider>();
    if (boxCollider == null)
      return;

    float halfXwidth = boxCollider.size.x * 0.5f;
    Plane plane = new Plane(other.transform.right, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
    float distance = plane.GetDistanceToPoint(charController.transform.position);

    if (distance < halfXwidth)
    {
      charController.transform.position += other.transform.right * (halfXwidth - distance);
    }
  }

  void RotatePlayer(bool isWithJoystick)
  {
    if (isWithJoystick)
    {
      if (joystick.InputDirection.magnitude > 0)
        charTrPublic.rotation = Quaternion.Euler(0.0f, camAngle + angleRot, 0.0f);
      movingAxis = joystick.InputDirection.magnitude;
    }
    else
    {
      charTrPublic.rotation = Quaternion.Euler(0.0f, camAngle + angleRot, 0.0f);
      movingAxis = Input.GetAxis("Vertical");
    }
  }

  float smoothTurnSpeed = 3.0f;
  void RotatePlayerSmooth( bool isWithJoystick )
  {
    if (isWithJoystick)
    {
      if (joystick.InputDirection.magnitude > 0)
        charTrPublic.rotation = Quaternion.Euler(0.0f, Mathf.LerpAngle(charTrPublic.rotation.eulerAngles.y, camAngle + angleRot, Time.deltaTime * smoothTurnSpeed ), 0.0f);
      movingAxis = joystick.InputDirection.magnitude;
    }
    else
    {
      charTrPublic.rotation = Quaternion.Euler(0.0f, Mathf.LerpAngle(charTrPublic.rotation.eulerAngles.y, camAngle + angleRot, Time.deltaTime * smoothTurnSpeed ), 0.0f);
      movingAxis = Input.GetAxis("Vertical");
    }
  }

  void SetDefaultCharCollider()
  {
    charTrPublic.localPosition = Vector3.zero;
    charController.height = 2.0f;
    charController.radius = 0.5f;
  }

  void SetCarryingCharCollider()
  {
    charController.radius = 2.0f;
  }

  Vector3 coinMoveDirection;
  float coinTakeDistance = 1.0f;
  float coinTakeSpeed = 10.0f;
  float coinTakeSpeedAcc = 2.0f;
  void TakeCoinAnim()
  {
    bool isAllTaken = true;
    for (int i = 0; i < takingCoinsList.Count; i++)
    {
      if (takingCoinsList[i] == null)
        continue;
      if (takingCoinsList[i].activeInHierarchy)
      {
        isAllTaken = false;
        coinMoveDirection = (charController.transform.position - takingCoinsList[i].transform.position).normalized;
        if (Vector3.Distance(charController.transform.position, takingCoinsList[i].transform.position) < coinTakeDistance * takingCoinsSpeed[i] * Time.deltaTime)
        {
          if (GameSystem.currentLevel != 1)
            GameSystem.collectedCoinsOnLevel++;
          else
            GameSystem.totalCoins++;
          EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
          PlayPartSystem(takeSmthPartSystem, takingCoinsList[i].transform.position);
          Destroy(takingCoinsList[i]); //takingCoinsList[i].SetActive(false);
        }
        else
        {
          takingCoinsList[i].transform.position += coinMoveDirection * takingCoinsSpeed[i] * Time.deltaTime;
          if (takingCoinsList[i].transform.localScale.x > 0.5f)
          {
            takingCoinsList[i].transform.localScale -= Vector3.one * 0.1f;
          }
          takingCoinsSpeed[i] += coinTakeSpeedAcc;
        }
        //
      }
    }
    if (isAllTaken)
    {
      takingCoinsList.Clear();
      takingCoinsSpeed.Clear();
    }
  }

  Vector3 objectMoveDirection;
  float objectTakeDistance = 1.0f;
  float objectTakeSpeed = 10.0f;
  float objectTakeSpeedAcc = 2.0f;
  void TakeObjectAnim()
  {
    bool isAllTaken = true;
    for (int i = 0; i < takinObjectsList.Count; i++)
    {
      if (takinObjectsList[i] == null)
        continue;
      if (takinObjectsList[i].activeInHierarchy)
      {
        isAllTaken = false;
        objectMoveDirection = (charController.transform.position - takinObjectsList[i].transform.position).normalized;
        if (Vector3.Distance(charController.transform.position, takinObjectsList[i].transform.position) < objectTakeDistance * takinObjectsSpeed[i] * Time.deltaTime)
        {
          if (takinObjectsList[i].tag == "Key")
          {
            if (takinObjectsList[i].name.Contains("aether"))
              GameSystem.collectedKeyIndex = Keys.AETHER;
            else if (takinObjectsList[i].name.Contains("water"))
              GameSystem.collectedKeyIndex = Keys.WATER;
            else if (takinObjectsList[i].name.Contains("fire"))
              GameSystem.collectedKeyIndex = Keys.FIRE;
            else if (takinObjectsList[i].name.Contains("ground"))
              GameSystem.collectedKeyIndex = Keys.GROUND;
            else if (takinObjectsList[i].name.Contains("air"))
              GameSystem.collectedKeyIndex = Keys.AIR;

            GameSystem.playerKeys++;
            EventsManager.TriggerEvent(EventsIds.CHANGE_KEYS_COUNT);
          }
          else
          if (takinObjectsList[i].tag == "Live")
          {
            GameSystem.playerLives++;
            EventsManager.TriggerEvent(EventsIds.INCREASE_LIVES);
          }

          PlayPartSystem(takeSmthPartSystem, takinObjectsList[i].transform.position);

          Destroy(takinObjectsList[i]);
        }
        else
        {
          takinObjectsList[i].transform.position += objectMoveDirection * takinObjectsSpeed[i] * Time.deltaTime;
          if (takinObjectsList[i].transform.localScale.x > 0.5f)
          {
            takinObjectsList[i].transform.localScale -= Vector3.one * 0.1f;
          }
          takinObjectsSpeed[i] += objectTakeSpeedAcc;
        }
        //
      }
    }
    if (isAllTaken)
    {
      takinObjectsList.Clear();
      takinObjectsSpeed.Clear();
    }

  }


  float hitImageEffectTime = 1.0f;

  IEnumerator hitImageEffect()
  {
    float timer = 0.0f;
    hitEffectImage.color = hitEffectCol;
    hitEffectImage.gameObject.SetActive(true);
    while (timer < hitImageEffectTime)
    {
      timer += Time.deltaTime;
      hitEffectImage.color -= deltaHitEffectCol;
      yield return null;
    }
    hitEffectImage.gameObject.SetActive(false);
  }

  void CheckMovingObjects()
  {

  }

  bool IsFalling()
  {
    Physics.CheckSphere(transform.position - Vector3.up * 0.5f, 1.2f);
    return true;
    //Debug.DrawLine(transform.position - Vector3.up  , transform.position - Vector3.up * 2, Color.blue);
    /*if (Physics.Raycast(transform.position - Vector3.up , -Vector3.up, 2.0f))
    {
      return false;
    }
    else
    {
      return true;
    }*/
  }

  void Move_Update()
  {
    if (!charController.isGrounded)
    {
      fallJumpTimer = 0.0f;
      canSmoothJump = true;
      ChangeState( PlayerStates.FALLING );
      StopRunParticles();
      ThirdPersonOrbitCam.instance.SetFallState();
      canDoubleJump = true;
      hermitSounds.StopSound();
      return;
    }

    if (CheckForSliding())
    {
      fallJumpTimer = 0.0f;
      canSmoothJump = true;
      StopRunParticles();
      ChangeState( PlayerStates.SLIDING );
      return;
    }

    if (input.Info.jumpInput)
    {
      if (slideAngle < customSlopeLimit)
      {
        if (SimpleJump())
        {
          ChangeState( PlayerStates.JUMPING );
          PlayPartSystem(jump_jump2_anim, charTrPublic.position);
          StopRunParticles();
          hermitSounds.JumpSound();
          jumpStartPoint = transform.position;
          //SoundManager.instance.PlayJumpSound();
          input.Info.jumpInput = false;
          return;//Тут не было ретерн, не знаю почему
        }
      }
    }

    if (input.Info.rolloverInput)
    {
      //RollOver();
      Fly_Wings();
      return;
    }

    if (isKnockback)
    {
      /*StopRunParticles();
      charController.Move((knockbackVector * knockbackSpeed + verticalVelocity) * Time.deltaTime);
      return;*/
    }

    if (input.Info.hitInput)
    {
      StaffHit();
    }


    InputControl_Movement();

    if (!StaffController.isStaffInHitt)
    {
      if (movingAxis > 0.0f && movingAxis < 0.5f)
      {
        StopRunParticles();
        SetPlayerAnimState(playerAnimStates.WALK, 1.0f);
        if (!isWalkOnDirt)
          hermitSounds.WalkSound();
        else
          hermitSounds.DirtWalk();
      }
      else if (movingAxis > 0.5f)
      {
        StartRunParticles();
        if (!isWalkOnDirt)
          hermitSounds.RuningSound();
        else
          hermitSounds.DirtRun();
        SetPlayerAnimState(playerAnimStates.RUN, 1.0f);
      }
      else
      {
        StopRunParticles();
        SetPlayerAnimState(playerAnimStates.IDLE, 1.0f);
        hermitSounds.StopSound();
      }
    }

    canJump = true;
    ReturnMoveSpeedToDefault();

    RotatePlayer(GameSystem.withJoystick);
    Vector3 moveVector = Vector3.ProjectOnPlane(charTrPublic.forward, hitNormal);  //Проекация на полоскость прикосновения
    if (moveVector.y > 0)
      moveVector.y = 0;
    verticalVelocity.y = -1.0f;
    motion = moveVector * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
  }

  void Jump_Update()
  {
    if (verticalVelocity.y < 0)
    {
      ChangeState( PlayerStates.FALLING );
      ThirdPersonOrbitCam.instance.SetFallState();
      return;
    }

    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        ChangeState( PlayerStates.DOUBLE_JUMP );
        PlayPartSystem(jump_jump2_anim, transform.position);
        hermitSounds.DoubleJumpSound();
      }
    }

    SetPlayerAnimState(playerAnimStates.JUMP_FIRST, 1.0f);


    if (isKnockback)
    {
     /* charController.Move((knockbackVector * knockbackSpeed + verticalVelocity) * Time.deltaTime);
      verticalVelocity.y -= gravityForce * Time.deltaTime;
      return;*/
    }

    if (input.Info.rolloverInput)
    {
      //RollOver();
      Fly_Wings();
      return;
    }

    if (input.Info.hitInput)
    {
      StaffHit();
    }

    InputControl_Movement();

    RotatePlayer(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    motion = moveDirection * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
    verticalVelocity.y -= gravityForce * Time.deltaTime;
  }

  void DoubleJump_Update()
  {
    if (verticalVelocity.y < 0)
    {
      ChangeState( PlayerStates.FALLING );
      ThirdPersonOrbitCam.instance.SetFallState();
      return;
    }

    SetPlayerAnimState(playerAnimStates.JUMP_SECOND, 1.0f);

    if (isKnockback)
    {
      /*charController.Move((knockbackVector * knockbackSpeed + verticalVelocity) * Time.deltaTime);
      verticalVelocity.y -= gravityForce * Time.deltaTime;
      return;*/
    }

    if (input.Info.rolloverInput)
    {
      //RollOver();
      Fly_Wings();
      return;
    }

    if (input.Info.hitInput)
    {
      StaffHit();
    }

    InputControl_Movement();
    RotatePlayer(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    motion = moveDirection * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
    verticalVelocity.y -= gravityForce * Time.deltaTime;
  }


  float actualFallTimer = 0.0f;
  float actulaFallTime = 0.5f;
  //Флаг, говорящий о том что можем прыгнуть в течении некоторого времени после начала падения ( для удобства  косоруких )
  bool canSmoothJump = true;
  void Fall_Update()
  {
    if (charController.isGrounded)
    {
      //SetPlayerAnimState(PlayerAnimStates.GROUND, 1.0f);
      SetPlayerAnimState(playerAnimStates.WALK, 1.0f);
      ChangeState( PlayerStates.MOVING );
      ThirdPersonOrbitCam.instance.SetDefaultState();
      hermitSounds.GroundedSound();

      if (actualFallTimer > actulaFallTime)
      {
        PlayPartSystem(jump_jump2_anim, transform.position - Vector3.up);
      }
      actualFallTimer = 0.0f;
      return;
    }

    if (checkHitch())
    {
      playerState = PlayerStates.HITCH_ON_CLIMB;
      return;
    }

    SetPlayerAnimState(playerAnimStates.FALL, 1.0f);

    if ((fallJumpTimer < fallJumpTime) && (canSmoothJump != false))
    {
      //Debug.Log("smooth jump");
      fallJumpTimer += Time.deltaTime;
      canSmoothJump = true;
    }
    else
    {
      canSmoothJump = false;
    }

    actualFallTimer += Time.deltaTime;

    canJump = false;
    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        if (canSmoothJump)
        {
          playerState = PlayerStates.JUMPING;
          PlayPartSystem(jump_jump2_anim, charTrPublic.position);
          canSmoothJump = false;
          canDoubleJump = true;
          hermitSounds.JumpSound();
          return;
        }
        else
        {
          playerState = PlayerStates.DOUBLE_JUMP;
          PlayPartSystem(jump_jump2_anim, transform.position);
          hermitSounds.DoubleJumpSound();
          return;
        }
      }
    }


    if (isKnockback)
    {
      /*charController.Move((knockbackVector * knockbackSpeed + verticalVelocity) * Time.deltaTime);
      verticalVelocity.y -= gravityForce * Time.deltaTime;
      return;*/
    }

    if (input.Info.rolloverInput)
    {
      //RollOver();
      Fly_Wings();
      return;
    }

    if (input.Info.hitInput)
    {
      StaffHit();
    }

    InputControl_Movement();
    ReturnMoveSpeedToDefault();

    RotatePlayer(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    motion = moveDirection * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
    verticalVelocity.y -= gravityForce * Time.deltaTime;
  }

  float slideSpeed = 1.0f;
  void Sliding_Update()
  {
    //Debug.Log("sliding");

    if (fallJumpTimer < fallJumpTime)
    {
      fallJumpTimer += Time.deltaTime;
      canJump = true;
      //Debug.Log("CansmoothJumop");
    }
    else
    {
      canSmoothJump = false;
    }

    //if( )
    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        if (canSmoothJump)
        {
          playerState = PlayerStates.JUMPING;
          PlayPartSystem(jump_jump2_anim, transform.position);
          hermitSounds.JumpSound();
          canSmoothJump = false;
          return;
        }
      }
    }

    if (!Physics.CheckSphere(transform.position - Vector3.up * 0.5f, 0.55f, layer))
    {
      canDoubleJump = true;
      playerState = PlayerStates.FALLING;
      ThirdPersonOrbitCam.instance.SetFallState();
      slideSpeed = 1.0f;
      return;
    }

    Vector3 moveVector = -Vector3.ProjectOnPlane(Vector3.up, hitNormal);

    if (moveVector == Vector3.zero)
    {
      playerState = PlayerStates.FALLING;
      ThirdPersonOrbitCam.instance.SetFallState();
      return;
    }

    slideSpeed += slideAcceleration;
    charController.Move(moveVector * slideSpeed * Time.deltaTime);

    if (charController.velocity.y > -0.5f)
    {
      playerState = PlayerStates.MOVING;
      return;
    }

  }

  void Climb_Update()
  {
    if (!isClimbing)
    {
      playerState = PlayerStates.FALLING;
      ThirdPersonOrbitCam.instance.SetFallState();
      return;
    }

    if (charController.isGrounded)
    {
      if (GameSystem.withJoystick)
      {
        if (joystick.InputDirection.y < 0)
        {
          playerState = PlayerStates.MOVING;
          return;
        }
      }
      else
      {
        if (keyboardInputAxis.y < 0)
        {
          playerState = PlayerStates.MOVING;
          return;
        }
      }
    }

    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        playerState = PlayerStates.JUMPING;
        PlayPartSystem(jump_jump2_anim, transform.position);
        hermitSounds.JumpSound();
        return;
      }
    }
    if (GameSystem.withJoystick)
      SetPlayerAnimState(playerAnimStates.CLIMB, Mathf.Clamp(joystick.InputDirection.magnitude, 0.0f, 1.0f));
    else
      SetPlayerAnimState(playerAnimStates.CLIMB, Mathf.Clamp(keyboardInputAxis.magnitude, 0.0f, 1.0f));

    if (joystick.InputDirection.magnitude > 0.1f)
    {
      HermitSoundManager.instance.Climb();
    }
    else
    {
      HermitSoundManager.instance.StopSound();
    }


    InputControl_Movement();
    if (GameSystem.withJoystick)
    {
      moveDirection = charTrPublic.up * joystick.InputDirection.y + charTrPublic.right * joystick.InputDirection.x + verticalVelocity;
    }
    else
    {
      moveDirection = charTrPublic.up * keyboardInputAxis.y + charTrPublic.right * keyboardInputAxis.x + verticalVelocity;
    }
    charController.Move((moveDirection * climbSpeed) * Time.deltaTime);
    canJump = true;
  }

  float deltaWalkSpeed = 40.0f;
  void ReturnMoveSpeedToDefault()
  {
    //Если скорость больше
    if (walkSpeed > defaultWalkSpeed)
    {
      // Debug.Log("Уменьш");
      if (defaultWalkSpeed > walkSpeed - deltaWalkSpeed * Time.deltaTime)
      {
        walkSpeed = defaultWalkSpeed;

      }
      else
      {
        walkSpeed -= deltaWalkSpeed * Time.deltaTime;
      }
      return;
    }


    //Если сокрость меньше
    if (walkSpeed < defaultWalkSpeed)
    {
      if (defaultWalkSpeed < walkSpeed + deltaWalkSpeed * Time.deltaTime)
      {
        walkSpeed = defaultWalkSpeed;
      }
      else
      {
        walkSpeed += deltaWalkSpeed * Time.deltaTime;
      }
      return;
    }
  }

  void Swim_Update()
  {
    if (!isSwimming)
    {
      playerState = PlayerStates.FALLING;
      return;
    }

    InputControl_Movement();
    if (GameSystem.withJoystick)
      SetPlayerAnimState(playerAnimStates.SWIM, Mathf.Clamp(joystick.InputDirection.magnitude, 0.3f, 1.0f));
    else
      SetPlayerAnimState(playerAnimStates.SWIM, Mathf.Clamp(keyboardInputAxis.magnitude, 0.3f, 1.0f));
    //Если вертикальная скорость > 0 и находимся выше поверхности
    if (verticalVelocity.y > 0.0f && transform.position.y + swimOnSurfaceOffset >= swimSurfaceY)
    {
      // swimParticles = false;
      playerState = PlayerStates.JUMPING;
      PlayPartSystem(jump_jump2_anim, transform.position);
      SetPlayerAnimState(playerAnimStates.JUMP_FIRST, 1.0f);
      isSwimming = false;
      verticalVelocity.y = jumpHeight;
      return;
    }

    //Ныряем
    if (verticalVelocity.y < 0.0f)
    {
      isSwimOnSurface = false;
      if (verticalVelocity.y + Time.deltaTime < 0)
      {
        verticalVelocity.y += Time.deltaTime;
      }
      else
      {
        verticalVelocity.y = 0.0f;
      }
    }

    //Всплываем
    if (verticalVelocity.y > 0.0f && !isSwimOnSurface)
    {
      if (verticalVelocity.y - Time.deltaTime > 0.0f)
      {
        if ((transform.position.y + verticalVelocity.y - Time.deltaTime) > swimSurfaceY)
        {
          isSwimOnSurface = true;
          transform.position = new Vector3(transform.position.x, swimSurfaceY, transform.position.z);
        }
        else
        {
          verticalVelocity.y -= Time.deltaTime;
        }
      }
      else
      {
        verticalVelocity.y = 0.0f;
      }
    }

    if (input.Info.jumpInput)
      verticalVelocity.y = jumpHeight * 0.1f;

    if (input.Info.hitInput)
      verticalVelocity.y = -2.0f;

    if (movingAxis < 0.5f)
    {
      //swimParticles.SetActive(false);
    }
    else
    {
      //swimParticles.SetActive(true);
    }

    HermitSoundManager.instance.Swim();
    //Debug.Log(movingAxis);
    swimParticles.transform.position = transform.position;
    RotatePlayer(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    motion = moveDirection * walkSpeed * 0.5f * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
  }

  Vector3 checkVector = new Vector3(2.5f, 2.5f, 2.5f) * 0.5f;
  void PickUp_Update()
  {
    if (!charController.isGrounded)
    {
      pickableObject.DropDown(charController.velocity);
      pickableObject = null;
      playerState = PlayerStates.FALLING;
      ThirdPersonOrbitCam.instance.SetFallState();
      canDoubleJump = true;
      hermitSounds.StopSound();
      return;
    }

    if (CheckForSliding())
    {
      pickableObject.DropDown(charController.velocity);
      pickableObject = null;
      playerState = PlayerStates.SLIDING;
      return;
    }


    InputControl_Movement();

    if (movingAxis > 0.0f)
    {
      SetPlayerAnimState(playerAnimStates.WALK, 1.0f);
      hermitSounds.WalkSound();
    }
    else
    {
      hermitSounds.StopSound();
    }
    SetPlayerAnimState(playerAnimStates.PICK_UP, JoystickController.instance.InputDirection.magnitude);

    canJump = false;
    if (Input.GetKeyDown(KeyCode.L))
      Debug.Log("camAngle " + camAngle);
    if (JoystickController.instance.InputDirection.magnitude > 0)
      pickUpVector = Quaternion.Euler(0.0f, camAngle + angleRot, 0.0f) * Vector3.forward;
    else
      pickUpVector = charTrPublic.forward;


    if (Physics.CheckBox(transform.position + Vector3.up + pickUpVector * 2, checkVector, Quaternion.LookRotation(pickUpVector), layer, QueryTriggerInteraction.Ignore))
    {
      return;
    }

    if (input.Info.interactInput)
    {
      pickableObject.DropDown(charController.velocity);
      pickableObject = null;
      input.Info.interactInput = false;
      playerState = PlayerStates.MOVING;
      return;
    }

    pickableObject.transform.position = transform.position + Vector3.up + pickUpVector * 2;
    pickableObject.transform.rotation = Quaternion.LookRotation(pickUpVector);

    RotatePlayer(GameSystem.withJoystick);
    Vector3 moveVector = Vector3.ProjectOnPlane(charTrPublic.forward, hitNormal);  //Проекация на полоскость прикосновения
    if (moveVector.y > 0)
      moveVector.y = 0;
    verticalVelocity.y = -1.0f;
    movingAxis = Mathf.Clamp(movingAxis, 0.0f, 0.5f);
    motion = moveVector * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
  }

  void Dialog_Update()
  {
    //Если приняли квест после цепочки диалогов, ты выходим
    if (quest.status == QuestStatus.Accepted)
    {
      playerState = PlayerStates.MOVING;
      GameSystem.playerCanBeHitted = true;
      cameraScript.SetDefaultCameraState();
      return;
    }

    if (quest.status == QuestStatus.Completed)
    {
      playerState = PlayerStates.MOVING;
      GameSystem.playerCanBeHitted = true;
      cameraScript.SetDefaultCameraState();
      return;
    }

    //Скипаем диалог, принимаем квест
    if (input.Info.hitInput)
    {
      if (quest.status == QuestStatus.Acceptable)
      {
        quest.AcсeptQuest();
        playerState = PlayerStates.MOVING;
        cameraScript.SetDefaultCameraState();
        GameSystem.playerCanBeHitted = true;
        return;
      }
      else if (quest.status == QuestStatus.Done)
      {
        quest.CompleteQuest();
        playerState = PlayerStates.MOVING;
        cameraScript.SetDefaultCameraState();
        GameSystem.playerCanBeHitted = true;
      }
    }

    SetPlayerAnimState(playerAnimStates.IDLE, 1.0f);
    GameSystem.playerCanBeHitted = false;
  }

  void Hitch_Update()
  {
    if (!checkHitch())
    {
      playerState = PlayerStates.FALLING;
      return;
    }
    canJump = true;

    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        playerState = PlayerStates.JUMPING;
        PlayPartSystem(jump_jump2_anim, transform.position);
        hermitSounds.JumpSound();
        return;
      }
    }

    if (isKnockback)
    {
      /*charController.Move(-charTrPublic.forward * knockbackSpeed * Time.deltaTime);
      playerState = PlayerStates.FALLING;
      return;*/
    }

    SetPlayerAnimState(playerAnimStates.HITCH, 1.0f);
  }

  void Wardrobe_Update()
  {
    if (!isInWardrobe)
    {
      playerState = PlayerStates.MOVING;
      return;
    }

    SetPlayerAnimState(playerAnimStates.IDLE, 1.0f);
  }

  void Hint_Update()
  {
    if (hint == null)
    {
      playerState = PlayerStates.MOVING;
      GameSystem.playerCanBeHitted = true;
      return;
    }

    if (hint.isClosed)
    {
      playerState = PlayerStates.MOVING;
      GameSystem.playerCanBeHitted = true;
      return;
    }

    SetPlayerAnimState(playerAnimStates.IDLE, 1.0f);
    GameSystem.playerCanBeHitted = false;
  }


  float rollOverTime = 0.25f;
  float rollOverTimer = 0.0f;
  float rollOverCd = 1.5f;
  float rollOverCurrentCd = 0.0f;
  float rollOverspeedDefault = 35.0f;
  float rollOverspeed = 35.0f;
  float rollOverAcc = 108.0f;
  void RollOver_Update()
  {
    if (rollOverTimer < rollOverTime)
    {
      rollOverTimer += Time.deltaTime;
    }
    else
    {
      walkSpeed = defaultWalkSpeed;//25.0f;
      charController.height = 2.0f;
      playerState = PlayerStates.FALLING;
      return;
    }

    /* InputControl_Movement();

     if (!StaffController.isStaffInHitt)
     {
       if (movingAxis > 0.0f && movingAxis < 0.5f)
       {
         SetPlayerAnimState(PlayerAnimStates.WALK, 1.0f);
         hermitSounds.WalkSound();
       }
       else if (movingAxis > 0.5f)
       {
         hermitSounds.RuningSound();
         SetPlayerAnimState(PlayerAnimStates.RUN, 1.0f);
       }
       else
       {
         SetPlayerAnimState(PlayerAnimStates.IDLE, 1.0f);
         hermitSounds.StopSound();
       }
     }*/

    //canJump = true;

    InputControl_Movement();

    RotatePlayer(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    rollOverspeed -= rollOverAcc * Time.deltaTime;
    motion = moveDirection * rollOverspeed + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
    //verticalVelocity.y -= gravityForce * Time.deltaTime;
  }

  void RollOver()
  {
    if (playerState == PlayerStates.PICK_UP)
      return;

    if ( rollOverCurrentCd < rollOverCd )
      return;
    SetPlayerAnimState(playerAnimStates.ROLL_UP, 3.0f);
    rollOverTimer = 0.0f;
    rollOverCurrentCd = 0.0f;
    verticalVelocity.y = 0.0f;
    rollOverspeed = rollOverspeedDefault;
    playerState = PlayerStates.ROLL_OVER;
  }

  void HomyThrow_Update()
  {
    if (input.Info.homyAimInput)
    {
      playerState = PlayerStates.MOVING;
      charTrPublic.gameObject.SetActive(true);
      ThirdPersonOrbitCam.instance.SetDefaultCameraState();
      return;
    }
  }

  //Сопротивление воздуха при переключении из падения в  планирование
  float airResistanse = 5.0f;
  void Fly_Wings_Update()
  {
    if (charController.isGrounded)
    {
      //SetPlayerAnimState(PlayerAnimStates.GROUND, 1.0f);
      SetPlayerAnimState(playerAnimStates.WALK, 1.0f);
      ChangeState(PlayerStates.MOVING);
      ThirdPersonOrbitCam.instance.SetDefaultState();
      hermitSounds.GroundedSound();

      if (actualFallTimer > actulaFallTime)
      {
        PlayPartSystem(jump_jump2_anim, transform.position - Vector3.up);
      }
      actualFallTimer = 0.0f;
      return;
    }

    if (checkHitch())
    {
      playerState = PlayerStates.HITCH_ON_CLIMB;
      return;
    }

    SetPlayerAnimState(playerAnimStates.FALL, 1.0f);

    if ((fallJumpTimer < fallJumpTime) && (canSmoothJump != false))
    {
      //Debug.Log("smooth jump");
      fallJumpTimer += Time.deltaTime;
      canSmoothJump = true;
    }
    else
    {
      canSmoothJump = false;
    }

    actualFallTimer += Time.deltaTime;

    if( input.Info.jumpInput )
    {
      if (SimpleJump())
      {
        if (canSmoothJump)
        {
          playerState = PlayerStates.JUMPING;
          PlayPartSystem(jump_jump2_anim, charTrPublic.position);
          canSmoothJump = false;
          canDoubleJump = true;
          hermitSounds.JumpSound();
          return;
        }
        else
        {
          playerState = PlayerStates.DOUBLE_JUMP;
          PlayPartSystem(jump_jump2_anim, transform.position);
          hermitSounds.DoubleJumpSound();
          return;
        }
      }

      playerState = PlayerStates.FALLING;
      return;
    }

    if( input.Info.hitInput )
    {
      playerState = PlayerStates.HITTING;
      return;
    }

    /*canJump = false;
    if (input.Info.jumpInput)
    {
      if (SimpleJump())
      {
        if (canSmoothJump)
        {
          playerState = PlayerStates.JUMPING;
          PlayPartSystem(jump_jump2_anim, charTrPublic.position);
          canSmoothJump = false;
          canDoubleJump = true;
          hermitSounds.JumpSound();
          return;
        }
        else
        {
          playerState = PlayerStates.DOUBLE_JUMP;
          PlayPartSystem(jump_jump2_anim, transform.position);
          hermitSounds.DoubleJumpSound();
          return;
        }
      }
    }*/

    if (isKnockback)
    {
      /*charController.Move((knockbackVector * knockbackSpeed + verticalVelocity) * Time.deltaTime);
      verticalVelocity.y -= gravityForce * Time.deltaTime;
      return;*/
    }

    if (input.Info.rolloverInput)
    {
      //RollOver();
      Fly_Wings();
      return;
    }

    if (input.Info.hitInput)
    {
      StaffHit();
    }

    InputControl_Movement();
    ReturnMoveSpeedToDefault();

    RotatePlayerSmooth(GameSystem.withJoystick);
    moveDirection = charTrPublic.forward;
    Debug.Log(verticalVelocity.y);
    verticalVelocity.y = Mathf.Lerp(verticalVelocity.y , -3.0f, Time.deltaTime * airResistanse );
    
    motion = moveDirection * walkSpeed * movingAxis + verticalVelocity;
    charController.Move(motion * Time.deltaTime);
    //verticalVelocity.y -= gravityForce * Time.deltaTime;
  }


  //Просто останавливаем игрока
  void Stop_Update()
  {

  }

  void InputControl_Movement()
  {
    if (GameSystem.withJoystick)
    {
      cameraTransformForwardXZ.x = cameraTransform.forward.x;
      cameraTransformForwardXZ.y = cameraTransform.forward.z;
      camAngle = Vector2.Angle(cameraTransformForwardXZ, Vector2.up);
      if (cameraTransform.forward.x < 0)
        camAngle *= -1;
      angleRot = joystick.InputDirection.x < 0 ? -Vector3.Angle(Vector3.up, joystick.InputDirection) :
      Vector3.Angle(Vector3.up, joystick.InputDirection);
    }
    else
    {
      if (Input.GetMouseButton(1))
      {
        cameraTransformForwardXZ.x = cameraTransform.forward.x;
        cameraTransformForwardXZ.y = cameraTransform.forward.z;
        angleRot = Vector2.Angle(cameraTransformForwardXZ, Vector2.up);
        if (cameraTransform.forward.x < 0)
          angleRot *= -1;
      }
      else
      {
        angleRot += Input.GetAxis("Horizontal") * keyboardTurnSpeed;
      }

      keyboardInputAxis.x = Input.GetAxis("Horizontal");
      keyboardInputAxis.y = Input.GetAxis("Vertical");
    }
  }

  //Можем ли прыгнуть
  bool canJump = true;
  //Можем ли прыгнуть второй  раз
  bool canDoubleJump = false;

  //Возращает true, если можем совершить простой прыжок
  bool SimpleJump()
  {
    if (canJump)
    {
      ThirdPersonOrbitCam.instance.SetJumpState();
      verticalVelocity.y = jumpHeight;
      canJump = false;
      canDoubleJump = true;
      return true;
    }

    if (canDoubleJump)
    {
      ThirdPersonOrbitCam.instance.SetJumpState();
      verticalVelocity.y = jumpHeight;
      canJump = false;
      canDoubleJump = false;
      return true;
    }
    return false;
  }

  void StartQuestDialog()
  {
    playerState = PlayerStates.DIALOG;
    hermitSounds.StopSound();
    transform.position = quest.transform.position + quest.transform.forward * 2 - Vector3.up * 0.5f;
    charTrPublic.rotation = quest.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
    quest.ShowQuestDialog();
    cameraScript.SetDialogCameraState(quest.transform);
  }

  void CompleteQuestDialog()
  {
    playerState = PlayerStates.DIALOG;
    transform.position = quest.transform.position + quest.transform.forward * 2;
    quest.ShowCompleteQuestDialog();
  }

  public void EnterStopState()
  {
    SetPlayerAnimState(playerAnimStates.IDLE, 1.0f);
    playerState = PlayerStates.STOP;
  }

  public void ExitStopState()
  {
    playerState = PlayerStates.MOVING;
  }

  public void EnterHintState(Hint hint_)
  {
    playerState = PlayerStates.HINT;
    hint = hint_;
  }

  public void PickUp(PickableObject pickableObject_)
  {
    pickableObject = pickableObject_;
    playerState = PlayerStates.PICK_UP;
  }

  void StaffHitAnim()
  {

  }

  void PlayPartSystem(ParticleSystem partSys, Vector3 pos)
  {
    partSys.transform.position = pos;
    partSys.Play();
  }


  ParticleSystem.MainModule runMainModule;

  void StopRunParticles()
  {
    runMainModule.loop = false;
    //runPartSystem.gameObject.SetActive(false);
  }

  void StartRunParticles()
  {
    runPartSystem.gameObject.SetActive(true);
    runMainModule.loop = true;
    if (!runPartSystem.isPlaying)
      runPartSystem.Play();
  }

  void DrawHitchSphere()
  {
    //Physics.Raycast(charController.transform.position + hitchRayCastOffset, charTrPublic.forward, out climbHit, 1.0f))

    //Gizmos.DrawSphere( )
  }

  bool CheckObjectForInterraction(Transform tr)
  {
    if (playerState == PlayerStates.HINT)
    {
      GameUIController.instance.HideInterractIcon();
      return false;
    }
    else
    {
      if (Vector3.Distance(tr.position, transform.position) < 5.0f)
      {
        GameUIController.instance.ShowInterractIcon();
        return true;
      }
      else
      {
        GameUIController.instance.HideInterractIcon();
        return false;
      }
    }
  }

  void CheckQuest()
  {
    if (input.Info.interactInput)
    {
      if (quest.status == QuestStatus.Completed)
      {

      }
      else if (quest.status == QuestStatus.Done)
      {
        CompleteQuestDialog();
      }
      else
      {
        StartQuestDialog();
      }
      input.Info.interactInput = false;
    }
  }

  void CheckHint()
  {
    if (input.Info.interactInput)
    {
      hint.ShowHint();
      playerState = PlayerStates.HINT;
      input.Info.interactInput = false;
    }
  }

  void ChangeState(PlayerStates newState)
  {
    prevState = playerState;
    playerState = newState;
  }

  public void SetDefaulPlayertState()
  {
    ChangeState(PlayerStates.MOVING);
  }
  
  //В этой стойке не выполняется никакой код
  public void SetInactivePlayerState()
  {
    ChangeState(PlayerStates.INACTIVE );
  }

  public void InitAnimParameters()
  {
    playerAnimStates.IDLE =  Animator.StringToHash( "ShouldIdle" );
    playerAnimStates.RUN = Animator.StringToHash("ShouldRun");
    playerAnimStates.WALK = Animator.StringToHash("ShouldWalk");
    playerAnimStates.GROUND = Animator.StringToHash("ShouldGround");
    playerAnimStates.JUMP_FIRST = Animator.StringToHash("ShouldJumpFirst");
    playerAnimStates.JUMP_SECOND = Animator.StringToHash("ShouldJumpSecond");
    playerAnimStates.FALL = Animator.StringToHash("ShouldFall");
    playerAnimStates.HIT = Animator.StringToHash("ShouldHit");
    playerAnimStates.CLIMB = Animator.StringToHash("ShouldClimb");
    playerAnimStates.SWIM = Animator.StringToHash("ShouldSwim");
    playerAnimStates.HIT2 = Animator.StringToHash("ShouldHit2");
    playerAnimStates.HIT3 = Animator.StringToHash("ShouldHit3");
    playerAnimStates.HIT4 = Animator.StringToHash("ShouldHitch");
    playerAnimStates.HITCH = Animator.StringToHash("ShouldPickUp");
    playerAnimStates.PICK_UP = Animator.StringToHash("ShouldPickUp");
    playerAnimStates.ROLL_UP = Animator.StringToHash("ShouldRollOver");
  }

  public Vector3 GetPlayerSpeed()
  {
    return charController.velocity;
  }

  //Возвращает 
  public Vector3 GetPlayerPosWithSpeed()
  {
    Debug.Log(charController.velocity * Time.deltaTime);
    return transform.position - Vector3.up + charController.velocity*Time.deltaTime * 10;
  }

  void Fly_Wings()
  {
    playerState = PlayerStates.FLY_WINGS;
    return;
  }
}
