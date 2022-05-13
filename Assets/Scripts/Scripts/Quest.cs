using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestStatus
{
  Acceptable,
  Accepted,        //Квест взят
  Done,            //Квест выполнен, но не сдан
  Completed        //Квест сдан
}

public enum QuestType
{
  COLLECT_ITEM
}

public enum QuestRewardType
{
  KEY,
  COINS
}

public enum QuestIds
{
  WIZARD_LIES = 0,
  TRABAR_SAWMILL = 1,
  MUSICAN_WITHOUT_INSTRUMENT = 2
}


public class Quest : MonoBehaviour
{
  public QuestIds questId;
  protected int id;
  public static int currentid;

  protected float temproraryHintTimer;

  public Sprite questIcon;

  protected int currentDialogReplic;
  int currentRewardReplic;

  public GameObject QuestRewardIcon;


  public int objectiveCount;
  public int currentObjectives;

  public QuestStatus status;
  public QuestRewardType reward;
  public Keys keyType;

  public int rewardCount;

  public Quest()
  {
    currentid++;
    id = currentid;
  }

  // Use this for initialization
  void Start()
  {

  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC, ShowNextQuestDialogReplic);
  }

  private void OnDestroy()
  {
    EventsManager.StopListening(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC, ShowNextQuestDialogReplic);
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void ShowNextQuestDialogReplic()
  {
    //StopCoroutine("DisplayHint");
    if (currentid != id)
    {
      return;
    }

    if( status == QuestStatus.Accepted )
    {
      CloseQuestDialog();
      return;
    }

    if( status == QuestStatus.Done )
    {
      ShowNextRewardDialogReplic();
      return;
    }

    if (currentDialogReplic == LangResources.GetQestDialogCount( (int)questId ) )
    {
      currentDialogReplic = 0;
      AcсeptQuest();
      return;
    }

    GameUIController.instance.SetQuestDialogText( LangResources.GetQuestDialogReplic( (int)questId, currentDialogReplic ) );
    currentDialogReplic++;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {

    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {

    }
  }

  public void AcсeptQuest()
  {
    status = QuestStatus.Accepted;
    EventsManager.TriggerEvent(EventsIds.QUEST_ACCEPTED);
    QuestSystem.instance.AddActiveQuest(this);
    CloseQuestDialog();
  }

  public void AchieveObjective()
  {
    currentObjectives++;
    CheckQuestStatus();
  }

  //Проверяем статус квеста, и в зависимости от его статуса выводим сообщения
  void CheckQuestStatus()
  {
    if( currentObjectives == objectiveCount )
    {
      if ( status == QuestStatus.Accepted )
      {
        GameUIController.instance.ShowFloatingText(LangResources.GetQuestName( (int) questId ) + ": " + currentObjectives.ToString() + "/" + objectiveCount.ToString() + " " + LangResources.GetQuestObjectiveName( (int) questId ) );
        status = QuestStatus.Done;
      }
    }
    else
    {
      if( status == QuestStatus.Accepted )
      {
        GameUIController.instance.ShowFloatingText( LangResources.GetQuestName((int)questId) + ": " + currentObjectives.ToString() + "/" + objectiveCount.ToString() + " " + LangResources.GetQuestObjectiveName((int)questId) );
      }
    }
  }

  public void ShowCompleteQuestDialog()
  {
    GameUIController.instance.ShowQuestDialog( LangResources.GetQuestGiverName( ( int ) questId ) );
    currentRewardReplic = 0;
    GameUIController.instance.SetQuestDialogText(LangResources.GetQuestRewarText( (int)questId, currentRewardReplic) );
    currentRewardReplic++;
  }

  public void ShowNextRewardDialogReplic()
  {
    if (currentid != id)
    {
      return;
    }

    if ( currentRewardReplic == LangResources.GetRewardTextCount((int)questId) )
    {
      currentDialogReplic = 0;
      CompleteQuest();
      return;
    }

    GameUIController.instance.SetQuestDialogText(LangResources.GetQuestRewarText((int)questId, currentRewardReplic));
    currentRewardReplic++;

  }

  public void CompleteQuest()
  {
    CloseQuestDialog();
    QuestRewardIcon.SetActive(false);

    if (reward == QuestRewardType.KEY)
    {
      GameSystem.collectedKeyIndex = keyType;
      GameSystem.playerKeys++;
      EventsManager.TriggerEvent(EventsIds.CHANGE_KEYS_COUNT);
    }

    if( reward == QuestRewardType.COINS )
    {
      GameSystem.collectedCoinsOnLevel += rewardCount;
      EventsManager.TriggerEvent(EventsIds.CHANGE_COINS_COUNT);
    }

    status = QuestStatus.Completed;

    QuestSystem.instance.RemoveActiveQuest(this);
  }


  public void Show()
  {
    StopCoroutine("DisplayHint");
    
  }

  public void ShowNextQuestReplic()
  {
    if (currentid != id)
    {
      return;
    }

    if (currentDialogReplic == LangResources.GetQestDialogCount((int)questId) )
    {
      currentDialogReplic = 0;
      CloseQuestDialog();
      return;
    }

    GameUIController.instance.SetQuestDialogText(LangResources.GetQuestDialogReplic( (int) questId, currentDialogReplic ) );
    currentDialogReplic++;
  }


  public void ShowQuestDialog()
  {
    if ( status == QuestStatus.Acceptable )
    {
      currentDialogReplic = 0;
      currentid = id;
      GameUIController.instance.ShowQuestDialog(LangResources.GetQuestGiverName( (int)questId) );
      ShowNextQuestReplic();
    }
    else if( status == QuestStatus.Accepted )
    {
      Debug.Log("Активный  квест квест");
      currentid = id;
      GameUIController.instance.ShowQuestDialog(LangResources.GetQuestGiverName((int)questId));
      GameUIController.instance.SetQuestDialogText(LangResources.GetQuestProggresText( (int)questId) );
    }
  }

  public void CloseQuestDialog()
  {
    GameUIController.instance.CloseQuestDialog();
  }
}
