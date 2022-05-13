using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip background_setting1;

	// Use this for initialization
	void Start ()
  {
    audioSource.clip = background_setting1;
    audioSource.Play();
  }
	
	// Update is called once per frame
	void Update ()
  {
		
	}
}
