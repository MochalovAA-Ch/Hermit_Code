using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

  public float maxSinkDistance;
  public float sinkSpeed;
  public float reSinkSpeed;
  public float startReSinkTimer;


  bool isSink = false;
  bool isReSink = false;


  float sinkDistance;

  Transform tr;
  Vector3 startPosition;


	// Use this for initialization
	void Start ()
  {
    tr = GetComponent<Transform>();
    startPosition = tr.position;
	}
	
	// Update is called once per frame
	void Update ()
  {

    if ( isSink )
    {
      if ( sinkDistance < maxSinkDistance )
      {
        sinkDistance += sinkSpeed * Time.deltaTime;
        tr.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
      }
      else
      {
        isSink = false;
      }
    }
    else if ( isReSink )
    {
      if ( sinkDistance > 0 )
      {
        sinkDistance -= reSinkSpeed * Time.deltaTime;
        tr.Translate( Vector3.up * reSinkSpeed * Time.deltaTime );
      }
      else
      {
        tr.position = startPosition;
        sinkDistance = 0.0f;
        isReSink = false;
      }
    }
	}

  public void StartSink()
  {
    isReSink = false;
    isSink = true;
  }

  public void ReSink()
  {
    isReSink = true;
    isSink = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Player" )
    {
      StartSink();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
      ReSink();
    }
  }

}
