using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestSystem : MonoBehaviour {

  public static QuestSystem instance;

  public List<Image> QuestImages;
  public List<Image> QuestIcons;
  public List<Text> QuestNames;
  public List<Quest> activeQuest;



  // Use this for initialization
  void Start()
  {
    instance = this;
    activeQuest = new List<Quest>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.QUEST_ICON_CLICKED, ShowQuestInformation);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.QUEST_ICON_CLICKED, ShowQuestInformation);
  }


  public void AddActiveQuest(Quest quest)
  {
    for (int i = 0; i < activeQuest.Count; i++)
    {
      if (activeQuest[i] == quest)
        return;
    }
    activeQuest.Add(quest);
    SetQuestsImages();
  }

  public void RemoveActiveQuest(Quest quest)
  {
    for (int i = 0; i < activeQuest.Count; i++)
    {
      if (activeQuest[i] == quest)
      {
        activeQuest.RemoveAt(i);
        break;
      }
    }
    SetQuestsImages();
  }

  void SetQuestsImages()
  {
    int activeQuestIndex = activeQuest.Count - 1;
    for  (int i = 0; i < QuestImages.Count; i++ )
    {
      if ( activeQuestIndex < 0  )
      {
        QuestImages[i].gameObject.SetActive(false);
      }
      else
      {
        QuestImages[i].gameObject.SetActive(true);
        QuestIcons[i].sprite = activeQuest[activeQuestIndex].questIcon;
        activeQuestIndex--;
      }
    }
  }
  
  void ShowQuestInformation()
  {
    Quest quest = activeQuest[GameSystem.QuestIconClicked];
    GameUIController.instance.ShowQuestInformation(quest);
  }

}
