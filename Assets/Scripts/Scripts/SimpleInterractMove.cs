using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInterractMove : MonoBehaviour {

  public bool automaticMoveOnEnable;

  public Transform objectTr;
  public Transform startPos;
  public Transform endPos;

  public float platformStartToEndTime;
  float speedForward;

  Vector3 moveDir;
  bool shouldMove;

  private void OnEnable()
  {
    if( automaticMoveOnEnable )
    {
      shouldMove = true;
    }
  }

  // Use this for initialization
  void Start ()
  {
    moveDir = (endPos.position - startPos.position).normalized;
    float distance = Vector3.Distance( startPos.position, endPos.position );
    speedForward = distance / platformStartToEndTime;
  }

  // Update is called once per frame
  void Update()
  {
    if( shouldMove )
    {
      if ( Vector3.Distance( objectTr.position, endPos.position )  < speedForward * Time.deltaTime )
      {
        shouldMove = false;
        return;
      }
      else
      {
        objectTr.position += moveDir * speedForward * Time.deltaTime;
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (!automaticMoveOnEnable)
      return;
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(startPos.position, Vector3.one);
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(endPos.position, Vector3.one);
  }
}
