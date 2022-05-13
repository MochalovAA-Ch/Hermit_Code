using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShooting : MonoBehaviour {

  public Transform Target;
  public float firingAngle = 45.0f;
  public float gravity = 9.8f;

  public bool isLaunched;

  public Transform Projectile;
  private Transform myTransform;

  Vector3 direction;

  void Awake()
  {
    myTransform = transform;
  }

  void Start()
  {

  }

  IEnumerator SimulateProjectile()
  {
    isLaunched = true;

    // Move projectile to the position of throwing object + add some offset if needed.
    Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

    // Calculate distance to target
    float target_Distance = Vector3.Distance(Projectile.position, Target.position);
    direction = (Projectile.position - Target.position).normalized;
    Vector3 horizontalVec = (Target.position - transform.position).normalized;
    Vector3 verticalVec = transform.up;

    // Calculate the velocity needed to throw the object to the target at specified angle.
    float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

    // Extract the X  Y componenent of the velocity
    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

    // Calculate flight time.
    float flightDuration = target_Distance / Vx;
    
    float elapse_time = 0;

    while (elapse_time < flightDuration)
    {
      direction = horizontalVec * Vx + verticalVec * (Vy - gravity * elapse_time);
      Projectile.position += direction * Time.deltaTime;
      Projectile.LookAt(Projectile.position + direction);
      elapse_time += Time.deltaTime;
      yield return null;
    }
    Projectile.gameObject.SetActive(false);
    Projectile.position = myTransform.position;
    isLaunched = false;
  }

  public void ThrowProjectile()
  {
    StartCoroutine(SimulateProjectile());
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube( Target.position, Vector3.one );

  }
}
