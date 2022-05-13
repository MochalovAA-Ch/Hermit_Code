using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml; //Needed for XML functionality
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization; //Needed for XML Functionality

public enum QuestId
{
  QUEST_WIZAR_LIES
}

public enum HintsIds
{

}

public class QuestResources
{
  public List<string> QuestName;
  public List<string> QuestGiverName;
  public List<List<string>> dialogList;
  public List<string> Summary;
  public List<string> Description;
  public List<List<string>> RewardTextList;
  public List<string> QuestInProccesText;
  public List<string> objectiveName;

  public QuestResources()
  {
    QuestName = new List<string>();
    QuestGiverName = new List<string>();
    dialogList = new List<List<string>>();
    Summary = new List<string>();
    Description = new List<string>();
    RewardTextList = new List<List<string>>();
    QuestInProccesText = new List<string>();
    objectiveName = new List<string>();
  }
}

public class HintResources
{
  public List<List<string>> hintList;

  public HintResources()
  {
    hintList = new List<List<string>>();
  }

}


public static class LangResources
{
  static Dictionary<int, QuestResources> questResources;
  static Dictionary<int, HintResources> hintResources;

  public static bool isCreated;

  static LangResources()
  {
    //Инициализация квестов
    questResources = new Dictionary<int, QuestResources>();
    hintResources = new Dictionary<int, HintResources>();
    isCreated = false;
    //Инициализация подсказок
  }

  public static string GetQuestName( int questid )
  {
    return questResources[questid].QuestName[GameSystem.language];
  }

  public static string GetQuestGiverName(int questid )
  {
    return questResources[questid].QuestGiverName[GameSystem.language];
  }

  public static string GetQuestSummary(int questid)
  {
    return questResources[questid].Summary[GameSystem.language];
  }

  public static string GetQuestDescription(int questid)
  {
    return questResources[questid].Description[GameSystem.language];
  }

  public static string GetQuestRewarText(int questid, int replicIndex )
  {
    return questResources[questid].RewardTextList[replicIndex][GameSystem.language];
  }

  public static string GetQuestProggresText(int questid)
  {
    return questResources[questid].QuestInProccesText[GameSystem.language];
  }

  public static string GetQuestDialogReplic( int questId, int replicIndex )
  {
    return questResources[questId].dialogList[replicIndex][GameSystem.language];
  }

  public static string GetQuestObjectiveName(int questId )
  {
    return questResources[questId].objectiveName[GameSystem.language];
  }

  public static int GetQestDialogCount( int questId )
  {
    return questResources[questId].dialogList.Count;
  }

  public static int GetHintCount( int hintId)
  {
if (!hintResources.ContainsKey(hintId) )
      return 0;
    return hintResources[hintId].hintList.Count;
  }

  public static int GetRewardTextCount(int hintId)
  {
    return questResources[hintId].RewardTextList.Count;
  }

  public static string GetHintText( int hintId, int hintTextIndex )
  {
    if ( !hintResources.ContainsKey(hintId) )
      return "empty text";
    return hintResources[hintId].hintList[hintTextIndex][GameSystem.language];
  }

  public static void InitLangResoursec()
  {
    XmlDocument xmlDoc = new XmlDocument();
#if UNITY_EDITOR
    xmlDoc.Load("Assets/Resources/langRes.xml");
#else
    TextAsset textAsset = (TextAsset)Resources.Load("langRes", typeof(TextAsset));
    xmlDoc.LoadXml(textAsset.text);
#endif
    XmlNode root = xmlDoc.ChildNodes[1];

    foreach (XmlNode xmlElem  in root.ChildNodes )
    {
      if( xmlElem.Name == "QuestList" )
      {
        foreach(XmlNode xmlQuest in xmlElem.ChildNodes )
        {
          
          int id = XmlConvert.ToInt32(xmlQuest.Attributes[0].Value);
          QuestResources questRes= new QuestResources();
          questResources.Add(id, questRes);

          foreach (XmlNode xmlNode in xmlQuest.ChildNodes)
          {

            if( xmlNode.Name == "QuestName" )
            {
              foreach(XmlNode langItem in xmlNode.ChildNodes )
              {
                questRes.QuestName.Add(langItem.InnerText);
              }
              continue;
            }

            if (xmlNode.Name == "QuestGiverName")
            {
              foreach (XmlNode langItem in xmlNode.ChildNodes)
              {
                questRes.QuestGiverName.Add(langItem.InnerText);
              }
              continue;
            }
            if (xmlNode.Name == "QuestRewardText")
            {
              foreach (XmlNode rewardText in xmlNode.ChildNodes)
              {
                List<string> langItems = new List<string>();
                foreach (XmlNode langItem in rewardText.ChildNodes)
                {
                  langItems.Add(langItem.InnerText);
                }
                questRes.RewardTextList.Add(langItems);
              }
            }
            if (xmlNode.Name == "QuestDescription")
            {
              foreach (XmlNode langItem in xmlNode.ChildNodes)
              {
                questRes.Description.Add(langItem.InnerText);
              }
              continue;
            }
            if (xmlNode.Name == "QuestSummary")
            {
              foreach (XmlNode langItem in xmlNode.ChildNodes)
              {
                questRes.Summary.Add(langItem.InnerText);
              }
              continue;
            }
            if (xmlNode.Name == "QuestInProggresText")
            {
              foreach (XmlNode langItem in xmlNode.ChildNodes)
              {
                questRes.QuestInProccesText.Add(langItem.InnerText);
              }
              continue;
            }
            if (xmlNode.Name == "QuestObjectiveName")
            {
              foreach (XmlNode langItem in xmlNode.ChildNodes)
              {
                questRes.objectiveName.Add(langItem.InnerText);
              }
              continue;
            }
            if (xmlNode.Name == "QuestDialogList")
            {
              foreach (XmlNode dialog in xmlNode.ChildNodes)
              {
                List<string> langItems = new List<string>();
                foreach (XmlNode langItem in dialog.ChildNodes )
                {
                  langItems.Add(langItem.InnerText);
                }
                questRes.dialogList.Add(langItems);
              }
              continue;
            }
          }
        }
        continue;
      }

      if(xmlElem.Name == "HintList")
      {
        foreach(XmlNode hint in xmlElem.ChildNodes )
        {
          int id = XmlConvert.ToInt32(hint.Attributes[0].Value);
          HintResources hintRes = new HintResources();
          hintResources.Add(id, hintRes);
          foreach(XmlNode hintText in hint.ChildNodes )
          {
            List<string> langItems = new List<string>();
            foreach(XmlNode langItem in hintText.ChildNodes )
            {
              langItems.Add(langItem.InnerText);
            }
            hintRes.hintList.Add(langItems);
          }
        }
      }
    }
  }
}
