using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShootingController : MonoBehaviour {

  public ParabolicShooting parabolicShooting;

  public float shootCooldown;
  float shootCooldownTimer;

	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update ()
  {
    if ( !parabolicShooting.isLaunched )
    {
      if( shootCooldownTimer < shootCooldown )
      {
        parabolicShooting.Projectile.gameObject.SetActive(false);
        shootCooldownTimer += Time.deltaTime;
      }
      else
      {
        parabolicShooting.Projectile.gameObject.SetActive(true);
        parabolicShooting.ThrowProjectile();
        shootCooldownTimer = 0.0f;
      }
    }
	}
}
