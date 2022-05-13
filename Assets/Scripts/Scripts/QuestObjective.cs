using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour {

  public Quest quest;           //Квест в которому относится объект квеста
  public OutlineController outLineController;
  bool active;

  // Use this for initialization
  void Start()
  {

  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.QUEST_ACCEPTED, CheckQuestForActive);
  }

  private void OnDisable()
  {

  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.QUEST_ACCEPTED, CheckQuestForActive);
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Player")
    {
      return;
    }

    if (!active)
    {
      if (GameSystem.language == 0)
      {
        GameUIController.instance.ShowFloatingText("Хм... Для чего это здесь?");

      }
      else
      {
        GameUIController.instance.ShowFloatingText("Hm... Why is it here?");
      }
    }
    else
    {
      AchieveObjective();
    }

  }

  void CheckQuestForActive()
  {
    if (quest.status == QuestStatus.Accepted)
    {
      SetQuestObjectiveActive();
    }
  }

  void SetQuestObjectiveActive()
  {
    active = true;
    outLineController.enabled = true;
  }

  void AchieveObjective()
  {
    HermitSoundManager.instance.GetSmth();
    quest.AchieveObjective();
    Destroy(gameObject);
  }
}
