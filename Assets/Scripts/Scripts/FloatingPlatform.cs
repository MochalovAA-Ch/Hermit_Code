using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

  [SerializeField]
  Transform platform;

  [SerializeField]
  Transform startPosition;

  [SerializeField]
  Transform endPosition;

  Transform tr;
  Transform playerPos;

  public float platformStartToEndTime;
  public float platformEndToStartTime;

  float platformFwdSpeed;
  float platformBckwSpeed;


  //Когда объект начинает работать
  public float startMovingTime;
  float startMovingTimer;

  public float startToEndDelay;
  public float endToStartDelay;
  float startToEndDelayTimer;
  float endToStartDelayTimer;
  float moveTimer;

  public Vector3 moveVector3;
  Vector3 distanceector;

  public bool shouldMoveForward = true;


  bool isPlayMoveSound;
  AudioSource audioSource;
  public AudioClip moveTowardsClip;
  public AudioClip moveBackwardsClip;

  // Use this for initialization
  void Start()
  {
    //audioSource = platform.GetComponent<AudioSource>();
    //audioSource.clip = 
    //audioSource = GetComponent<AudioSource>();
    //audioSource.clip = moveTowardsClip;
    //audioSource.Play();
    //audioSource.maxDistance = 50.0f;
    //playerPos = FindObjectOfType<CharacterControllerScript>().transform;
    tr = platform.GetComponent<Transform>();
    //tr.position = startPosition.position;
    moveVector3 = (endPosition.position - startPosition.position).normalized;
    startToEndDelayTimer = 0.0f;
    endToStartDelayTimer = 0.0f;

    float distance = Vector3.Distance(startPosition.position, endPosition.position);
    platformFwdSpeed = distance / platformStartToEndTime;
    platformBckwSpeed = distance / platformEndToStartTime;
  }

  // Update is called once per frame
  void Update()
  {
    if (startMovingTimer < startMovingTime)
    {
      startMovingTimer += Time.deltaTime;
      return;
    }

    if (shouldMoveForward)
    {
      if (startToEndDelayTimer < startToEndDelay)
      {
        startToEndDelayTimer += Time.deltaTime;
      }
      else
      {
        if (Vector3.Distance(tr.position, endPosition.position) > platformFwdSpeed * Time.deltaTime)
        {
          tr.position += moveVector3 * platformFwdSpeed * Time.deltaTime;
          //PlayMoveClip(moveTowardsClip);
        }
        else
        {
          StopMoveSound();
          tr.position = endPosition.position;
          //moveVector3 *= -1;
          shouldMoveForward = false;
          startToEndDelayTimer = 0.0f;
        }
      }
    }
    else
    {
      if (endToStartDelayTimer < endToStartDelay)
      {
        endToStartDelayTimer += Time.deltaTime;
      }
      else
      {
        if (Vector3.Distance(tr.position, startPosition.position) > platformBckwSpeed * Time.deltaTime)
        {
          tr.position -= moveVector3 * platformBckwSpeed * Time.deltaTime;
          //PlayMoveClip(moveBackwardsClip);
        }
        else
        {
          StopMoveSound();
          tr.position = startPosition.position;
          //moveVector3 *= -1;
          shouldMoveForward = true;
          endToStartDelayTimer = 0.0f;
        }
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (startPosition == null || endPosition == null || platform == null)
      return;

    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(startPosition.position, platform.localScale);
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(endPosition.position, platform.localScale);


    float distance = Vector3.Distance(startPosition.position, endPosition.position);
    platformFwdSpeed = distance / platformStartToEndTime;
    platformBckwSpeed = distance / platformEndToStartTime;
    //Handles.Label(transform.position, "speedToStart " + platformBckwSpeed + " speedToEnd " + platformBckwSpeed);
  }


  void PlayMoveClip( AudioClip newClip)
  {
    return;
    if( !GameSystem.isSoundEnabled )
    {
      audioSource.Stop();
      return;
    }

    if (!audioSource.isPlaying)
    {
      audioSource.clip = newClip;
    }
      

    float distance = Vector3.Distance(playerPos.position, platform.position);
    if( distance > 50.0f )
    {
      audioSource.Stop();
    }
    else
    {
      if (!audioSource.isPlaying)
      {
        audioSource.loop = true;
        audioSource.Play();
      }
      audioSource.volume = GameUtils.LinearSoundFunction(distance);
    }
  }

  void StopMoveSound()
  {
   // audioSource.Stop();
  }

 

}
