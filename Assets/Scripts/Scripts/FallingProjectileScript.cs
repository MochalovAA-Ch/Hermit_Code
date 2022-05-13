using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingProjectileScript : MonoBehaviour {

  Transform playerPosition;
  Vector3 startPosition;
  public CoconutProjectile projectile;
  public float explosionDistance;
  Vector3 direction;
  bool isPlayerInTrigger;
  public Transform projLandingArea;

  public float gravity;
  public float respawnTimer;
  public LayerMask layer;
  float currRespTimer;

  RaycastHit hit;

  public float vectXKoef;
  public float raysLength;

  // Use this for initialization
  void Start () {
    playerPosition = FindObjectOfType<CharacterControllerScript>().gameObject.transform;
    startPosition = projectile.rb.position;
  }

  // Update is called once per frame
  void Update()
  {

   /* if (Input.GetKey(KeyCode.P) )
    {
      projectile.rb.position = startPosition;
      Vector3 currPos = startPosition;
      projectile.isHitSomething = false;
      projectile.isLaunched = true;
      //Направление к игроку
      Vector3 vectX = new Vector3(direction.x, 0.0f, direction.z);
      Vector3 velocityX = vectX.normalized * GameUtils.getFallingVx(-direction.y, vectX.magnitude, gravity);
      direction = playerPosition.position - projectile.rb.position;
      Debug.DrawLine(startPosition, startPosition + direction);
      float gravityTst = gravity;
       for (int i = 0; i < 10; i++)
        {
          Debug.DrawLine(currPos, currPos + velocityX.normalized * vectXKoef + new Vector3( 0.0f, gravityTst, 0.0f ), Color.red );
          currPos += velocityX.normalized * vectXKoef + new Vector3(0.0f, gravityTst, 0.0f);
          gravityTst += gravity;
          /*if (Physics.Raycast(currPos, currPos + velocityX + new Vector3(0.0f, gravity, 0.0f), out hit, 5.0f, layer, QueryTriggerInteraction.Ignore))
          {
            projLandingArea.position = hit.point;
            projLandingArea.gameObject.SetActive(true);
            break;
          }
          currPos += velocityX + new Vector3(0.0f, gravity, 0.0f);
        }
    }*/
    if ( !projectile.isLaunched )
    {
      if( currRespTimer < respawnTimer )
      {
        currRespTimer += Time.deltaTime;
      }
      else
      {
        if( isPlayerInTrigger )
        {
          projLandingArea.position = playerPosition.position;
          projLandingArea.gameObject.SetActive(true);
          projectile.rb.position = startPosition;
          Vector3 currPos = startPosition;
          projectile.isHitSomething = false;
          projectile.isLaunched = true;
          //Направление к игроку
          direction = playerPosition.position - projectile.rb.position;
          Vector3 vectX = new Vector3(direction.x, 0.0f, direction.z);
          Vector3 velocityX = vectX.normalized * GameUtils.getFallingVx(-direction.y, vectX.magnitude, gravity);
          projectile.rb.velocity = velocityX;
        }
      }
    }
    else
    {
      if (projectile.isHitSomething)
      {
        if (Vector3.Distance(projectile.transform.position, playerPosition.position) < explosionDistance)
        {
          if( GameSystem.playerCanBeHitted )
          {
            CharacterControllerScript.knockbackVector = (playerPosition.position - projectile.transform.position).normalized;
            EventsManager.TriggerEvent(EventsIds.KNOCKBACK);
            GameSystem.playerLives--;
            EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
          }
        }
        projectile.isLaunched = false;
        projLandingArea.gameObject.SetActive(false);
        projectile.rb.position = startPosition;
        projectile.rb.velocity = Vector3.zero;
        currRespTimer = 0.0f;
      }
      else
      {
        projectile.rb.velocity += new Vector3(0.0f, gravity, 0.0f) * Time.deltaTime;
      }
    }
	}

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      isPlayerInTrigger = true;
    }
  }

    private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
      isPlayerInTrigger = false;
    }
  }

  /*private void OnCollisionEnter(Collision collision)
  {
    if( collision.gameObject.tag == "Player" )
    {
      CharacterControllerScript.knockbackVector = (playerPosition.position - rb.position).normalized;
      EventsManager.TriggerEvent( EventsIds.KNOCKBACK );
      GameSystem.playerLives--;
      EventsManager.TriggerEvent(EventsIds.DECREASE_LIVES);
    }
    Debug.Log("stolknulis s chem-to");
    isLaunched = false;
    rb.position = startPosition;
    rb.velocity = Vector3.zero;
    currRespTimer = 0.0f;
  }*/
}
