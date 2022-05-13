using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog: MonoBehaviour
{
  protected int id;
  public static int currentid;
  
  protected float temproraryHintTimer;



  [TextArea]
  public List<string> dialogList;

  protected int currentDialogReplic;

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC, ShowNextDialogReplic);
  }

  private void OnDisable()
  {
    EventsManager.StopListening(EventsIds.SHOW_NEXT_QUEST_DIALOG_REPLIC, ShowNextDialogReplic);
  }

  public Dialog()
  {
    currentid++;
    id = currentid;
    temproraryHintTimer = 3.0f;
  }

  public virtual void DialogEnd()
  {

  }

  public void ShowDialog()
  {
    //StopCoroutine("DisplayHint");
    currentDialogReplic = 0;
    currentid = id;
    GameUIController.instance.ShowDialog();
    ShowNextDialogReplic();
  }

  public void ShowNextDialogReplic()
  {
    //StopCoroutine("DisplayHint");
    if (currentid != id)
    {
      return;
    }

    if (currentDialogReplic == dialogList.Count)
    {
      currentDialogReplic = 0;
      CloseDialog();
      DialogEnd();
      return;
    }

    GameUIController.instance.SetDialogText( dialogList[currentDialogReplic] );
    currentDialogReplic++;
  }

  public void CloseDialog()
  {
    GameUIController.instance.CloseDialog();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      ShowDialog();
    }
  }
}
