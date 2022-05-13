using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrSounds : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip destrSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void DestrSound()
  {
    audioSource.clip = destrSound;
    audioSource.Play();
  }

}
