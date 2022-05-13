using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharController : MonoBehaviour
{

  public JoystickController joystick;
  public Vector3 halfCheckGroundedBox;
  public Transform charTr;
  public Transform cameraTransform;
  public float moveSpeed;
  public float defaultSpeed;
  public float jumpForce;
  public float gravityAccForce;
  bool isJumping;
  bool isDoubleJumping;
  bool canDoubleJump;

  Rigidbody rb;
  public float velocityY;
  public Vector3 drawCheckBoxCenter;
  CapsuleCollider capsule;
  public bool isGrounded;
  RaycastHit isGroundedHit;

  public bool shouldSlide = false;

  public LayerMask layer;

  public Vector3 castSpherePoint;
  Vector3 moveVector;
  Vector3 velocityVector;

  float angleRot;
  public float keyboardTurnSpeed;
  float movinAxis;
  float movingPlatformsRot;

  Vector3 movingRbVelocity;

  //Переменные с char controllera
  Vector2 cameraTransformForwardXZ;
  float camAngle;


  private void Start()
  {
    rb = GetComponent<Rigidbody>();
    //velocityY = new Vector3( 0.0f, -9.8f, 0.0f );
    isGrounded = true;
    capsule = GetComponent<CapsuleCollider>();
    moveVector = Vector3.zero;//transform.forward;
    defaultSpeed = moveSpeed;
    //drawCheckBoxCenter = capsule.center;
    // drawCheckBoxCenter.y = -capsule.height * 0.5f - halfCheckGroundedBox.y ;
    //castSpherePoint.y = capsule.height/4 + 0.1f;
    movingPlatformsRot = 0.0f;
  }

  int updateCounter = 0;
  int fixedUpdateCounter = 0;

  private void Update()
  {
    cameraTransformForwardXZ.x = cameraTransform.forward.x;
    cameraTransformForwardXZ.y = cameraTransform.forward.z;
    camAngle = Vector2.Angle(cameraTransformForwardXZ, Vector2.up);
    if (cameraTransform.forward.x < 0)
      camAngle *= -1;
    /*angleRot = joystick.InputDirection.x < 0 ? -Vector3.Angle(Vector3.up, joystick.InputDirection) :
    Vector3.Angle(Vector3.up, joystick.InputDirection);*/


    angleRot += Input.GetAxis("Horizontal") * keyboardTurnSpeed;
    movinAxis = Input.GetAxis("Vertical");


    charTr.rotation = Quaternion.Euler(0.0f, angleRot, 0.0f);
    //moveVector = charTr.forward * joystick.InputDirection.magnitude;
    moveVector = charTr.forward * movinAxis;
    //Debug.Log(angleRot);
    /*if (joystick.InputDirection.magnitude > 0)
      transform.rotation = Quaternion.Euler(0.0f, angleRot, 0.0f);*/
    if (Input.GetKeyDown(KeyCode.Space) )
    {
      Jump();
    }
    //Проверка сферой

    if (Physics.CheckBox(transform.position + castSpherePoint, halfCheckGroundedBox, Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
    {
      // Debug.Log("Попадаем сферой");
      isGrounded = true;
      canDoubleJump = true;
      //isJumping = false;
    }
    else
    {
      isGrounded = false;
    }
    //Debug.Log(rb.velocity.y);
    //shouldSlide = false;
  }

  private void FixedUpdate()
  {
    //С помощью гравитации
    if (shouldSlide && !isGrounded)
      return;

    //velocityVector.x = moveVector.x * moveSpeed * movinAxis + movingRbVelocity.x - rb.velocity.x;
    //velocityVector.z = moveVector.z * moveSpeed * movinAxis + movingRbVelocity.z - rb.velocity.z;

    velocityVector.x = moveVector.x * moveSpeed * movinAxis  - rb.velocity.x;
    velocityVector.z = moveVector.z * moveSpeed * movinAxis  - rb.velocity.z;
    //velocityVector.y = 0.0f;

    if (isJumping)
    {
      velocityVector.y -= rb.velocity.y;
    }
    else if ( isDoubleJumping )
    {
      velocityVector.y -= rb.velocity.y;
    }
            

    rb.AddForce(velocityVector, ForceMode.VelocityChange);

    if (isJumping)
    {
      velocityVector.y -= rb.velocity.y;
      isJumping = false;
      velocityVector.y = 0.0f;
    }
    else if ( isDoubleJumping )
    {
      velocityVector.y -= rb.velocity.y;
      isJumping = false;
      isDoubleJumping = false;
      velocityVector.y = 0.0f;
    }
  }



  void Jump()
  {
    if (isGrounded)
    {
      isJumping = true;
      velocityVector.y = jumpForce;//velocityY = jumpForce;
    }
    else
    {
      if (!isDoubleJumping && canDoubleJump )
      {
        isDoubleJumping = true;
        canDoubleJump = false;
        velocityVector.y = jumpForce;//velocityY = jumpForce;
      }
    }
  }

  void OnDrawGizmosSelected()
  {
    //charTransform = GetComponent<Transform>();
    Gizmos.color = Color.black;
    //charTransform.position + Vector3.up, 0.3f ,charTransform.forward, out checkHitInfo, 3.0f
    Gizmos.DrawCube(transform.position + castSpherePoint, halfCheckGroundedBox * 2);
  }

  private void OnCollisionEnter(Collision collision)
  {
    shouldSlide = true;

    if (collision.transform.CompareTag("MovingPlatform") )
    {
      //movingRbVelocity = collision.rigidbody.velocity;
     // movingPlatformsRot = collision.rigidbody.rotation.eulerAngles.y;
    }

    if ( collision.transform.CompareTag("RotationPlatform") )
    {
      //movingRbVelocity = collision.rigidbody.velocity;
      movingPlatformsRot = collision.rigidbody.rotation.eulerAngles.y;
    }

  }

  private void OnCollisionStay(Collision collision)
  {
    shouldSlide = true;
    if (collision.transform.CompareTag("MovingPlatform"))
    {
      movingRbVelocity = collision.rigidbody.velocity;
      //movingPlatformsRot = collision.rigidbody.rotation.eulerAngles.y;
    }

    if (collision.transform.CompareTag("RotationPlatform"))
    {
      //movingRbVelocity = collision.rigidbody.velocity;
      movingPlatformsRot = collision.rigidbody.rotation.eulerAngles.y;
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    shouldSlide = false;
    movingRbVelocity = Vector3.zero;
    movingPlatformsRot = 0.0f;
  }
}
