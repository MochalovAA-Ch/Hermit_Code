using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsHitable : MonoBehaviour {

  //Количество ударов для уничтожения
  public int hitsToDestroy;

  List<Transform> children;

  //Количество ударов, сделанных по объекту
  int hits;
  
	// Use this for initialization
	void Start () {
    children = new List<Transform>();
    int childrenCount = transform.childCount;
    for ( int i = 0; i < childrenCount; i++ )
    {
      children.Add(transform.GetChild(i));
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnTriggerEnter( Collider other )
  {
    if ( StaffController.isStaffInHitt )
    {
      if (children.Count != 0)
      {
        children[children.Count - 1].gameObject.SetActive(false);
        children.RemoveAt(children.Count - 1);
        if (children.Count == 0)
        {
          gameObject.SetActive(false);
        }

      }
      else
      {
        gameObject.SetActive(false);
      }
    }
  }

}
