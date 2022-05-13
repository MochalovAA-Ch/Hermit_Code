using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HorizontalScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public RectTransform ScrollPanel;
  public Image[] buttons;
  public RectTransform center;
  float[] distance;
  bool dragging = false;
  int btnDistance;
  int minButtonNum;
  float newX, newXY;
  float minDistance;
  Vector2 newPosition;
  // Use this for initialization
  void Start()
  {
    dragging = false;
    int btnLength = buttons.Length;
    distance = new float[btnLength];
    btnDistance = (int)Mathf.Abs(buttons[2].GetComponent<RectTransform>().anchoredPosition.x - buttons[1].GetComponent<RectTransform>().anchoredPosition.x);
    ScrollPanel.anchoredPosition = new Vector2(-buttons[0].GetComponent<RectTransform>().anchoredPosition.x,
        ScrollPanel.anchoredPosition.y);

  }

  // Update is called once per frame
  void Update()
  {
    for (int i = 0; i < buttons.Length; i++)
    {

      distance[i] = Mathf.Abs(center.transform.position.x - buttons[i].transform.position.x);
    }
    minDistance = Mathf.Min(distance);

    for (int a = 0; a < buttons.Length; a++)
    {
      if (minDistance == distance[a])
      {

        minButtonNum = a;
      }
    }

    if (!dragging)
    {
      //	Debug.Log ((minButtonNum * -btnDistance).ToString ());
      //	LerpToButton (minButtonNum * -btnDistance);
      LerpToButton(minButtonNum * -btnDistance);
      //	StartCoroutine("scrolling");
    }

  }

  void LerpToButton(int position)
  {
    //Debug.Log (minButtonNum.ToString ());
    //newX = Mathf.Lerp (ScrollPanel.anchoredPosition.x, position, Time.deltaTime * 10f);

    //	newX = position * -btnDistance;
    newX = Mathf.Lerp(ScrollPanel.anchoredPosition.x, position, Time.deltaTime * 10.0f);
    //	Debug.Log (newX.ToString ());
    newPosition = new Vector2(newX, ScrollPanel.anchoredPosition.y);
    ScrollPanel.anchoredPosition = newPosition;

  }
  /*	void LerpToButton(int buttNum){
      //Debug.Log (minButtonNum.ToString ());
      newX = buttNum * -btnDistance;
      //newXY = Mathf.Lerp (ScrollPanel.anchoredPosition.x, newX, Time.deltaTime * 10f);
      Debug.Log (newX.ToString ());
      newPosition = new Vector2 (newX, ScrollPanel.anchoredPosition.y);
      ScrollPanel.anchoredPosition = newPosition;

    }*/


  public void StartDrag()
  {
    dragging = true;
  }

  public void EndDrag()
  {
    dragging = false;
  }

  /*	IEnumerator scrolling(){
      newX = minButtonNum * -btnDistance;
      while (ScrollPanel.anchoredPosition.x <= newX + 5 && ScrollPanel.anchoredPosition.x >= newX - 5) {

          ScrollPanel.anchoredPosition += new Vector2 (0.1f, 0.0f);

        yield return null;
      }

      ScrollPanel.anchoredPosition = new Vector2 (newX, 0.0f);
    }*/

  public void OnPointerDown(PointerEventData pointerData)
  {
    dragging = true;

  }


  public void OnPointerUp(PointerEventData pointerData)
  {
    dragging = false;
  }
}
