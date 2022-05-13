using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

  public Image healthBarPartImage;
  public Canvas healthBarCanvas;

  Transform cameraTr;

  float canvasWidth;
  float partWidth;

  List<Image> hpBarsPartsList;
  List<Image> hpBarAnimQueue;
	// Use this for initialization
	void Start ()
  {
    cameraTr = Camera.main.transform; 
  }
	
	// Update is called once per frame
	void Update ()
  {
    transform.LookAt(cameraTr);
	}


  //Создает деления хелфбара
  public void CreateHealthBarsImages( int hitPoint )
  {
    hpBarsPartsList = new List<Image>();
    hpBarAnimQueue = new List<Image>();
    canvasWidth = healthBarCanvas.GetComponent<RectTransform>().sizeDelta.x;
    partWidth = canvasWidth / hitPoint;
    for( int i = 0; i < hitPoint; i++ )
    {
      Image partImage = (Image)Instantiate(healthBarPartImage, healthBarCanvas.transform.position, healthBarCanvas.transform.rotation, healthBarCanvas.transform);
      partImage.rectTransform.sizeDelta = new Vector2( partWidth,partImage.rectTransform.sizeDelta.y);
      partImage.rectTransform.localPosition = new Vector3(-canvasWidth*0.5f + partWidth * i + partWidth*0.5f, 0.0f, 0.0f);
      hpBarsPartsList.Add(partImage);
    }
    healthBarPartImage.gameObject.SetActive(false);
  }


  public void DecreaseHelth()
  {
    if (hpBarsPartsList.Count == 0)
      return;
    hpBarAnimQueue.Add(hpBarsPartsList[0]);
    hpBarsPartsList.RemoveAt(0);
    if(  !isAnimInProggres)
      StartCoroutine(decrLive());
  }

  bool isAnimInProggres = false;
  IEnumerator decrLive()
  {
    isAnimInProggres = true;

    Image currHpBar = hpBarAnimQueue[0];
    hpBarAnimQueue.RemoveAt(0);
    float animTime = 0.3f;
    float animTimer = 0.0f;

    float delatWidth = partWidth / animTime;
    while (animTimer < animTime)
    {
      currHpBar.rectTransform.sizeDelta -= new Vector2(delatWidth*Time.deltaTime, 0);
      currHpBar.rectTransform.localPosition += new Vector3(delatWidth * Time.deltaTime*0.5f, 0.0f, 0.0f);
      animTimer += Time.deltaTime;
      yield return null;
    }
    currHpBar.rectTransform.sizeDelta = Vector2.zero;
    if (hpBarAnimQueue.Count != 0)
      StartCoroutine(decrLive());

    isAnimInProggres = false;
  }
}
