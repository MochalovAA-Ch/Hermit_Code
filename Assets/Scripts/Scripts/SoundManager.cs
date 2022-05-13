using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

  public static SoundManager instance;
  public AudioSource playerSource;
 // public AudioSource p

  public AudioClip background;
  public AudioClip hit;
  public AudioClip jump;
  public AudioClip doubleJump;
  public AudioClip getLive;
  public AudioClip getKey;
  public AudioClip getCoin;
  //public 


  // Use this for initialization
  void Start () {

    instance = this;
    //musicSource.clip = background;
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.H))
    {
     // musicSource.Play();
    }
     // background
		
	}

  public void PlayJumpSound()
  {
    playerSource.clip = jump;
    playerSource.Play();
  }

  public void PlayDoubleJumpSound()
  {
    playerSource.clip = doubleJump;
    playerSource.Play();
  }
}
