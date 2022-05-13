using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour {

  public float angle = 90.0f;

  public float speed = 2.0f;

  float startTime = 0.0f;

  Quaternion start, end;

  // Use this for initialization
  void Start ()
  {
    //cashedAmplitudeAngle = BalanceAngle + AmplitudeAngle;
    //coveredAngle = transform.eulerAngles.z - BalanceAngle;
    start = pendulumRotation(angle);
    end = pendulumRotation(-angle);
    
  }
	
	// Update is called once per frame
	void Update ()
  {

    startTime += Time.deltaTime;
    transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(startTime * speed + Mathf.PI) + 1.0f) /2.0f);

  }

  void ResetTimer()
  {
    startTime = 0.0f;
  }

  Quaternion pendulumRotation( float angle )
  {
    var pendulumRotation = transform.rotation;
    var angleZ = pendulumRotation.eulerAngles.z + angle;

    if (angleZ > 180)
      angleZ -= 360;
    else if (angleZ < -180)
      angleZ += 360;

    pendulumRotation.eulerAngles = new Vector3(pendulumRotation.eulerAngles.x, pendulumRotation.eulerAngles.y, angleZ);
    return pendulumRotation;
  }
}
