using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMovement : MonoBehaviour {

  public Transform Target;
  public Transform Projectile;
  public float firingAngle;
  public float gravity;


  Vector3 direction;
  Vector3 verticalVector;
  float moveTimer;
  // Use this for initialization
  bool isLaunched;

  //Горизонтальный вектор ( кратчайший путь от начала до конца )
  Vector3 horizontalVec;
  Vector3 verticalVec;
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    if (Input.GetKeyDown(KeyCode.P))
      Launch();

    if( isLaunched )
    {
      //if( moveTimer < )
      Debug.DrawRay(transform.position, verticalVec, Color.black);
      //if( flightD)
    }
    else
    {

    }
	}

  void Launch()
  {
    StartCoroutine(ParabolicMovementCorutine());
  }

  IEnumerator ParabolicMovementCorutine()
  {
    float target_Distance = Vector3.Distance(Projectile.position, Target.position);
    horizontalVec = (Target.position - transform.position).normalized;
    verticalVec = transform.up;

    // Calculate the velocity needed to throw the object to the target at specified angle.
    float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

    // Extract the X  Y componenent of the velocity
    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
    float elapsedTime = 0.0f;
    // Calculate flight time.
    float flightDuration = target_Distance / Vx;

    while (elapsedTime < flightDuration )
    {
      direction = horizontalVec * Vx + verticalVec * (Vy - gravity * elapsedTime);
      //Projectile.Translate(direction * Time.deltaTime);
      Projectile.position += direction * Time.deltaTime;
      Projectile.LookAt(Projectile.position + direction);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    Projectile.position = transform.position;
    isLaunched = false;
  }
  // Calculate distance to target


  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(Target.position, Vector3.one);

  }
}
