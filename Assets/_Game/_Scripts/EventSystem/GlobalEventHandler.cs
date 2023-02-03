// This is a C# Event Handler (notification center) for Unity. It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other.

using System;
using System.Collections.Generic;

namespace SovereignStudios.EventSystem
{
    // These are callbacks (delegates) that can be used by the messengers defined in EventHandler class below
    public delegate void Callback(Object arg);
    public delegate object CallbackWithReturnType(Object args);

    /*** A handler for events that have one parameter of type T. ***/
    public static class GlobalEventHandler
    {

        private static Dictionary<EventID, Delegate> eventTable = new Dictionary<EventID, Delegate>();
        public static void AddListener(EventID eventType, Callback handler)
        {
            // Obtain a lock on the event table to keep this thread-safe.
            // Obtain a lock on the event table to keep this thread-safe.
            lock (eventTable)
            {
                // Create an entry for this event type if it doesn't already exist.
                if (!eventTable.ContainsKey(eventType))
                {
                    eventTable.Add(eventType, null);
                }
                // Add the handler to the event.
                eventTable[eventType] = (Callback)eventTable[eventType] + handler;
            }
        }
        public static void RemoveListener(EventID eventType, Callback handler)
        {
            // Obtain a lock on the event table to keep this thread-safe.
            lock (eventTable)
            {
                // Only take action if this event type exists.
                if (eventTable.ContainsKey(eventType))
                {
                    // Remove the event handler from this event.
                    eventTable[eventType] = (Callback)eventTable[eventType] - handler;

                    // If there's nothing left then remove the event type from the event table.
                    if (eventTable[eventType] == null)
                    {
                        eventTable.Remove(eventType);
                    }
                }
            }
        }
        public static void AddListener(EventID eventType, CallbackWithReturnType handler)
        {
            // Obtain a lock on the event table to keep this thread-safe.
            // Obtain a lock on the event table to keep this thread-safe.
            lock (eventTable)
            {
                // Create an entry for this event type if it doesn't already exist.
                if (!eventTable.ContainsKey(eventType))
                {
                    eventTable.Add(eventType, null);
                }
                // Add the handler to the event.
                //If there are multiple methods bind to the same event then the last binded method's returned value will be sent.
                eventTable[eventType] = (CallbackWithReturnType)eventTable[eventType] + handler;
            }
        }

        public static void RemoveListener(EventID eventType, CallbackWithReturnType handler)
        {
            // Obtain a lock on the event table to keep this thread-safe.
            lock (eventTable)
            {
                // Only take action if this event type exists.
                if (eventTable.ContainsKey(eventType))
                {
                    // Remove the event handler from this event.
                    eventTable[eventType] = (CallbackWithReturnType)eventTable[eventType] - handler;

                    // If there's nothing left then remove the event type from the event table.
                    if (eventTable[eventType] == null)
                    {
                        eventTable.Remove(eventType);
                    }
                }
            }
        }
        public static void TriggerEvent(EventID eventType, System.Object arg = null)
        {
            Delegate d;
            // Invoke the delegate only if the event type is in the dictionary.
            if (eventTable.TryGetValue(eventType, out d))
            {
                // Take a local copy to prevent a race condition if another thread
                // were to unsubscribe from this event.
                Callback callback = (Callback)d;

                // Invoke the delegate if it's not null.
                if (callback != null)
                {
                    callback(arg);
                }
            }
        }
        public static object TriggerEventForReturnType(EventID eventType, object arg = null)
        {
            Delegate d;
            object returnValue = null;
            // Invoke the delegate only if the event type is in the dictionary.
            if (eventTable.TryGetValue(eventType, out d))
            {
                // Take a local copy to prevent a race condition if another thread
                // were to unsubscribe from this event.
                CallbackWithReturnType callback = (CallbackWithReturnType)d;

                // Invoke the delegate if it's not null.
                if (callback != null)
                {
                    returnValue = callback(arg);
                }
            }
            //If there are multiple methods bind to the same event then the last binded method's returned value will be sent.
            return returnValue;
        }

        public static void CleanUpTable()
        {
            eventTable.Clear();
        }
    }
    public enum EventID
    {

        //Level
        EVENT_ON_LEVEL_STARTED,
        EVEN_ON_LEVEL_FINISHED,
        EVENT_ON_LEVEL_EXITED,
        EVENT_RESTART_LEVEL_REQUESTED,

        //Player
        EVENT_ON_PLAYER_WIN,

        //Spot point
        EVENT_ON_SPOTPOINT_CLICKED,
        EVENT_ON_SPOTPOINT_SELECTED,
        EVENT_ON_SPOTPOINT_SELECTION_ENDED,
        EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC,

        //Goat
        EVENT_ON_GOAT_ONBOARDING_REQUESTED,
        EVENT_ANIMAL_ONBOARDED,
        EVENT_ON_GOAT_TURN,
        EVENT_ON_GOAT_DEAD_POINT_DETECTED,
        EVENT_ON_GOAT_KILLED,


        //Tiger
        EVENT_ON_TIGER_TURN,
        EVENT_ON_TIGER_BLOCKED,
        EVENT_ON_TIGER_UNBLOCKED,

        //Gameplay
        EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY,
        EVENT_REQUEST_TO_CHANGE_PLAYER_TURN,
        EVENT_ON_ANIMAL_MOVED,

