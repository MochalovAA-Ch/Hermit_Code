using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitSoundManager : MonoBehaviour {

  public static HermitSoundManager instance;

  public AudioSource audioSource;
  public AudioSource audioSource2;
  public AudioSource audioSource3;
  public AudioSource bgSoundSource;
  public AudioClip jump;
  public AudioClip doubleJump;
  public AudioClip hitted;
  public AudioClip running;
  public AudioClip walking;
  public AudioClip ground;
  public AudioClip flowerJump;
  public AudioClip getSmth;
  public AudioClip swim;
  public AudioClip climb;
  public AudioClip dirtWalk;
  public AudioClip dirtRun;
  public AudioClip flowerSpring;

  public AudioClip key;
  public AudioClip coin;
  public AudioClip live;

  bool isRunLooping;
  bool isWalkLooping;
  bool isClimbLoop;
  bool isSwimLoop;
  // Use this for initialization
  void Start() {
    instance = this;
  }

  // Update is called once per frame
  void Update() {
    /* if( GameSystem.isMusicEnabled )
     {
       if( !bgSoundSource.isPlaying )
       {
         bgSoundSource.Play();
       }
     }
     else
     {
       if( bgSoundSource.isPlaying )
         bgSoundSource.Stop();
     }*/
    if (!bgSoundSource.isPlaying)
    {
      bgSoundSource.Play();
    }
  }

  public void JumpSound()
  {
    //if (!GameSystem.isSoundEnabled)
     // return;

    audioSource.loop = false;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.clip = jump;
    audioSource.Play();
  }

  public void DoubleJumpSound()
  {
   // if (!GameSystem.isSoundEnabled)
   //   return;

    audioSource.loop = false;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.clip = doubleJump;
    audioSource.Play();
  }

  public void HitedSound()
  {
  //  if (!GameSystem.isSoundEnabled)
   //   return;

    audioSource.loop = false;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.clip = hitted;
    audioSource.Play();
  }

  public void RuningSound()
  {
    //if (!GameSystem.isSoundEnabled)
     // return;

    if (audioSource.clip == running)
      return;

    audioSource.clip = running;
    audioSource.loop = true;
    audioSource.Play();

   /* if ( !isRunLooping )
    {
      audioSource.clip = running;
      audioSource.loop = true;
      audioSource.Play();
      isRunLooping = true;
      isWalkLooping = false;
      isSwimLoop = false;
      isClimbLoop = false;
    }*/
  }

  public void StopSound()
  {
    if (audioSource.clip == hitted)
      return;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.Stop();
  }

  public void GroundedSound()
  {
   // if (!GameSystem.isSoundEnabled)
     // return;

    audioSource.loop = false;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.clip = ground;
    audioSource.Play();
  }

  public void WalkSound()
  {
   // if (!GameSystem.isSoundEnabled)
   //   return;

    if( audioSource.clip == walking )
    {
      return;
    }

    audioSource.clip = walking;
    audioSource.loop = true;
    audioSource.Play();

   /* if (!isWalkLooping)
    {
      audioSource.clip = walking;
      audioSource.loop = true;
      audioSource.Play();
      isWalkLooping = true;
      isRunLooping = false;
      isSwimLoop = false;
      isClimbLoop = false;
    }*/
  }

  public void FlowerJump()
  {
   // if (!GameSystem.isSoundEnabled)
    //  return;

    audioSource.loop = false;
    isRunLooping = false;
    isWalkLooping = false;
    isSwimLoop = false;
    isClimbLoop = false;
    audioSource.clip = flowerJump;
    audioSource.Play();

    audioSource3.clip = flowerSpring;
    audioSource3.Play();
  }

  public void GetSmth()
  {
   // if (!GameSystem.isSoundEnabled)
    //  return;

    audioSource2.clip = getSmth;
    audioSource2.Play();
  }

  public void Swim()
  {
   // if (!GameSystem.isSoundEnabled)
   //   return;

    if (audioSource.clip == swim)
      return;

    audioSource.clip = swim;
    audioSource.loop = true;
    audioSource.Play();
    /* if (!isSwimLoop)
     {
       audioSource.clip = swim;
       audioSource.loop = true;
       audioSource.Play();
       isWalkLooping = false;
       isRunLooping = false;
       isSwimLoop = true;
       isClimbLoop = false;
     }*/
  }

  public void Climb()
  {
   // if (!GameSystem.isSoundEnabled)
   //   return;

    if (audioSource.clip == climb)
      return;

    audioSource.clip = climb;
    audioSource.loop = true;
    audioSource.Play();
    /*  if (!isClimbLoop)
      {
        audioSource.clip = climb;
        audioSource.loop = true;
        audioSource.Play();
        isWalkLooping = false;
        isRunLooping = false;
        isSwimLoop = false;
        isClimbLoop = true;
      }*/
  }

  public void DirtWalk()
  {
  //  if (!GameSystem.isSoundEnabled)
  //    return;
    if (audioSource.clip == dirtWalk)
      return;

    audioSource.clip = dirtWalk;
    audioSource.loop = true;
    audioSource.Play();
  }

  public void DirtRun()
  {
  //  if (!GameSystem.isSoundEnabled)
  //    return;
    if (audioSource.clip == dirtRun)
      return;

    audioSource.clip = dirtRun;
    audioSource.loop = true;
    audioSource.Play();
  }

  public void GetKey()
  {
  //  if (!GameSystem.isSoundEnabled)
  //    return;

    audioSource2.clip = key;
    audioSource2.Play();
  }

  public void GetCoin()
  {
  //  if (!GameSystem.isSoundEnabled)
   //   return;

    audioSource2.clip = coin;
    audioSource2.Play();
  }

  public void GetLive()
  {
   // if (!GameSystem.isSoundEnabled)
    //  return;

    audioSource2.clip = live;
    audioSource2.Play();
  }



}
