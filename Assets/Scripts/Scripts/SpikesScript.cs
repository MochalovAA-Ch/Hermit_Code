using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour {

  public FloatingPlatform floatingPlatformScript;
	// Use this for initialization
	void Start ()
  {
	  
	}
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    //Если шипы едут  вперед, то можем задеть

      if( other.tag == "Player" )
      {
        if( GameSystem.playerCanBeHitted )
        {
          CharacterControllerScript.knockbackVector =  (other.transform.position - transform.position).normalized;
          EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
          GameSystem.playerLives--;
          EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
        }
      }
    
  }

}
