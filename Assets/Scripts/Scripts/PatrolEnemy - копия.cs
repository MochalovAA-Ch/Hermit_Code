using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy2 : MonoBehaviour {

  public List<Transform> wayPoints;
  public int targetPointIndex;
  public float defaultSpeed;
  public float chaseSpeed;
  public float gravity;
  public bool isReverse = false;
  public Transform patrolMeshTr;
  public CharacterController crocCharController;

  Vector3 moveDirection;
  Vector3 targetPosition;
  Vector3 gravityVec;

  Transform playerTr;
  Transform charTr;
  float speed;
  float swampSurface;
  Animator animator;

  bool isChasing;

	// Use this for initialization
	void Start ()
  {
    targetPointIndex = 0;
    targetPosition = wayPoints[targetPointIndex].position;
    //moveDirection = (targetPosition - patrolMeshTr.position ).normalized;
    moveDirection.y = 0.0f;
    SetRotationToDirection();
    speed = defaultSpeed;
    animator = patrolMeshTr.GetComponent<Animator>();
    playerTr = FindObjectOfType<CharacterControllerScript>().transform;
    crocCharController.transform.position = new Vector3(targetPosition.x, crocCharController.transform.position.y, targetPosition.z);
    swampSurface = crocCharController.transform.position.y;
  }
	
	// Update is called once per frame
	void Update ()
  {
    //1.Патрулирование
    if( !isChasing )
    {
      //Круговое
      if (!isReverse)
      {
        if (getDistanceByXZ(crocCharController.transform.position, targetPosition) < speed * Time.deltaTime)
        {
          crocCharController.transform.position = new Vector3(targetPosition.x, crocCharController.transform.position.y, targetPosition.z);
          if (targetPointIndex == wayPoints.Count - 1)
            targetPointIndex = 0;
          else
            targetPointIndex++;
          targetPosition = wayPoints[targetPointIndex].position;
          moveDirection = (targetPosition - crocCharController.transform.position).normalized;
          moveDirection.y = 0.0f;
          SetRotationToDirection();
        }
        else
        {
          crocCharController.Move(moveDirection * speed * Time.deltaTime);
        }
      }
      //Туда-сюда
      else
      {

      }
    }
    else
    {
      moveDirection = (playerTr.position - crocCharController.transform.position).normalized;
      moveDirection.y = 0;
      if (crocCharController.transform.position.y > swampSurface + gravity * Time.deltaTime )
      {
        gravityVec.y = gravity;
      }
      else
      {
        gravityVec = Vector3.zero;
      }
        
      SetRotationToDirection();
      crocCharController.Move(( moveDirection * speed  + gravityVec) * Time.deltaTime );

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
    if ( other.tag == "Player" )
    {
      isChasing = true;
    }
    
    //Debug.Log("playare atatatatatat");
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
      isChasing = false;
      SetReturnDirection();
    }
  }


  float getDistanceByXZ( Vector3 a, Vector3 b )
  {
    return (Mathf.Sqrt( ( b.x - a.x ) * (b.x - a.x) + (b.z - a.z) * (b.z - a.z) ) );
  }

  void SetRotationToDirection()
  {
    float angle = Vector3.Angle(moveDirection, Vector3.forward);
    if (moveDirection.x < 0)
      angle = -angle;
    patrolMeshTr.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
  }

  void SetReturnDirection()
  {
    float minDistance = getDistanceByXZ(wayPoints[0].position, patrolMeshTr.position);
    int minIndex = 0;
    for (int i = 1; i < wayPoints.Count; i++)
    {
      if (Vector3.Distance(wayPoints[i].position, patrolMeshTr.position) < minDistance)
      {
        minDistance = Vector3.Distance(wayPoints[i].position, patrolMeshTr.position);
        minIndex = i;
      }
    }
    targetPointIndex = minIndex;
    moveDirection = (wayPoints[targetPointIndex].position - patrolMeshTr.position).normalized;
    moveDirection.y = 0.0f;
    SetRotationToDirection();
  }
}

