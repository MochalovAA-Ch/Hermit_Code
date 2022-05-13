using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSounds : MonoBehaviour {

  public static StaffSounds instance;
  public AudioSource audioSource;
  public AudioClip staffSwipe;
  public AudioClip staffHit;

	// Use this for initialization
	void Start () {
    instance = this;
    
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnEnable()
  {

  }

  private void OnDestroy()
  {

  }

  public void StaffSwipe()
  {
    if (!GameSystem.isSoundEnabled)
      return;
    audioSource.clip = staffSwipe;
    audioSource.Play();
  }
  public void StaffHit()
  {
    if (!GameSystem.isSoundEnabled)
      return;
    audioSource.clip = staffHit;
    audioSource.Play();
  }
}
