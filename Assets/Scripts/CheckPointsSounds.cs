using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsSounds : MonoBehaviour {

  public AudioSource audioSouce;
  public AudioClip fireClip;

  Transform player;

  bool isPlaying;

  float dist = 20.0f;

	// Use this for initialization
	void Start () {
    player = FindObjectOfType<CharacterControllerScript>().transform;

  }
	
	// Update is called once per frame
	void Update () {

    if( isPlaying )
    {
      if( Vector3.Distance( player.position, transform.position ) > 20.0f )
      {
        audioSouce.volume = 0.0f;
      }
      else if( Vector3.Distance(player.position, transform.position) > 10.0f)
      {
        audioSouce.volume = 0.5f;
      }
      else
      {
        audioSouce.volume = 1.0f;
      }
    }

		
	}

  public void FireSound()
  {
    if( !isPlaying )
    {
      audioSouce.clip = fireClip;
      audioSouce.loop = true;
      audioSouce.Play();
      isPlaying = true;
    }
  }

  public void StopSound()
  {
    isPlaying = false;
    audioSouce.loop = false;
    audioSouce.Stop();
  }
}
