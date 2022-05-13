using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectScript : MonoBehaviour {

  public List<GameObject> animObjects;
  public GameObject wholeObject;
  public Animator animator;
  public Collider destColl;
  public GameObject coinsEffect;
  public DestrSounds destrSounds;

  float timeToDestroy = 2.0f;
  float destroyTimer;
  

  bool isDestroyed;
	// Use this for initialization
	void Start ()
  {

	}

  // Update is called once per frame
  void Update ()
  {
		if( isDestroyed )
    {
      if( destroyTimer < timeToDestroy )
      {
        destroyTimer += Time.deltaTime;
      }
      else
      {
        Destroy(gameObject);
      }
    }
	}

  private void OnCollisionEnter(Collision collision)
  {

  }

  public void DestroyObject()
  {
    wholeObject.SetActive(false);
    coinsEffect.SetActive(true);
    destColl.enabled = false;
    for (int i = 0; i < animObjects.Count; i++)
    {
      animObjects[i].SetActive(true);
    }
    animator.Play("Destroy");
    isDestroyed = true;
    destrSounds.DestrSound();
}


  private void OnTriggerEnter(Collider other)
  {
    if (isDestroyed)
      return;
    if( other.tag == "Staff"  )
    {
      if( StaffController.isStaffInHitt )
      {
        DestroyObject();
      }
    }
  }
}
