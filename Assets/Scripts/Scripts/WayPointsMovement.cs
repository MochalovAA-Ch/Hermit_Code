using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsMovement : MonoBehaviour {

  public List<Transform> wayPoints;
  public Transform movingObject;
  public float speed;
  public bool isReverse;

  Vector3 moveDirection;
  Vector3 targetPosition;

  int targetPointIndex;

  bool reachedEnd;

  // Use this for initialization
  void Start ()
  {
    targetPointIndex = 0;
    targetPosition = wayPoints[targetPointIndex].position;
    moveDirection = wayPoints[targetPointIndex].position - targetPosition;
  }
	
	// Update is called once per frame
	void Update ()
  {
    Move();
	}

  public void Move()
  {
    if( isReverse )
    {
      //Если достигли точки назначения, меняем точку назначения
      if ( Vector3.Distance( targetPosition, movingObject.position ) < speed * Time.deltaTime)
      {
        //Достигнули последней точки
        if( reachedEnd )
        {
          //Если целевая точка была самой первой в пути, указываем что нужно двигатся в обратном направлении
          if( targetPointIndex == 0 )
          {
            reachedEnd = false;
            targetPointIndex++;
          }
          else
          {
            targetPointIndex--;
          }
        }
        //Иначе движемся к последней точке
        {
          //Достигли конца
          if( targetPointIndex == wayPoints.Count - 1 )
          {
            reachedEnd = true;
            targetPointIndex--;
          }
          else
          {
            targetPointIndex++;
          }
        }      

        targetPosition = wayPoints[targetPointIndex].position;
        moveDirection = (targetPosition - movingObject.transform.position).normalized;
      }
      //Иначе просто двигаемся к точке назначения
      else
      {
        movingObject.position += moveDirection * speed * Time.deltaTime;
      }
    }
    else
    {
      if (Vector3.Distance(targetPosition, movingObject.position) < speed * Time.deltaTime)
      {
        if (targetPointIndex == wayPoints.Count - 1)
          targetPointIndex = 0;
        else
          targetPointIndex++;
        targetPosition = wayPoints[targetPointIndex].position;
        moveDirection = (targetPosition - movingObject.transform.position).normalized;
      }
      else
      {
        movingObject.position += moveDirection * speed * Time.deltaTime;
      }
    }
  }

}