        //ScreenManager
        EVENT_ON_CHANGE_SCREEN_REQUESTED,
        EVENT_REQUEST_TO_CHANGE_SCREEN_WITH_TRANSITION,
        EVENT_ON_REMOVE_ALL_SCREENS_REQUESTED,
        EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN,
        EVENT_REQUEST_GENERIC_POPUP_SETUP,
        EVENT_REQUEST_SCREEN_TRANSITION,
        EVENT_REQUEST_GET_CURRENT_SCREEN,
        EVENT_REQUEST_GE_PREVIOUS_SCREEN,


        //Fade Screen
        EVENT_REQEST_FADE_SCREEN_IN,
        EVENT_REQEST_FADE_SCREEN_OUT,
        EVENT_REQUEST_SCREEN_BLINK,
        EVENT_REQUEST_SCREEN_BLOCK,
        EVENT_REQUEST_SCREEN_UNBLOCK,
        EVENT_REQUEST_TO_KILL_TURN_TIMER_TWEENING,

        ////ADS 
        EVENT_ON_SHOW_BANNER_AD_REQUESTED,
        EVENT_ON_HIDE_BANNER_AD_REQUESTED,
        EVENT_ON_LOAD_MREC_AD_REQUESTED,
        EVENT_ON_SHOW_MREC_AD_REQUESTED,
        EVENT_ON_HIDE_MREC_AD_REQUESTED,
        EVENT_ON_SHOW_INTERSTITIAL_AD_REQUESTED,
        EVENT_ON_SHOW_REWARDED_AD_REQUESTED,
        EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED,
        EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED,
        EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED,
        EVENT_ON_REWARDED_AD_AVAILABILITY_REQUESTED,
        EVENT_ON_INTERSTITIAL_AD_AVAILABILITY_REQUESTED,

        //Ad State
        EVENT_ON_AD_STATE_CHANGED,
    }
}


/*
// These are callbacks (delegates) that can be used by the messengers defined in EventHandler class below
   public delegate void Callback(System.Object arg);
   public delegate object CallbackWithReturnType(System.Object arg);


public static class DeftEventHandler
{

   private static Dictionary<EventID, Dictionary<int, List<Delegate>>> eventTable = new Dictionary<EventID, Dictionary<int, List<Delegate>>>();
   public static Dictionary<EventID, Dictionary<int, List<Delegate>>> EventTable => eventTable;
   public static void AddListener(EventID eventType, Callback handler, int priority = 5)
   {
       // Obtain a lock on the event table to keep this thread-safe.
       lock (eventTable)
       {

           if (!eventTable.ContainsKey(eventType))
           {
               eventTable.Add(eventType, new Dictionary<int, List<Delegate>>());
           }

           Dictionary<int, List<Delegate>> value = eventTable[eventType];

           if (!value.ContainsKey(priority))
           {
               value.Add(priority, new List<Delegate>());
           }

           value[priority].Add(handler);

       }
   }

   public static void RemoveListener(EventID eventType, Callback handler)
   {
       // Obtain a lock on the event table to keep this thread-safe.
       lock (eventTable)
       {
           if (eventTable.ContainsKey(eventType))
           {
               Dictionary<int, List<Delegate>> value = eventTable[eventType];

               foreach (KeyValuePair<int, List<Delegate>> entry in value)
               {
                   entry.Value.Remove(handler);
               }
           }
       }
   }

   public static void TriggerEvent(EventID eventType, System.Object arg = null)
   {
       if (eventTable.ContainsKey(eventType))
       {
           Dictionary<int, List<Delegate>> value = eventTable[eventType];

           foreach (KeyValuePair<int, List<Delegate>> entry in value.OrderByDescending(x => x.Key))
           {
               foreach (Delegate observer in entry.Value)
               {
                   observer.DynamicInvoke(arg);
               }
           }
       }
   }

   public static void CleanUpTable()
   {
       eventTable.Clear();
   }

   #region Return Type 
   public static void AddListener(EventID eventType, CallbackWithReturnType handler, int priority = 5)
   {
       // Obtain a lock on the event table to keep this thread-safe.
       lock (eventTable)
       {

           if (!eventTable.ContainsKey(eventType))
           {
               eventTable.Add(eventType, new Dictionary<int, List<Delegate>>());
           }

           Dictionary<int, List<Delegate>> value = eventTable[eventType];

           if (!value.ContainsKey(priority))
           {
               value.Add(priority, new List<Delegate>());
           }

           value[priority].Add(handler);

       }
   }

   public static void RemoveListener(EventID eventType, CallbackWithReturnType handler)
   {
       // Obtain a lock on the event table to keep this thread-safe.
       lock (eventTable)
       {
           if (eventTable.ContainsKey(eventType))
           {
               Dictionary<int, List<Delegate>> value = eventTable[eventType];

               foreach (KeyValuePair<int, List<Delegate>> entry in value)
               {
                   entry.Value.Remove(handler);
               }
           }
       }
   }

   public static object TriggerEventForReturnType(EventID eventType, System.Object arg = null)
   {
       object returnValue = null;
       if (eventTable.ContainsKey(eventType))
       {
           Dictionary<int, List<Delegate>> value = eventTable[eventType];

           foreach (KeyValuePair<int, List<Delegate>> entry in value.OrderByDescending(x => x.Key))
           {
               foreach (Delegate observer in entry.Value)
               {
                   returnValue = observer.DynamicInvoke(arg);
               }
           }
       }
       return returnValue;
   }
   #endregion


}





*/