using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangText : MonoBehaviour {

  public string RusText;
  public string EngText;
  Text langString;
  bool isLangApplyed;
	// Use this for initialization
	void Start () {
    langString = GetComponent<Text>();
	}

  private void OnEnable()
  {
    if( langString == null )
      langString = GetComponent<Text>();

    ApplyLanguage();
    EventsManager.StartListening(EventsIds.LANG_CHANGE, ApplyLanguage);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.LANG_CHANGE, ApplyLanguage);
  }

  // Update is called once per frame
  void Update () {
		if( !isLangApplyed )
    {
      ApplyLanguage();
      isLangApplyed = true;
    }
	}

  void ApplyLanguage()
  {
    if( GameSystem.language == 0)
    {
      langString.text = RusText;
    }
    if( GameSystem.language == 1 )
    {
      langString.text = EngText;
    }
  }
}
