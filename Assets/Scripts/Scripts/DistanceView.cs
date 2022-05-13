using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceView : MonoBehaviour {

  public float Distance1;
  public float grassDistance;

  Transform player;

  public List<GameObject> distance1Objects;

  List<DistanceViewItem> distanceItemsList;

  List<DistanceViewItem> removeList;

  public List<GameObject> grassList;
  List<DistanceViewItem> grassItemsList;

  bool isCorutineStarted;
  float Hidetimer;
  float HideTime = 1.0f;
  // Use this for initialization
  void Start()
  {
    removeList = new List<DistanceViewItem>();
    if (distance1Objects.Count > 0)
    {
      distanceItemsList = new List<DistanceViewItem>();
      grassItemsList = new List<DistanceViewItem>();
      for (int i = 0; i < distance1Objects.Count; i++)
      {
        if (distance1Objects[i] == null)
          continue;
        distanceItemsList.Add(new DistanceViewItem { position = distance1Objects[i].transform.position, item = distance1Objects[i] });
      }

      for (int i = 0; i < grassList.Count; i++)
      {
        if (grassList[i] == null)
          continue;
        grassItemsList.Add(new DistanceViewItem { position = grassList[i].transform.position, item = grassList[i] });
      }
    }
    player = FindObjectOfType<CharacterControllerScript>().transform;
  }

  private void Update()
  {
    if (isCorutineStarted)
      return;

    if( Hidetimer < HideTime )
    {
      Hidetimer += Time.deltaTime;
    }
    else
    {
      if( !isCorutineStarted )
      {
        isCorutineStarted = true;
        StartCoroutine("CheckDistance1");
      }
    }
  }

  IEnumerator CheckDistance1()
  {
    if(distanceItemsList.Count > 0 )
    {
      foreach( DistanceViewItem distanceViewObject in distanceItemsList)
      {
        if (distanceViewObject.item == null)
        {
          removeList.Add(distanceViewObject);
        }
        else
        {
          if (Vector3.Distance(distanceViewObject.position, player.position) > Distance1 )
          {
            distanceViewObject.item.SetActive( false );
          }
          else
          {
            distanceViewObject.item.SetActive( true );
          }
        }
      }
    }

    foreach (DistanceViewItem distanceViewObject in grassItemsList)
    {
      if (Vector3.Distance(distanceViewObject.position, player.position) > grassDistance)
      {
        distanceViewObject.item.SetActive(false);
      }
      else
      {
        distanceViewObject.item.SetActive(true);
      }
    }

    yield return new WaitForSeconds(1.0f);
    
    for ( int i = 0; i < removeList.Count; i++ )
    {
      distanceItemsList.Remove(removeList[i]);
    }

    StartCoroutine("CheckDistance1");
  }
}

class DistanceViewItem
{
  public Vector3 position;
  public GameObject item;
}