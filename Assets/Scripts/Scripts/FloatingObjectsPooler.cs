using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectsPooler : MonoBehaviour {

  [SerializeField]
  Transform platform;

  [SerializeField]
  Transform startPosition;

  [SerializeField]
  Transform endPosition;

  public float platformsSpeed;

  public Vector3 direction;

  public List<Transform> platformsList;

  int platformIndex;
  int platformsCount;

	// Use this for initialization
	void Start () {
    direction = ( endPosition.localPosition - startPosition.localPosition ).normalized;
    platformsCount = platformsList.Count;
    platformIndex = 0;
  }

  private void Update()
  {
    if ( Vector3.Distance( platformsList[platformIndex].position, endPosition.position  ) < platformsSpeed * Time.deltaTime)
    {
      platformsList[platformIndex].position = startPosition.position;

      if( platformIndex == platformsCount - 1 )
      {
        platformIndex = 0;
      }
      else
      {
        platformIndex++;
      }
    }

    for ( int i = 0; i < platformsCount; i++ )
    {
      platformsList[i].Translate( direction * platformsSpeed * Time.deltaTime );
      //platformsList[i].velocity = direction * platformsSpeed * Time.fixedDeltaTime;

    }
  }

  // Update is called once per frame
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube( startPosition.position, new Vector3( 1, 1 ,1 ) );
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube( endPosition.position, new Vector3(1, 1, 1) );
  }
}
