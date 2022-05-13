using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonOrbitCam : MonoBehaviour
{
  public static ThirdPersonOrbitCam instance;
  public Transform player;                                           // Player's reference.
  public Transform playerRotationPoint;
  public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);       // Offset to repoint the camera.
  public Vector3 camOffset = new Vector3(0.0f, 0.7f, -3.0f);       // Offset to relocate the camera related to the player position.

  public LayerMask layer;
  public GameObject underwaterImage;

  public bool isCamInCorutine = false;
  public bool isTelekenesis = false;

  public float smooth = 10f;                                         // Speed of camera responsiveness.
  public float horizontalAimingSpeed = 400f;                         // Horizontal turn speed.
  public float verticalAimingSpeed = 400f;                           // Vertical turn speed.
  public float maxVerticalAngle = 30f;                               // Camera max clamp angle. 
  public float minVerticalAngle = -60f;                              // Camera min clamp angle.
  public float followCharacterminAngle;
  public float followCharacterSmoothness;                            // Насколько резко камера следует за поворотами персонажа
  public float defaultVertRotation;                                  //Стандартный наклон угла камеры
  float deltaReturnVert = 100.0f;

  private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
  private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
  private float playerCameraAngle;                                   // Угол между камерой и игроком
  private Transform cam;                                             // This transform.
  private Vector3 relCameraPos;                                      // Current camera position relative to the player.
  private float relCameraPosMag;                                     // Current camera distance to the player.
  private Vector3 smoothPivotOffset;                                 // Смещение точки относительно игрока, вокруг которой вращается камера
  private Vector3 smoothCamOffset;                                   // Отдаление камеры относительно камеры

  Color normalColor;
  Color underwaterColor;

  float defaultCamDistance;
  //private float 

  Quaternion aimRotation;
  Transform target;                                                 //Цель, за кем следит камера

  float camAngleOffset = 1.0f;                                             //Смещение камеры относительно того, куда смотрит игрок
  bool wasCamCollission;

  enum CameraStates
  {
    DEFAULT_CAMERA,
    FREE_CAMERA,
    DIALOG_CAMERA,
    HINT_CAMERA,
    JUMP_CAMERA,
    FALL_CAMERA,
    HOMY_AIM
  }

  CameraStates cameraState;
  void Awake()
  {
    instance = this;
    normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    // Reference to the camera transform.
    cam = transform;

    // Set camera default position.
    cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
    cam.rotation = Quaternion.identity;//Quaternion.Euler( defaultVertRotation, 0.0f, 0.0f);//Quaternion.identity;

    // Get camera position relative to the player, used for collision test.
    /*relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;*/
    defaultCamDistance = (transform.position - player.position).magnitude;
    // Set up references and default values.
    smoothPivotOffset = pivotOffset;
    smoothCamOffset = camOffset;
    playerCameraAngle = 0.0f;
  }


  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.JUMP_FROM_CLIMB, SetCamBehindPlayerSmooth);
    EventsManager.StartListening(EventsIds.JUMP_ON_CLIMB, SetCamBehindPlayerSmooth);
    EventsManager.StartListening(EventsIds.STOP_SMOOTH_CAM_BEHIND_PLAYER, StopCamBehindPlayer);
    EventsManager.StartListening(EventsIds.CAMERA_TO_PLAYER_BEHIND, SetCamBehindPlayerSmooth);
  }

  private void OnDisable()
  {

  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.JUMP_FROM_CLIMB, SetCamBehindPlayerSmooth);
    EventsManager.StopListening(EventsIds.JUMP_ON_CLIMB, SetCamBehindPlayerSmooth);
    EventsManager.StopListening(EventsIds.STOP_SMOOTH_CAM_BEHIND_PLAYER, StopCamBehindPlayer);
    EventsManager.StopListening(EventsIds.CAMERA_TO_PLAYER_BEHIND, SetCamBehindPlayerSmooth);
  }

  //Ручное управление камерой в текущий момент
  //bool isManualCameraControl = false;

  private void Start()
  {
    cameraState = CameraStates.DEFAULT_CAMERA;
    target = player;
  }

  void Update()
  {
    switch (cameraState)
    {
      case CameraStates.DEFAULT_CAMERA:
        {
          DefaultCamera_Update();
          DefaultBehavior_Update();
          break;
        }

      case CameraStates.FREE_CAMERA:
        {
          FreeCamera_Update();
          DefaultBehavior_Update();
          break;
        }

      case CameraStates.DIALOG_CAMERA:
        {
          DialogCamera_Update();
          break;
        }
      case CameraStates.HINT_CAMERA:
        {
          Hint_Update();
          break;
        }
      case CameraStates.JUMP_CAMERA:
        {
          //smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, smoothPivotOffset + Vector3.up*0.25f, 0.1f);
          //DefaultCamera_Update();
          Jump_Update();
          //DefaultBehavior_Update();
          break;
        }
      case CameraStates.FALL_CAMERA:
        {
          //DefaultCamera_Update();
          //smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, smoothPivotOffset - Vector3.up * 0.25f, 0.1f);
          Fall_Update();
          //DefaultBehavior_Update();
          break;
        }
      case CameraStates.HOMY_AIM:
        {
          HomyAim_Update();
          break;
        }
      
    }

    CheckCollision();
  }

  public void DefaultBehavior_Update()
  {
    angleH += (playerCameraAngle + CameraLook.deltaPos.x * horizontalAimingSpeed) * Time.deltaTime;
    //angleH = Mathf.Lerp(angleH, playerCameraAngle + CameraLook.deltaPos.x * horizontalAimingSpeed, 0.1f);
    if (angleH > 360.0f)
    {
      angleH -= 360;
    }
    else if (angleH < -360.0f)
    {
      angleH += 360.0f;
    }
    //Если давно не перемещали камеру вручную, устанавливаем вращение по Y в стандартное положение
    if ( CameraLook.shouldReturnToDefaultVert)
    {
      if (!GameSystem.isFreeCam)
      {
        StartCoroutine("smoothReturnToDefaultVert");
        CameraLook.shouldReturnToDefaultVert = false;
        CameraLook.hasStartedCameraReturn = true;
      }
    }
    else
    {
      //angleV += Mathf.Lerp(angleV, CameraLook.deltaPos.y * verticalAimingSpeed, 0.1f);
      angleV += CameraLook.deltaPos.y * verticalAimingSpeed * Time.deltaTime;
      angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
    }

    aimRotation = Quaternion.Euler(-angleV + defaultVertRotation, angleH, 0);

    cam.rotation = aimRotation;
  }

  public void LateUpdate()
  {
    //if( cameraState != CameraStates.DIALOG_CAMERA )
    switch (cameraState)
    {
      case CameraStates.DEFAULT_CAMERA:
        {
          //cam.position = Vector3.Lerp( cam.position,target.position + smoothPivotOffset + aimRotation * smoothCamOffset, 0.05f);

          cam.position = target.position + smoothPivotOffset + aimRotation * smoothCamOffset * camAngleOffset;
          break;
        }
      case CameraStates.FREE_CAMERA:
        {
          cam.position = target.position + smoothPivotOffset + aimRotation * smoothCamOffset * camAngleOffset;
          break;
        }
      case CameraStates.FALL_CAMERA:
        {
          cam.position = target.position + smoothPivotOffset + aimRotation * smoothCamOffset * camAngleOffset;
          break;
        }
      case CameraStates.JUMP_CAMERA:
        {
          cam.position = target.position + smoothPivotOffset + aimRotation * smoothCamOffset * camAngleOffset;
          break;
        }
    }
  }

  float tempAngle = 0.0f;
  void CheckCollision()
  {
    Vector3 defaultCamDir = -transform.forward;
    defaultCamDir = defaultCamDir * defaultCamDistance;
    Vector3 camRotPoint = player.position + pivotOffset + Vector3.up;


    Debug.DrawRay(camRotPoint, defaultCamDir);
    float magnitude = 0.0f;
    RaycastHit hit;

    if (Physics.Linecast(camRotPoint, camRotPoint + defaultCamDir, out hit, layer, QueryTriggerInteraction.Ignore))
    {
      if (!wasCamCollission)
      {
        tempAngle = angleV;
      }

      Vector3 hitVector = hit.point - camRotPoint;
      magnitude = hitVector.magnitude;
    }
    else
    {
      magnitude = defaultCamDistance;
    }

    smoothCamOffset = smoothCamOffset.normalized * magnitude;
  }

  bool shouldIncrease = false;
  float deltRetAngle = 100.0f;
  public IEnumerator smoothReturnToDefaultVert()
  {
    yield break;
    isCamInCorutine = true;
    while (angleV > 0.1f || angleV < -0.1f)
    {
      angleV = Mathf.Lerp(angleV, 0.0f, 0.1f);
      yield return null;
    }

    angleV = 0.0f;
    isCamInCorutine = false;
    yield break;
  }

  public void SetCamBehindPlayerSmooth()
  {
    StartCoroutine("smoothCamBehindPlayer");
  }

  float deltaAngle = 300.0f;
  IEnumerator smoothCamBehindPlayer()
  {
    isCamInCorutine = true;
    float playerAngleY = playerRotationPoint.rotation.eulerAngles.y;
    if (playerAngleY > 180)
      playerAngleY -= 360f;

    if (angleH > 180)
      angleH = -360 + angleH;

    if (angleH < -180)
      angleH = 360 + angleH;

    if (Mathf.Approximately(playerAngleY, angleH))
    {
      isCamInCorutine = false;
      yield break;
    }

    if (Mathf.Approximately(angleH, 0.0f))
    {
      angleH = 0.01f;
    }

    //В одном секторе углов
    if ((playerAngleY > 0 && angleH > 0) || (playerAngleY < 0 && angleH < 0))
    {
      if (angleH > playerAngleY)
      {
        while (angleH > playerAngleY)
        {
          if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }

          if (angleH - deltaAngle * Time.deltaTime < playerAngleY)
          {
            angleH = playerAngleY;
            break;
          }
          angleH -= deltaAngle * Time.deltaTime;
          yield return null;
        }
        //yield break;
      }
      else
      {
        while (angleH < playerAngleY)
        {
          if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }

          if (angleH + deltaAngle * Time.deltaTime > playerAngleY)
          {
            angleH = playerAngleY;
            break;
          }
          yield return null;
          angleH += deltaAngle * Time.deltaTime;
        }
      }
    }
    else
    {
      //Угол камеры отрицательный
      if (angleH < 0)
      {
        //Путь при обходе по часовой
        float path1 = 360 + angleH - playerAngleY;
        //Путь при обходе против часовой
        float path2 = -angleH + playerAngleY;
        //Идем против часовой 
        if (path1 < path2)
        {
          //Если путь меньше дельты, сразу переводим камеру в нужное положение
          if (path1 < deltaAngle * Time.deltaTime)
          {
            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }

          //Если значение очень близкое к - 180, сразу переводим угол в положительную область
          if (angleH - deltaAngle * Time.deltaTime < -180.0f)
          {
            angleH = 360 + angleH - deltaAngle * Time.deltaTime;
          }
          else
          {
            //Доводим до -180.0f
            while (angleH - deltaAngle * Time.deltaTime > -180.0f)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }

              angleH -= deltaAngle * Time.deltaTime;
              yield return null;
            }
            angleH = 180.0f;
          }

          if (angleH - deltaAngle * Time.deltaTime < playerAngleY)
          {
            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }
          else
          {
            while (angleH - deltaAngle * Time.deltaTime > playerAngleY)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }

              angleH -= deltaAngle * Time.deltaTime;
              yield return null;
            }
            angleH = playerAngleY;
            yield break;
          }
        }
        else
        {
          if (path2 < deltaAngle * Time.deltaTime)
          {
            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }
          else
          {
            if (angleH + deltaAngle * Time.deltaTime > 0)
            {
              angleH = 0.0f;
            }
            else
            {
              while (angleH + deltaAngle * Time.deltaTime < 0)
              {
                if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
                angleH += deltaAngle * Time.deltaTime;
                yield return null;
              }
              angleH = 0.0f;
            }

            if (angleH + deltaAngle * Time.deltaTime > playerAngleY)
            {
              angleH = playerAngleY;
              isCamInCorutine = false;
            }
            else
            {
              while (angleH + deltaAngle * Time.deltaTime < playerAngleY)
              {
                if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
                angleH += deltaAngle * Time.deltaTime;
                yield return null;
              }
              angleH = playerAngleY;
              isCamInCorutine = false;
              yield break;
            }
          }
        }
      }
      //Угол камеры положительный
      else
      {
        //При обходе по часовой
        float path1_1 = 360 - angleH + playerAngleY;
        //Против часовой
        float path1_2 = angleH - playerAngleY;

        //По часовой
        if (path1_1 < path1_2)
        {
          if (path1_1 < deltaAngle * Time.deltaTime)
          {
            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }
          else
          {
            while (angleH + deltaAngle * Time.deltaTime < 180)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
              angleH += deltaAngle * Time.deltaTime;
              yield return null;
            }
            angleH = -180;

            if (angleH + deltaAngle * Time.deltaTime > playerAngleY)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
              angleH = playerAngleY;
              isCamInCorutine = false;
              yield break;
            }
            else
            {
              while (angleH + deltaAngle * Time.deltaTime < playerAngleY)
              {
                if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
                angleH += deltaAngle * Time.deltaTime;
                yield return null;
              }
              isCamInCorutine = false;
              angleH = playerAngleY;
              yield break;
            }
          }
        }
        //Против часовой
        else
        {
          if (path1_2 < deltaAngle * Time.deltaTime)
          {
            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }
          else
          {
            while (angleH - deltaAngle * Time.deltaTime > 0)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
              angleH -= deltaAngle * Time.deltaTime;
              yield return null;
            }

            angleH = 0.0f;

            if (angleH - deltaAngle * Time.deltaTime < playerAngleY)
            {
              angleH = playerAngleY;
              isCamInCorutine = false;
              yield break;
            }

            while (angleH - deltaAngle * Time.deltaTime > playerAngleY)
            {
              if (CameraLook.isDragging) { isCamInCorutine = false; yield break; }
              angleH -= deltaAngle * Time.deltaTime;
              yield return null;
            }

            angleH = playerAngleY;
            isCamInCorutine = false;
            yield break;
          }
        }
      }
    }
    isCamInCorutine = false;
    yield break;
  }

  void StopCamBehindPlayer()
  {
    StopCoroutine("smoothCamBehindPlayer");
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "SwimArea")
    {
      underwaterImage.SetActive(true);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "SwimArea")
    {
      underwaterImage.SetActive(false);
    }
  }


  void HorizontalPlayerAngle()
  {
    if (!CameraLook.isDragging && JoystickController.instance.isJoystickDragging && !CharacterControllerScript.isClimbing && !CharacterControllerScript.isJumpingFromClimb)
    {
      playerCameraAngle = Vector2.Angle(new Vector2(playerRotationPoint.forward.x, playerRotationPoint.forward.z), new Vector2(cam.forward.x, cam.forward.z));
      if (playerCameraAngle > 90.0f)
      {
        playerCameraAngle = 180.0f - playerCameraAngle;
      }

      if (playerCameraAngle < 170.0f)
      {
        if (JoystickController.instance.inputAngle < 0)
        {
          playerCameraAngle *= -1;
        }
      }
      else
      {
        playerCameraAngle = 0.0f;
      }
    }
    else
    {

      playerCameraAngle = 0.0f;
    }
  }

  void DefaultCamera_Update()
  {
    if (GameSystem.isFreeCam)
    {
      cameraState = CameraStates.DEFAULT_CAMERA;
      return;
    }

    target = player;

    HorizontalPlayerAngle();
    if ( /*false*/CameraLook.shouldReturnToDefaultVert)
    {
      CameraLook.shouldReturnToDefaultVert = false;
      CameraLook.hasStartedCameraReturn = true;
    }
    else
    {
      angleV += CameraLook.deltaPos.y * verticalAimingSpeed * Time.deltaTime;
      angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
    }

    if (JoystickController.instance.InputDirection.y < 0.0f)
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.5f, 0.05f);
    }
    else
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.0f, 0.05f);
    }


  }

  void FreeCamera_Update()
  {
    if (!GameSystem.isFreeCam)
    {
      cameraState = CameraStates.DEFAULT_CAMERA;
      return;
    }

    target = player;

    playerCameraAngle = 0.0f;
    angleV += CameraLook.deltaPos.y * verticalAimingSpeed * Time.deltaTime;
    angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
  }

  void DialogCamera_Update()
  {
    aimRotation = Quaternion.Euler(-angleV + defaultVertRotation, angleH, 0);
    cam.rotation = aimRotation;
    cam.position = target.position + smoothPivotOffset + aimRotation * smoothCamOffset;
  }

  float tmpJumpAngle = 0.0f;
  void Jump_Update()
  {
    if (CameraLook.isDragging)
    {
      SetDefaultCameraState();
    }

    HorizontalPlayerAngle();

    angleH += (playerCameraAngle + CameraLook.deltaPos.x * horizontalAimingSpeed) * Time.deltaTime;
    if (angleH > 360.0f)
    {
      angleH -= 360;
    }
    else if (angleH < -360.0f)
    {
      angleH += 360.0f;
    }

    tmpJumpAngle -= 40 * Time.deltaTime;
    if (tmpJumpAngle < 0)
    {
      tmpJumpAngle = 0.0f;
    }

    angleV += tmpJumpAngle * Time.deltaTime;
    angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

    aimRotation = Quaternion.Euler(-angleV + defaultVertRotation, angleH, 0);

    cam.rotation = aimRotation;

    if (JoystickController.instance.InputDirection.y < 0.0f)
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.5f, 0.05f);
    }
    else
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.0f, 0.05f);
    }
  }

  void Fall_Update()
  {
    if (CameraLook.isDragging)
    {
      SetDefaultCameraState();
    }

    HorizontalPlayerAngle();

    angleH += (playerCameraAngle + CameraLook.deltaPos.x * horizontalAimingSpeed) * Time.deltaTime;
    if (angleH > 360.0f)
    {
      angleH -= 360;
    }
    else if (angleH < -360.0f)
    {
      angleH += 360.0f;
    }
    
    if( !GameSystem.isHomyInThrow )
    {
      tmpJumpAngle += 40 * Time.deltaTime;
      angleV -= tmpJumpAngle * Time.deltaTime;
    }

    angleV = Mathf.Clamp(angleV, -15.0f, maxVerticalAngle);
    aimRotation = Quaternion.Euler(-angleV + defaultVertRotation, angleH, 0);
    cam.rotation = aimRotation;

    if (JoystickController.instance.InputDirection.y < 0.0f)
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.5f, 0.05f);
    }
    else
    {
      camAngleOffset = Mathf.Lerp(camAngleOffset, 1.0f, 0.05f);
    }
  }

  public void SetFallState()
  {
    cameraState = CameraStates.FALL_CAMERA;
  }

  public void SetJumpState()
  {
    tmpJumpAngle = 20.0f;
    cameraState = CameraStates.JUMP_CAMERA;
  }

  public void SetDefaultState()
  {
    if (GameSystem.isFreeCam)
    {
      cameraState = CameraStates.FREE_CAMERA;
    }
    else
    {
      cameraState = CameraStates.DEFAULT_CAMERA;
      if( !GameSystem.isHomyInThrow )
        StartCoroutine("smoothReturnToDefaultVert");
    }
  }

  float maxCamMoveSpeed = 50.0f;
  float camMoveSpeed = 50.0f;
  float minSpeed = 5.0f;
  float speedAcc = 5.0f;
  float rotSpeed = 1.0f;
  float distance;
  float smoothSpeed = 0.05f;
  void Hint_Update()
  {
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, camTarget.position, smoothSpeed);
    transform.position = smoothedPosition;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, camTarget.rotation, 10.0f);
  }

  void HomyAim_Update()
  {
    transform.position = target.position;
    angleH += CameraLook.deltaPos.x * horizontalAimingSpeed * Time.deltaTime;
    if (angleH > 360.0f)
    {
      angleH -= 360;
    }
    else if (angleH < -360.0f)
    {
      angleH += 360.0f;
    }
    angleV += CameraLook.deltaPos.y * horizontalAimingSpeed * Time.deltaTime;

    aimRotation = Quaternion.Euler(-angleV, angleH, 0);

    cam.rotation = aimRotation;
  }

  void ZoomInCam()
  {
    smoothCamOffset -= smoothCamOffset * 0.1f;

  }

  void ZoomOutCam()
  {
    smoothCamOffset += smoothCamOffset * 0.1f;
  }

  public void SetDialogCameraState(Transform dialogSource)
  {
    cameraState = CameraStates.DIALOG_CAMERA;
    target = dialogSource;

    float angleTarget = Vector2.SignedAngle(new Vector2(dialogSource.forward.x, dialogSource.forward.z), new Vector2(-transform.forward.x, -transform.forward.z));
    smoothPivotOffset = new Vector3(0.0f, -3.0f, 0.0f);
    smoothCamOffset = new Vector3(0.0f, 2.0f, -10.0f);

    angleH += angleTarget - 30.0f;// - 45.0f;
    angleV = -10.0f;
  }

  Transform camTarget;
  Vector3 targetDir;
  public void SetHintCameraState(Transform hintPos)
  {
    targetDir = (hintPos.position - transform.position).normalized;
    cameraState = CameraStates.HINT_CAMERA;
    camTarget = hintPos;
  }

  public void ChangeCamTarget(Transform hintPos)
  {
    targetDir = (hintPos.position - transform.position).normalized;
    camTarget = hintPos;
  }

  public void SetDefaultCameraState()
  {
    target = player;
    smoothCamOffset = camOffset;
    smoothPivotOffset = pivotOffset;
    if (GameSystem.isFreeCam)
      cameraState = CameraStates.FREE_CAMERA;

    if (!GameSystem.isFreeCam)
      cameraState = CameraStates.DEFAULT_CAMERA;
  }

  public void SetCameraBehindPlayer()
  {
    
  }

  public void SetHomyAimCameraState()
  {
    cameraState = CameraStates.HOMY_AIM;
    target = player;
  }

  void SmoothCameraRelocate( Transform targetPos )
  {
    //Движение к точке
   // if( Vector3.Distance( ) )

  }

}
