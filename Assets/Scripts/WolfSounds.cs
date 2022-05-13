using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSounds : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip wolfGroan;
  public AudioClip wolfBite;
  public AudioClip wolfHitted;

  bool isGroan;
  bool isHited;
  bool isBite;
  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
  }

  public void WolfGroan( float dist )
  {
    audioSource.volume = GameUtils.LinearSoundFunction2(dist);

    if (audioSource.clip == wolfGroan)
    {
      return;
    }

    audioSource.clip = wolfGroan;
    audioSource.loop = true;
    audioSource.Play();

  }

  public void WolfBite( float dist )
  {
    //Debug.Log(Time.time + "  asdasds");
    audioSource.volume = GameUtils.LinearSoundFunction2(dist);
    audioSource.loop = false;
    audioSource.clip = wolfBite;
    audioSource.Play();
  }

  public void WolfHited( float dist )
  {
    if( !isHited )
    {
      audioSource.Stop();
      audioSource.clip = wolfHitted;
      audioSource.loop = false;
      audioSource.Play();
      isGroan = false;
      isBite = false;
      isHited = true;
    }
  }

}
