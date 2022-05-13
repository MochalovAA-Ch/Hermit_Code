using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocSounds : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip crocGroan;
  public AudioClip crocBite;
  public AudioClip crocHitted;

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

  public void CrocGroan( float dist )
  {

    audioSource.volume = GameUtils.LinearSoundFunction2(dist);

    if (audioSource.clip == crocGroan)
    {
      return;
    }

    audioSource.clip = crocGroan;
    audioSource.loop = true;
    audioSource.Play();

  }

  public void CrocBite( float dist )
  {

    //Debug.Log(Time.time + "  asdasds");
    audioSource.volume = GameUtils.LinearSoundFunction2(dist);
    //Debug.Log(audioSource.volume);
    audioSource.loop = false;
    audioSource.clip = crocBite;
    audioSource.Play();
  }

  public void CrocHited( float dist )
  {

    if ( !isHited )
    {
      audioSource.Stop();
      audioSource.clip = crocHitted;
      audioSource.loop = false;
      audioSource.Play();
      isGroan = false;
      isBite = false;
      isHited = true;
    }
  }

}
