using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutProjectile : MonoBehaviour {

  public Rigidbody rb;
  public bool isHitSomething;
  public bool isLaunched;
	// Use this for initialization
	void Start ()
  {
   // rb = GetComponent<Rigidbody>();
    isLaunched = false;
    isHitSomething = false;
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnCollisionEnter(Collision collision)
  {
    isHitSomething = true;
    /*if ( collision.gameObject.tag == "Player" )
    {
      if (collision.gameObject.tag == "Player")
      {
        CharacterControllerScript.knockbackVector = (collision.transform.position - rb.position).normalized;
        EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
        GameSystem.playerLives--;
        EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
      }
    }*/
  }


}
