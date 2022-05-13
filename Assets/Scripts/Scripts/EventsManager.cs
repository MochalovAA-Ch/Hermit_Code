using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour {

  Dictionary< EventsIds, UnityEvent> eventDictionary;
  
  private static EventsManager eventManager;

  //Получение ссылки на EventsManager
  public static EventsManager instance
  {
    get
    {
      //Если не установили ссылку на EventManager
      if ( !eventManager )
      {
        //Ищем объект EventManager на сцене
        eventManager = FindObjectOfType(typeof( EventsManager )) as EventsManager;
        
        //Если нет объекта EventManager, выводим ошибку
        if( !eventManager )
        {
          Debug.LogError("Отсутствует GameObject с прикрепленным EventsManager ");
        }
        else
        {
          eventManager.Init();
        }
      }
      return eventManager;
    }
  }//конец instance

  void Init()
  {
    if ( eventDictionary == null )
    {
      eventDictionary = new Dictionary< EventsIds, UnityEvent>();
    }
  }

  //Начать слушать событие
  public static void StartListening( EventsIds eventId, UnityAction listener )
  {
    UnityEvent thisEvent = null;
    if ( instance.eventDictionary.TryGetValue(eventId, out thisEvent ) )
    {
      thisEvent.AddListener( listener );
    }
    else
    {
      thisEvent = new UnityEvent();
      thisEvent.AddListener(listener);
      instance.eventDictionary.Add( eventId, thisEvent );
    }
  }

  //Завершить прослушивание события
  public static void StopListening( EventsIds eventId, UnityAction listener )
  {
    if (eventManager == null) return;
    UnityEvent thisEvent = null;
    if (instance.eventDictionary.TryGetValue( eventId, out thisEvent) )
    {
      thisEvent.RemoveListener(listener);
    }
  }

  //Инициировать событие
  public static void TriggerEvent( EventsIds eventId)
  {
    UnityEvent thisEvent = null;
    if( instance.eventDictionary.TryGetValue( eventId, out thisEvent ) )
    {
      thisEvent.Invoke();
    }
  }
}

public enum EventsIds
{
  DECREASE_LIVES,
  INCREASE_LIVES,
  INCREASE_COINS,
  DECREASE_COINS,
  CHANGE_COINS_COUNT,
  CHANGE_KEYS_COUNT,
  GAME_OVER,
  MOVING_PLATFORM_VELOCITY_CHANGED,
  JUMP_FROM_CLIMB,
  JUMP_ON_CLIMB,
  STOP_SMOOTH_CAM_BEHIND_PLAYER,
  KNOCKBACK,
  CUPOL_BUTTON_BOX_ENTER,
  CUPOL_BUTTON_BOX_EXIT,
  CAMERA_TO_PLAYER_BEHIND,
  SELECT_CLOTH,
  PIKC_UP_TELEKENESIS,
  SHOW_NEXT_QUEST_DIALOG_REPLIC,
  QUEST_ACCEPTED,
  PLAYER_ENTER_QUEST_AREA,
  PLAYER_EXIT_QUEST_AREA,
  END_LEVEL_STAND_ACTIVATED,
  QUEST_ICON_CLICKED,
  SHOW_NEXT_HINT_REPLIC,
  SHOW_NEXT_LEVEL,
  SHOW_PREV_LEVEL,
  PLAY_LEVEL,
  LANG_CHANGE,
  CLOSE_REWARD_VIDEO,
  STAFF_HIT,
  PUT_ON_CURRENT_CLOTH_SET,
  STAFF_START_HIT,
  STAFF_END_HIT,
  CHANGE_STAFF_COLLIDER
}
