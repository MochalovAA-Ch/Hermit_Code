using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupolButton : MonoBehaviour {

  AudioSource audioSource;
  public AudioClip boxSetUp;
  // public List<GameObject> movingBoxes;
  public GameObject cupolRay;

  bool isPressed = false;
	// Use this for initialization
	void Start () {
    audioSource = GetComponent<AudioSource>();
    audioSource.clip = boxSetUp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "MovableObject")
    {
      if ( !isPressed )
      {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z );
        other.transform.rotation = Quaternion.identity;

        other.GetComponent<OutlineController>().enabled = false;
        other.GetComponent<Outline>().enabled = false;
        other.GetComponent<PickableObject>().enabled = false;
        GameUIController.instance.SetDefaultIcon();
        GameUIController.instance.HideInterractIcon();
        audioSource.Play();
        isPressed = true;
        Destroy(cupolRay);
        //cupolRay.SetActive(false);
        EventsManager.TriggerEvent(EventsIds.CUPOL_BUTTON_BOX_ENTER);
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {

  }
}
