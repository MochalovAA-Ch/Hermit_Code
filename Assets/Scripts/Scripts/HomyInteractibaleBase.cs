using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс для интерактивных объектов, взаимодействующих с хому ( путем их обнаружение и подлета к ним хоуми )
public class HomyInteractibaleBase : MonoBehaviour
{
  public static HomyInteractibaleBase nearstHomyInteractible;
  // Start is called before the first frame update
  float noticeDistance = 10.0f;
  public bool hasInteracted;
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public virtual void Interract()
  {

  }

  public void IsHomyNoticedObject()
  {
    if (HomyThrow.instance == null)
      return;

    if (!HomyThrow.instance.isFly )
      return;

    if( Vector3.Distance(  HomyThrow.instance.transform.position, transform.position ) < noticeDistance)
    {
      HomyThrow.instance.SetNoticedObjectState();
      nearstHomyInteractible = this;
    }
  }
}
