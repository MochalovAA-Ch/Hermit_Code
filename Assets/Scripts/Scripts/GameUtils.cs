using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  //Функция устанавливает bool параметр paramName аниматора в true, остальные в false
  public static void SetAnimState(Animator animator, string paramName )
  {
    for (int i = 0; i < animator.parameterCount; i++)
    {
      AnimatorControllerParameter param = animator.parameters[i];
      if ( param.name != paramName)
      {
        animator.SetBool(param.name, false);
      }
      else
      {
        animator.SetBool(param.name, true);
      }
    }
  }

  //Горизонтальная скорость 
  public static float getFallingVx(float h, float xDistance, float gravity )
  {
    float fallTime = Mathf.Sqrt(2 * h / Mathf.Abs(gravity));
    return xDistance / fallTime;
  }

  public static float getDistanceByXZ(Vector3 a, Vector3 b)
  {
    return (Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.z - a.z) * (b.z - a.z)));
  }

  public static float getDistanceByXZ(Vector3 a)
  {
    return Mathf.Sqrt(a.x*a.x + a.z*a.z );
  }

  public static float getSignedAngle( float unsingedAngle )
  {
    return unsingedAngle < 180 ? unsingedAngle : unsingedAngle - 360.0f;
  }

  //Вращает угол налево относительно направления угла в тригонометрическом круге( обход справа налево )
  public static float RotateSignedAngleToLeft( float angle, float rotAngle )
  {
    if( angle > 0  )
    {
      return getSignedAngle(angle + rotAngle);
    }
    else
    {
      return angle + rotAngle;
    }
  }

  //Вращает угол направо относительно направления угла в тригонометрическом круге( обход справа налево )
  public static float RotateSignedAngleToRight( float angle, float rotAngle)
  {
    float tmpAngle = angle - rotAngle;
    if ( angle < 0 )
    {
      if ( tmpAngle < -180.0f )
      {
        return 360.0f + (angle - rotAngle);
      }
      else
      {
        return tmpAngle;
      }
    }
    else
    {
      return tmpAngle;

    }
  }

  public static float LinearSoundFunction(float x)
  {
    return -x * 0.02f + 1.0f;
  }

  public static float LinearSoundFunction2(float x)
  {
    return -x * 0.01f + 1.0f;
  }

  /**
   * Возвращает гравитацию, которая должна быть, чтобы обьект прошел с заданными скоростью vx и vy путь distance
   * и поднялся на максимальную высоту h
   * 
   * 
   * */
  public static float getGravity( float distance, float h, float vx, float vy )
  {
    float gravity = 0;
    float t = distance / vx;
    float th = t / 2;
    gravity = vy * th - h / (th * th);
    return -gravity;
  }

  public static void SetRotationToDirection( Transform meshTr, Vector3 moveDirection )
  {
    float angle = -Vector3.SignedAngle(moveDirection, Vector3.forward, Vector3.up);
    meshTr.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
  }

  public static void Get_XZ_DirectionToPlayer( out Vector3 moveDirection, Transform target )
  {
    moveDirection = SceneGeneralObjects.instance.playerTr.position - target.position;
    moveDirection.y = 0;
  }

}
