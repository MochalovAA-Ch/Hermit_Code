using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespObjOnLeaveArea : MonoBehaviour {

  public List<GameObject> objectsList;
  private List<Vector3> startPosition;
  private List<float> respawnTimer;
  private List<bool> shouldRespawn;
	// Use this for initialization
	void Start ()
  {
    startPosition = new List<Vector3>();
    respawnTimer = new List<float>();
    shouldRespawn = new List<bool>();
    for ( int i = 0; i < objectsList.Count; i++ )
    {
      startPosition.Add( objectsList[i].transform.position );
      respawnTimer.Add( 0.0f );
      shouldRespawn.Add( false );
    }
  }
	
	// Update is called once per frame
	void Update ()
  {
    for( int i = 0; i < respawnTimer.Count; i++ )
    {
      if( shouldRespawn[i] )
      {
        if( respawnTimer[i] > 3.0f )
        {
          respawnTimer[i] = 0.0f;
          shouldRespawn[i] = false;
          objectsList[i].transform.position = startPosition[i];
        }
        else
        {
          respawnTimer[i] += Time.deltaTime;
        }
      }
    }
	}

  private void OnTriggerExit(Collider other)
  {
    if( other.tag == "MovingObject" )
    {
      for ( int i = 0; i < objectsList.Count; i++ )
      {
        if( other.gameObject == objectsList[i] )
        {
          respawnTimer[i] = 0.0f;
          shouldRespawn[i] = true;
         // objectsList[i].transform.position = startPosition[i];
        }
      }
    }
    //Debug.Log("Покинул триггер");
  }
}
