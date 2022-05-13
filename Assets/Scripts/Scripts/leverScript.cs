using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverScript : HomyInteractibaleBase {

  public delegate void leverFn();

  public AudioSource audioSource;

  public AudioClip leverlActClip;
  public Animator anim;
  public leverFn leverOn;
  public leverFn leverOff;

  public bool isLeverOn;
  public bool isReusable;

	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update ()
  {
    IsHomyNoticedObject();
	}

  private void OnTriggerEnter(Collider other)
  {
    if( other.tag == "Staff" )
    {
      if( StaffController.isStaffInHitt )
      {
        SwitchLever();
      }
    }

    if( other.tag == "Homy" )
    {
      SwitchLever();
    }
  }

  void SwitchLever()
  {
    if (!isLeverOn)
    {
      anim.SetBool("ShouldAct", true);
      GetComponent<OutlineController>().enabled = false;
      GetComponent<Outline>().enabled = false;
      leverOn();
      PlayeLeverActSound();
      isLeverOn = true;
      this.enabled = false;
    }
    else
    {
      if (isReusable)
      {

      }
    }
  }

  void PlayeLeverActSound()
  {
   // if (!GameSystem.isSoundEnabled)
   //   return;

    audioSource.clip = leverlActClip;
    audioSource.Play();
  }

  public override void Interract()
  {
    base.Interract();
    SwitchLever();
  }
}
